using HW_2_POS.Domain.Factories;
using HW_2_POS.Domain.Models;

namespace HW_2_POS.Tests;

public class DomainDeletionTests
{
    [Fact]
    public void RemoveCriterion_Existing_RemovesIt()
    {
        var rubric = new Rubric();
        var lab = rubric.AddCriterion("Lab", 10, 2);
        rubric.AddCriterion("Exam", 20, 3);

        rubric.RemoveCriterion(lab.Id);

        Assert.Null(rubric.GetCriterion(lab.Id));
        Assert.Single(rubric.Criteria);
        Assert.Equal(3m, rubric.TotalWeight);
    }

    [Fact]
    public void RemoveCriterion_Unknown_Throws()
    {
        var rubric = new Rubric();

        Assert.Throws<InvalidOperationException>(() => rubric.RemoveCriterion(Guid.NewGuid()));
    }

    [Fact]
    public void CancelIfPresent_ReturnsTrueOnlyWhenGradeExisted()
    {
        var rubric = new Rubric();
        var lab = rubric.AddCriterion("Lab", 10, 1);
        var grades = new StudentGrades(Guid.NewGuid());
        grades.Put(EvaluationFactory.Create(rubric, lab.Id, 5));

        Assert.True(grades.CancelIfPresent(lab.Id));
        Assert.False(grades.CancelIfPresent(lab.Id));
    }
}
