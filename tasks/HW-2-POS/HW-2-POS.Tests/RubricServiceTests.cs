namespace HW_2_POS.Tests;

public class RubricServiceTests
{
    [Fact]
    public void DeleteCriterion_RemovesCriterionAndItsGradesFromAllStudents()
    {
        var app = new TestApp();
        var anna = app.Students.Add("Anna");
        var boris = app.Students.Add("Boris");
        var lab = app.Rubric.AddCriterion("Lab", 10, 1);
        var exam = app.Rubric.AddCriterion("Exam", 20, 1);
        app.Grading.Assign(anna.Id, lab.Id, 5);
        app.Grading.Assign(anna.Id, exam.Id, 10);
        app.Grading.Assign(boris.Id, lab.Id, 7);

        var removed = app.Rubric.DeleteCriterion(lab.Id);

        Assert.Equal(2, removed);
        Assert.Single(app.Rubric.List());
        Assert.Single(app.Grading.ListForStudent(anna.Id));
        Assert.Empty(app.Grading.ListForStudent(boris.Id));
    }

    [Fact]
    public void DeleteCriterion_Unknown_ThrowsAndKeepsGrades()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var lab = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Grading.Assign(student.Id, lab.Id, 5);

        Assert.Throws<InvalidOperationException>(() => app.Rubric.DeleteCriterion(Guid.NewGuid()));

        Assert.Single(app.Grading.ListForStudent(student.Id));
    }

    [Fact]
    public void DeleteCriterion_ThenGradingIt_IsRejected()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var lab = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Rubric.DeleteCriterion(lab.Id);

        Assert.Throws<InvalidOperationException>(() => app.Grading.Assign(student.Id, lab.Id, 5));
    }
}
