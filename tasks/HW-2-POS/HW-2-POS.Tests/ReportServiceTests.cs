namespace HW_2_POS.Tests;

public class ReportServiceTests
{
    [Fact]
    public void GetFinal_UsesGradesAndRubric()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        var criterion = app.Rubric.AddCriterion("Lab", 10, 1);
        app.Grading.Assign(student.Id, criterion.Id, 8);

        Assert.Equal(8m, app.Reports.GetFinal(student.Id).Value);
    }

    [Fact]
    public void GetFinal_StudentWithoutGrades_IsZero()
    {
        var app = new TestApp();
        var student = app.Students.Add("Anna");
        app.Rubric.AddCriterion("Lab", 10, 1);

        Assert.Equal(0m, app.Reports.GetFinal(student.Id).Value);
    }

    [Fact]
    public void GetFinal_UnknownStudent_Throws()
    {
        var app = new TestApp();

        Assert.Throws<InvalidOperationException>(() => app.Reports.GetFinal(Guid.NewGuid()));
    }

    [Fact]
    public void GetAllResults_SortedByStudentName()
    {
        var app = new TestApp();
        app.Students.Add("Boris");
        app.Students.Add("Anna");

        var results = app.Reports.GetAllResults();

        Assert.Equal("Anna", results[0].StudentName);
        Assert.Equal("Boris", results[1].StudentName);
    }

    [Fact]
    public void Export_UnknownFormat_Throws()
    {
        var app = new TestApp();

        Assert.Throws<InvalidOperationException>(() => app.Reports.Export("xml", "out.xml"));
    }
}
