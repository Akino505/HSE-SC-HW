# ПОС - Преподавательская Оценка Студентов

Домашнее задание №2 по курсу «Конструирование программного обеспечения» (DDD · архитектура · паттерны GoF).

Автор: _Ким Алина Алексеевна, БПИ253_

Консольное приложение на C# (.NET 8), в котором преподаватель ведёт рубрикатор (критерии оценивания), выставляет студентам оценки и получает итоговый балл по весам с выгрузкой в CSV/JSON.

---

## Содержание

1. [Возможности и запуск](#1-возможности)
2. [Логика решений](#2-логика-решений)
3. [Карта DDD](#3-карта-ddd)
4. [Архитектура и слои](#4-архитектура-и-слои)
5. [Паттерны GoF](#5-паттерны-gof)
6. [Тесты](#6-тесты)
7. [Роль ИИ в решении](#7-роль-ии-в-решении)

---

## 1. Возможности
Данные хранятся в файле `pos-data.json` в текущей папке (удалите файл, чтобы начать с нуля).

| Команда | Что делает |
|---|---|
| `students` / `add student` / `delete student` | список, добавление, удаление студента (вместе с его оценками) |
| `criteria` / `add criterion` / `delete criterion` | рубрикатор: список, добавление, удаление критерия (вместе с оценками по нему) |
| `grade` | выставить или **исправить** оценку студенту по критерию |
| `grades` | показать оценки студента |
| `cancel` | отменить оценку |
| `final` / `results` | итог одного студента / итоги всех |
| `export <csv\|json> <путь>` | выгрузка итогов в файл |
| `demo` | сквозной сценарий из ТЗ (см. ниже) |
| `help`, `exit` | справка, выход |

Студентов и критерии выбирают **по номеру из списка**, вводить GUID не нужно. Опасные действия (удаление) просят подтверждение `y/n`.

**Сквозной сценарий (`demo`):** завести студентов и критерии → выставить оценки → попытка поставить 12 из 10 (отклоняется доменом) → исправление оценки → итоги → экспорт в `demo-results.csv` и `demo-results.json`. Для демо нужна пустая база.

---

## 2. Логика решений

| Решение | Почему |
|---|---|
| **Итог не хранится, а вычисляется** из оценок студента и рубрикатора | Итог всегда согласован с оценками. |
| **Шкала итога 0-10**, формула `Σ(балл / максимум × вес) / Σ(веса) × 10`, затем округление до 2 знаков | Нормировка на максимум позволяет сравнивать критерии с разным максимумом; вес задаёт вклад в итог. |
| **Критерий, по которому оценки ещё нет, считается как 0** | Оценка по всему курсу. Есть альтернатива ("учитывать только уже оценённые"), реализована отдельной стратегией |
| **Оценка (`Evaluation`) - Value Object** | У факта нет собственной идентичности. Исправление = замена факта новым. |
| **Оценки лежат внутри агрегата `StudentGrades`** (ведомость одного студента) и ссылаются на критерий по `CriterionId` | Правило "одна оценка на критерий" охраняется одним корнем агрегата. Агрегаты ссылаются друг на друга только по идентификаторам. |
| **Оба инварианта из ТЗ проверяются в одном месте** - `EvaluationFactory` | Фабрике нужны одновременно рубрикатор и балл, поэтому она единственная точка создания оценки. |
| **Максимум и вес строго больше нуля** | Максимум 0 даёт деление на ноль в формуле и бессмысленный критерий; вес 0 означает "критерий не влияет на итог". |
| **Удаление критерия каскадно удаляет оценки по нему** (с подтверждением) | Иначе бы остались висячие оценки |

---
## 3. Карта DDD

### 3.1. Ubiquitous Language

Код использует те же слова, что и предметная область.

| Термин предметной области | В коде |
|---|---|
| студент | `Student` |
| критерий (название, максимум, вклад в итог) | `Criterion`, `MaxScore`, `Weight` |
| рубрикатор | `Rubric` |
| оценка (студент · критерий · балл, дата, комментарий) | `Evaluation`, `Score` |
| ведомость студента | `StudentGrades` |
| итог | `FinalScore`, `IFinalScoreCalculator` |

### 3.2. Entity и Value Object

| Тип | Класс | Обоснование |
|---|---|---|
| Entity | `Student` | Идентичность — `Id`. Два студента с одинаковым именем - разные люди. |
| Entity | `Criterion` | Есть `Id`, на него ссылаются оценки. |
| Value Object | `Score`, `MaxScore`, `Weight`, `FinalScore` | Равны по значению, неизменяемы, сами проверяют допустимость (`record`). |
| Value Object | `Evaluation` | Факт без идентичности; чтобы исправить оценку, её заменяют новой. |

### 3.3. Агрегаты и их границы

```
┌─ Student ─────────┐   ┌─ Rubric (корень) ──────────┐   ┌─ StudentGrades (корень) ───────┐
│ Id,     Name      │   │ Criteria: список Criterion │   │ StudentId                      │
└───────────────────┘   │  (Id, Name, MaxScore,      │   │ Evaluations: CriterionId →     │
                        │   Weight)                  │   │   Evaluation (Score, Date,     │
                        └────────────────────────────┘   │   Comment)                     │
                                    ▲                    └────────────────────────────────┘
                                    └── ссылка по CriterionId ─────┘
```

- **`Student`**, **`Rubric`**, **`StudentGrades`** - три отдельных агрегата, один сценарий меняет один агрегат.
- Между агрегатами только ссылки по `Id` (`StudentGrades` знает `StudentId`, `Evaluation` знает `CriterionId`).
- Оценки не хранят `StudentId`: они лежат внутри ведомости конкретного студента, поэтому положить оценку одного студента в ведомость другого невозможно.

### 3.4. Инварианты

| Инвариант | Где охраняется | Тест |
|---|---|---|
| балл >= 0 | конструктор `Score` | `EvaluationFactoryTests.Create_NegativeScore_Throws` |
| балл <= максимума критерия | `MaxScore.Allows` + `EvaluationFactory` | `Create_ScoreAboveMax_Throws` |
| оценка только по критерию из рубрикатора | `EvaluationFactory` (`rubric.GetCriterion`) | `Create_CriterionNotInRubric_Throws`, `DeleteCriterion_ThenGradingIt_IsRejected` |
| максимум > 0, вес > 0 | `MaxScore`, `Weight` | `RubricTests.AddCriterion_NonPositiveMaxScoreOrWeight_Throws` |
| название критерия уникально в рубрикаторе | `Rubric` | `AddCriterion_DuplicateName_Throws` |
| одна оценка на критерий в ведомости | `StudentGrades` (словарь по `CriterionId`) | `Put_SameCriterionTwice_ReplacesGrade` |
| нельзя отменить несуществующую оценку | `StudentGrades.Cancel` | `Cancel_NoGrade_Throws` |
| итог согласован с оценками | итог вычисляется Domain Service и не хранится | `FinalScoreCalculatorTests` |
| после удаления критерия нет оценок по нему | `RubricService.DeleteCriterion` (каскад) | `RubricServiceTests` |

### 3.5. Repository (порты)

Интерфейсы лежат в домене (`Domain/Repositories`), реализации - снаружи, в Infrastructure:
`IStudentRepository`, `IRubricRepository`, `IGradesRepository`. Домен ничего не знает о JSON и файлах.

### 3.6. Domain Service

`IFinalScoreCalculator.Calculate(rubric, grades)` - расчёт итога. Правило требует **двух агрегатов** (рубрикатор и ведомость), поэтому не принадлежит ни одному из них. Domain Service не знает о репозиториях: получает объекты и возвращает результат.

---

## 4. Архитектура и слои

Выбрана архитектура **Ports and Adapters**.

```
 Presentation ──► Application ──► Domain ◄── (реализует порты) ── Infrastructure
   ConsoleUi       use cases       модель,                          JSON, CSV/JSON-
                                   инварианты,                      экспорт, DTO,
                                   Domain Service,                  маппинг
                                   порты репозиториев
```

**Стрелка зависимостей направлена внутрь:** Domain не зависит ни от кого; Application зависит только от Domain; Infrastructure реализует интерфейсы, которые определены в Domain и Application; Presentation вызывает только интерфейсы Application. Единственное место, где известны все конкретные классы, - `Program.cs` (composition root).

```
HW-2-POS-SRC/
  Domain/
    Models/          Student, Criterion, Rubric, StudentGrades, Evaluation, Score, MaxScore, Weight, FinalScore
    Factories/       EvaluationFactory
    Services/        IFinalScoreCalculator, WeightedAverageCalculator, WeightedRatedOnlyCalculator, RoundingCalculator
    Repositories/    IStudentRepository, IRubricRepository, IGradesRepository   ← порты
  Application/
    Services/        StudentService, RubricService, GradingService, ReportService (+ интерфейсы)
    Models/          StudentResult, GradeLine   ← модели для чтения
    Ports/           IResultExporter            ← порт выгрузки
  Infrastructure/
    Storage/         IDataStorage, JsonStorage
    Dtos/, Mappers/  DTO ↔ домен (через Restore)
    Repositories/    Json*Repository            ← адаптеры
    Export/          ResultExporter, CSVResultExporter, JSONResultExporter   ← адаптеры
  Presentation/      ConsoleUI
  Program.cs         composition root (DI-контейнер)
HW-2-POS.Tests/        xUnit-тесты
```

| Порт (интерфейс) | Адаптер (реализация) |
|---|---|
| `IStudentRepository`, `IRubricRepository`, `IGradesRepository` | `JsonStudentRepository`, `JsonRubricRepository`, `JsonGradesRepository` |
| `IResultExporter` | `CSVResultExporter`, `JSONResultExporter` |

**DI-контейнер** - `Microsoft.Extensions.DependencyInjection`. В `Program.cs` регистрируются реализации; именно там выбираются формула итога и форматы выгрузки:

```csharp
services.AddSingleton<IFinalScoreCalculator>(
    s => new RoundingCalculator(new WeightedAverageCalculator(), digits: 2));
```
---

## 5. Паттерны GoF

| Группа | Паттерн | Где | Какую боль закрывает |
|---|---|---|---|
| Порождающий | **Factory** | `Domain/Factories/EvaluationFactory` | Единая точка создания оценки с инвариантами |
| Структурный | **Decorator** | `Domain/Services/RoundingCalculator` | Округление поверх любого расчёта итога |
| Поведенческий | **Strategy** | `WeightedAverageCalculator`, `WeightedRatedOnlyCalculator` | Сменяемое правило расчёта итога |
| Поведенческий | **Template Method** | `Infrastructure/Export/ResultExporter` | Общий каркас выгрузки, разный формат |

**Factory.** Оценку нельзя собрать из одного балла: нужно знать рубрикатор (есть ли критерий) и максимум критерия (диапазон). Если разрешить `new Evaluation(...)` везде, правило "12 из 10 нельзя" придётся дублировать. Конструктор `Evaluation` сделан `internal`, обычный путь создания - только фабрика. Инфраструктура собирает уже сохранённые оценки напрямую, потому что это сохранённые факты, а не новые оценки.

**Decorator.** Округление - отдельная ответственность. `RoundingCalculator` реализует тот же интерфейс `IFinalScoreCalculator` и оборачивает другой калькулятор, поэтому формулу можно менять, не трогая округление, и наоборот. Тем же способом можно добавить ограничение сверху или бонус.

**Strategy.** Правило «как считать итог» - самое вероятно меняющееся в этом домене. Реализованы две стратегии: _все критерии_ (непоставленные считаются нулём; по умолчанию) и _только оценённые_ (удобно в середине семестра). Переключение происходит в `Program.cs`.

**Template Method.** Выгрузка всегда проходит три шага: подготовить (отсортировать) -> сформировать текст -> записать в файл. `ResultExporter.Export` фиксирует порядок и общие шаги; `CsvResultExporter` и `JsonResultExporter` переопределяют только формирование текста. Добавить формат - новый подкласс без изменения остального кода.

**Что не использовали и почему.** Builder и Facade не добавлены: рубрикатор собирается простым `AddCriterion`, а UI работает с четырьмя небольшими сервисами, поэтому использование этих паттернов было бы избыточным.

---
## 6. Тесты

Фреймворк - **xUnit** (`dotnet test`). Тестируются инварианты, Domain Service и сценарии прикладного слоя:

| Файл | Что проверяет |
|---|---|
| `RubricTests` | уникальность названий, положительные максимум и вес, общий вес |
| `EvaluationFactoryTests` | диапазон балла, критерий из рубрикатора |
| `StudentGradesTests` | исправление и отмена оценки |
| `FinalScoreCalculatorTests` | обе стратегии, оценок нет, округление (Decorator) |
| `DomainDeletionTests` | удаление критерия из рубрикатора, `CancelIfPresent` |
| `GradingServiceTests`, `ReportServiceTests` | сценарии выставления оценок и расчёта итога |
| `StudentServiceTests`, `RubricServiceTests` | удаление студента и критерия вместе с оценками |

Тесты прикладного слоя используют **in-memory реализации портов** (`InMemoryRepositories.cs`), поэтому не требуют файлов. В этом проявляется польза портов.

---

## 7. Роль ИИ в решении
В работе использовался ИИ-помощник (Claude, Anthropic) как инструмент.
**Для чего использовался:**
- разобрать семинарский проект (AutoMarket) - был использован как референс для этой домашки;
- сгенерировать заготовки кода: JSON-/CSV-Конвертеры, консольный UI, тесты;
- составить **черновик** этого README: схемы, таблицы в '.md' формате.
