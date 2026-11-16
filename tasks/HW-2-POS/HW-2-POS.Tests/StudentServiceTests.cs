namespace HW_2_POS.Tests;

public class StudentServiceTests
{
    [Fact]
    public void Delete_RemovesStudentAndTheirGrades()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Grading.Assign(student.Id, criterion.Id, 5);

        app.Students.Delete(student.Id);

        Assert.Null(app.Students.GetById(student.Id));
        Assert.Null(app.GradesRepository.GetByStudent(student.Id));
    }

    [Fact]
    public void Delete_DoesNotTouchOtherStudents()
    {
        var app = new TestApp();
        var anna = app.Students.Add("Anna");
        var boris = app.Students.Add("Boris");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Grading.Assign(anna.Id, criterion.Id, 5);
        app.Grading.Assign(boris.Id, criterion.Id, 7);

        app.Students.Delete(anna.Id);

        Assert.Single(app.Students.List());
        Assert.Single(app.Grading.ListForStudent(boris.Id));
    }

    [Fact]
    public void Delete_UnknownStudent_Throws()
    {
        var app = new TestApp();

        Assert.Throws<InvalidOperationException>(() => app.Students.Delete(Guid.NewGuid()));
    }
}
