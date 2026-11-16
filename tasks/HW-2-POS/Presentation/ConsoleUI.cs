using HW_2_POS.Application.Services;
using HW_2_POS.Domain.Models;
using System.Globalization;
namespace HW_2_POS.Presentation
{
    public sealed class ConsoleUI(
        IStudentService studentService,
        IRubricService rubricService,
        IGradingService gradingService,
        IReportService reportService) : IConsoleUI
    {
        private const string HelpText =
            "Commands:\n" +
            "  students                    list students\n" +
            "  add student                 add a student\n" +
            "  criteria                    show the rubric (criteria)\n" +
            "  add criterion               add a criterion to the rubric\n" +
            "  grade                       put or correct a grade\n" +
            "  grades                      show grades of a student\n" +
            "  cancel                      cancel a grade\n" +
            "  final                       final score of one student\n" +
            "  results                     final scores of all students\n" +
            "  export <csv|json> <path>    export final scores to a file\n" +
            "  demo                        run the end-to-end demo scenario\n" +
            "  help                        show this help\n" +
            "  exit                        quit";

        public void Run()
        {
            Console.WriteLine("POS - teacher evaluation of students");
            Console.WriteLine(HelpText);
            Console.WriteLine();
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line is null)
                    return;
                var input = line.Trim();
                if (input.Length == 0)
                    continue;
                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var command = parts[0].ToLowerInvariant();
                if (command == "add" && parts.Length >= 2)
                    command = "add " + parts[1].ToLowerInvariant();
                try
                {
                    switch (command)
                    {
                        case "students": ListStudents(); break;
                        case "add student": AddStudent(); break;
                        case "criteria": ListCriteria(); break;
                        case "add criterion": AddCriterion(); break;
                        case "grade": PutGrade(); break;
                        case "grades": ShowGrades(); break;
                        case "cancel": CancelGrade(); break;
                        case "final": ShowFinal(); break;
                        case "results": PrintResults(); break;
                        case "export": Export(parts); break;
                        case "demo": RunDemo(); break;
                        case "help": Console.WriteLine(HelpText); break;
                        case "exit": return;
                        default:
                            Console.WriteLine($"Unknown command: {input}");
                            Console.WriteLine("Type 'help' to see the commands.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                Console.WriteLine();
            }
        }

        // ---------------------------------------------------------------- students
        private void ListStudents()
        {
            var students = studentService.List();
            if (students.Count == 0)
            {
                Console.WriteLine("No students yet.");
                return;
            }
            Console.WriteLine("Students:");
            for (var i = 0; i < students.Count; i++)
                Console.WriteLine($"  {i + 1}. {students[i].Name}");
        }
        private void AddStudent()
        {
            var name = Ask("Full name: ");
            var student = studentService.Add(name);
            Console.WriteLine($"Student added: {student.Name}");
        }

        // ---------------------------------------------------------------- rubric
        private void ListCriteria()
        {
            var criteria = rubricService.List();
            if (criteria.Count == 0)
            {
                Console.WriteLine("The rubric is empty.");
                return;
            }
            Console.WriteLine("Rubric:");
            for (var i = 0; i < criteria.Count; i++)
            {
                var c = criteria[i];
                Console.WriteLine($"  {i + 1}. {c.Name}  (max {Fmt(c.MaxScore.Value)}, weight {Fmt(c.Weight.Value)})");
            }
        }
        private void AddCriterion()
        {
            var name = Ask("Criterion name: ");
            var maxScore = AskDecimal("Max score: ");
            var weight = AskDecimal("Weight: ");
            var criterion = rubricService.AddCriterion(name, maxScore, weight);
            Console.WriteLine(
    $"Criterion added: {criterion.Name} (max {Fmt(criterion.MaxScore.Value)}, weight {Fmt(criterion.Weight.Value)})");
        }

        // ---------------------------------------------------------------- grades
        private void PutGrade()
        {
            var studentId = PickStudent();
            var criterionId = PickCriterion();
            var score = AskDecimal("Score: ");
            var comment = Ask("Comment (optional): ");

            gradingService.Assign(studentId, criterionId, score, comment);
            Console.WriteLine("Grade saved (an existing grade for this criterion is replaced).");
        }
        private void ShowGrades()
        {
            var studentId = PickStudent();
            var lines = gradingService.ListForStudent(studentId);
            if (lines.Count == 0)
            {
                Console.WriteLine("This student has no grades yet.");
                return;
            }
            Console.WriteLine("Grades:");
            foreach (var line in lines)
            {
                var comment = string.IsNullOrEmpty(line.Comment) ? "" : $"  \"{line.Comment}\"";
                Console.WriteLine(
                    $"  {line.CriterionName,-20} {Fmt(line.Score)} / {Fmt(line.MaxScore)}  ({line.Date:yyyy-MM-dd}){comment}");
            }
        }
        private void CancelGrade()
        {
            var studentId = PickStudent();
            var criterionId = PickCriterion();
            gradingService.Cancel(studentId, criterionId);
            Console.WriteLine("Grade cancelled.");
        }

        // ---------------------------------------------------------------- results
        private void ShowFinal()
        {
            var studentId = PickStudent();
            var final = reportService.GetFinal(studentId);
            Console.WriteLine($"Final score: {Fmt(final.Value)} / {Fmt(FinalScore.MaxValue)}");
        }
        private void PrintResults()
        {
            var results = reportService.GetAllResults();
            if (results.Count == 0)
            {
                Console.WriteLine("No students yet.");
                return;
            }
            Console.WriteLine("Final scores:");
            foreach (var result in results)
                Console.WriteLine($"  {result.StudentName,-25} {Fmt(result.Score.Value)} / {Fmt(FinalScore.MaxValue)}");
        }

        private void Export(string[] parts)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine($"Usage: export <{string.Join("|", reportService.ExportFormats)}> <path>");
                return;
            }
            reportService.Export(parts[1], parts[2]);
            Console.WriteLine($"Results exported to {parts[2]}");
        }

