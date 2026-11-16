using HW_2_POS.Domain.Models;

namespace HW_2_POS.Tests;

public class RubricTests
{
    [Fact]
    public void AddCriterion_DuplicateName_Throws()
    {
        var rubric = new Rubric();
        rubric.AddCriterion("Exam", 10, 1);

        Assert.Throws<InvalidOperationException>(() => rubric.AddCriterion("exam", 20, 2));
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(10, 0)]
    [InlineData(10, -1)]
    public void AddCriterion_NonPositiveMaxScoreOrWeight_Throws(decimal max, decimal weight)
    {
        var rubric = new Rubric();

        Assert.Throws<ArgumentOutOfRangeException>(() => rubric.AddCriterion("Test", max, weight));
    }

    [Fact]
    public void TotalWeight_IsSumOfWeights()
    {
        var rubric = new Rubric();
        rubric.AddCriterion("Lab", 10, 2);
        rubric.AddCriterion("Exam", 20, 3);

        Assert.Equal(5m, rubric.TotalWeight);
    }
}
