using HW_2_POS.Domain.Factories;
using HW_2_POS.Domain.Models;

namespace HW_2_POS.Tests;

public class EvaluationFactoryTests
{
    [Fact]
    public void Create_ValidScore_ReturnsEvaluation()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 10, 1);

        var evaluation = EvaluationFactory.Create(rubric, criterion.Id, 7.5m, "ok");

        Assert.Equal(criterion.Id, evaluation.CriterionId);
        Assert.Equal(7.5m, evaluation.Score.Value);
    }

    [Fact]
    public void Create_ScoreAboveMax_Throws()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => EvaluationFactory.Create(rubric, criterion.Id, 12));
    }

    [Fact]
    public void Create_NegativeScore_Throws()
    {
        var rubric = new Rubric();
        var criterion = rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => EvaluationFactory.Create(rubric, criterion.Id, -1));
    }

    [Fact]
    public void Create_CriterionNotInRubric_Throws()
    {
        var rubric = new Rubric();
        rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<InvalidOperationException>(() => EvaluationFactory.Create(rubric, Guid.NewGuid(), 5));
    }
}
