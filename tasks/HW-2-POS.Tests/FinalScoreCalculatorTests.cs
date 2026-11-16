using HW_2_POS.Domain.Factories;
using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Services;

namespace HW_2_POS.Tests;

public class FinalScoreCalculatorTests
{
    // Lab: max 10, weight 2. Exam: max 20, weight 3.
    private static (Rubric rubric, Criterion lab, Criterion exam) CreateRubric()
    {
        var rubric = new Rubric();
        var lab = rubric.AddCriterion("Lab", 10, 2);
        var exam = rubric.AddCriterion("Exam", 20, 3);
        return (rubric, lab, exam);
    }

    [Fact]
    public void WeightedAverage_AllGraded_ReturnsExpected()
    {
        var (rubric, lab, exam) = CreateRubric();
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, lab.Id, 8));   // 0.8 * 2 = 1.6
        grades.Put(EvaluationFactory.Create(rubric, exam.Id, 10)); // 0.5 * 3 = 1.5

        var result = new WeightedAverageCalculator().Calculate(rubric, grades);

        Assert.Equal(6.2m, result.Value); // (1.6 + 1.5) / 5 * 10
    }

    [Fact]
    public void WeightedAverage_MissingGradeCountsAsZero()
    {
        var (rubric, lab, _) = CreateRubric();
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, lab.Id, 10)); // 1.0 * 2 = 2

        var result = new WeightedAverageCalculator().Calculate(rubric, grades);

        Assert.Equal(4m, result.Value); // 2 / 5 * 10
    }

    [Fact]
    public void RatedOnly_IgnoresMissingGrades()
    {
        var (rubric, lab, _) = CreateRubric();
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, lab.Id, 8)); // 0.8 * 2 = 1.6

        var result = new WeightedRatedOnlyCalculator().Calculate(rubric, grades);

        Assert.Equal(8m, result.Value); // 1.6 / 2 * 10
    }

    [Fact]
    public void NoGrades_ReturnsZero()
    {
        var (rubric, _, _) = CreateRubric();
        var grades = new StudentGrades(Guid.NewGuid());

        Assert.Equal(0m, new WeightedAverageCalculator().Calculate(rubric, grades).Value);
        Assert.Equal(0m, new WeightedRatedOnlyCalculator().Calculate(rubric, grades).Value);
    }

    [Fact]
    public void RoundingDecorator_RoundsInnerResult()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 3, 1);
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, criterion.Id, 2)); // 2/3 * 10 = 6.666...

        var calculator = new RoundingCalculator(new WeightedAverageCalculator(), digits: 2);

        Assert.Equal(6.67m, calculator.Calculate(rubric, grades).Value);
    }
}
