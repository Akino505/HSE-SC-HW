using HW_2_POS.Domain.Factories;
using HW_2_POS.Domain.Models;

namespace HW_2_POS.Tests;

public class StudentGradesTests
{
    [Fact]
    public void Put_SameCriterionTwice_ReplacesGrade()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 10, 1);
        var grades = new StudentGrades(Guid.NewGuid());

        grades.Put(EvaluationFactory.Create(rubric, criterion.Id, 5));
        grades.Put(EvaluationFactory.Create(rubric, criterion.Id, 9));

        Assert.Single(grades.Evaluations);
        Assert.Equal(9m, grades.Find(criterion.Id)!.Score.Value);
    }

    [Fact]
    public void Cancel_ExistingGrade_RemovesIt()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 10, 1);
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, criterion.Id, 5));

        grades.Cancel(criterion.Id);

        Assert.Null(grades.Find(criterion.Id));
    }

    [Fact]
    public void Cancel_NoGrade_Throws()
    {
        var grades = new StudentGrades(Guid.NewGuid());

        Assert.Throws<InvalidOperationException>(() => grades.Cancel(Guid.NewGuid()));
    }
}
