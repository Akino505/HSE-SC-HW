namespace HW_2_POS.Tests;

public class GradingServiceTests
{
    [Fact]
    public void Assign_UnknownStudent_Throws()
    {
        var app = new TestApp();
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<InvalidOperationException>(() => app.Grading.Assign(Guid.NewGuid(), criterion.Id, 5));
    }

    [Fact]
    public void Assign_ScoreAboveMax_ThrowsAndSavesNothing()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<ArgumentOutOfRangeException>(() => app.Grading.Assign(student.Id, criterion.Id, 12));
        Assert.Empty(app.Grading.ListForStudent(student.Id));
    }

    [Fact]
    public void Assign_UnknownCriterion_Throws()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        app.Rubric.AddCriterion("Lab", 10, 1);

        Assert.Throws<InvalidOperationException>(() => app.Grading.Assign(student.Id, Guid.NewGuid(), 5));
    }

    [Fact]
    public void Assign_TwiceForSameCriterion_ReplacesGrade()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);

        app.Grading.Assign(student.Id, criterion.Id, 5);
        app.Grading.Assign(student.Id, criterion.Id, 9);

        var lines = app.Grading.ListForStudent(student.Id);
        Assert.Single(lines);
        Assert.Equal(9m, lines[0].Score);
    }

    [Fact]
    public void Cancel_RemovesGrade()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Grading.Assign(student.Id, criterion.Id, 5);

        app.Grading.Cancel(student.Id, criterion.Id);

        Assert.Empty(app.Grading.ListForStudent(student.Id));
    }
}