        // ---------------------------------------------------------------- demo
        private void RunDemo()
        {
            if (studentService.List().Count > 0 || rubricService.List().Count > 0)
                throw new InvalidOperationException("The demo needs an empty database. Delete pos-data.json and restart.");
            Console.WriteLine("1) Adding students and criteria...");
            var anna = studentService.Add("Anna Ivanova");
            var boris = studentService.Add("Boris Petrov");
            studentService.Add("Clara Smirnova");
            var lab = rubricService.AddCriterion("Lab work", 10, 2);
            var homework = rubricService.AddCriterion("Homework", 10, 3);
            var exam = rubricService.AddCriterion("Exam", 20, 5);
            ListStudents();
            ListCriteria();
            Console.WriteLine();
            Console.WriteLine("2) Assigning grades...");
            gradingService.Assign(anna.Id, lab.Id, 9, "Great");
            gradingService.Assign(anna.Id, homework.Id, 8);
            gradingService.Assign(anna.Id, exam.Id, 17);
            gradingService.Assign(boris.Id, lab.Id, 6);
            gradingService.Assign(boris.Id, exam.Id, 12);
            Console.WriteLine("   done");
            Console.WriteLine();
            Console.WriteLine("3) Trying an invalid grade: 12 out of 10...");
            try
            {
                gradingService.Assign(boris.Id, lab.Id, 12);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"   Rejected: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine("4) Correcting a grade: Boris, Lab work 6 -> 8...");
            gradingService.Assign(boris.Id, lab.Id, 8, "Resubmitted");
            Console.WriteLine();
            Console.WriteLine("5) Final scores:");
            PrintResults();
            Console.WriteLine();
            Console.WriteLine("6) Exporting...");
            reportService.Export("csv", "demo-results.csv");
            reportService.Export("json", "demo-results.json");
            Console.WriteLine("   demo-results.csv and demo-results.json created");
        }

        // ---------------------------------------------------------------- input helpers
        private Guid PickStudent()
        {
            var students = studentService.List();
            if (students.Count == 0)
                throw new InvalidOperationException("No students yet. Use 'add student' first.");

            for (var i = 0; i < students.Count; i++)
                Console.WriteLine($"  {i + 1}. {students[i].Name}");
            var number = AskNumber("Student number: ", students.Count);
            return students[number - 1].Id;
        }
        private Guid PickCriterion()
        {
            var criteria = rubricService.List();
            if (criteria.Count == 0)
                throw new InvalidOperationException("The rubric is empty. Use 'add criterion' first.");
            for (var i = 0; i < criteria.Count; i++)
                Console.WriteLine($"  {i + 1}. {criteria[i].Name} (max {Fmt(criteria[i].MaxScore.Value)})");
            var number = AskNumber("Criterion number: ", criteria.Count);
            return criteria[number - 1].Id;
        }
        private static string Ask(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
        private static int AskNumber(string prompt, int max)
        {
            if (!int.TryParse(Ask(prompt), NumberStyles.Integer, CultureInfo.InvariantCulture, out var number)
                || number < 1 || number > max)
                throw new FormatException($"Please enter a number from 1 to {max}");
            return number;
        }
        private static decimal AskDecimal(string prompt)
        {
            var text = Ask(prompt).Replace(',', '.'); // accept both 7,5 and 7.5
            if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                throw new FormatException("Please enter a number, for example 7.5");
            return value;
        }
        private static string Fmt(decimal value) => value.ToString("0.##", CultureInfo.InvariantCulture);
    }

}
