using HW_2_POS.Application.Ports;
using HW_2_POS.Application.Services;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Domain.Services;

namespace HW_2_POS.Tests;

internal sealed class TestApp
{
    public StudentService Students { get; }
    public RubricService Rubric { get; }
    public GradingService Grading { get; }
    public ReportService Reports { get; }

    /// <summary>Exposed so tests can check what is really stored.</summary>
    public IGradesRepository GradesRepository { get; }

    public TestApp()
    {
        var studentRepository = new InMemoryStudentRepository();
        var rubricRepository = new InMemoryRubricRepository();
        var gradesRepository = new InMemoryGradesRepository();
        GradesRepository = gradesRepository;

        Students = new StudentService(studentRepository, gradesRepository);
        Rubric = new RubricService(rubricRepository, gradesRepository);
        Grading = new GradingService(studentRepository, rubricRepository, gradesRepository);
        Reports = new ReportService(
            studentRepository,
            rubricRepository,
            gradesRepository,
            new WeightedAverageCalculator(),
            Array.Empty<IResultExporter>());
    }
}
