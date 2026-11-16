using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;

namespace HW_2_POS.Tests;

internal sealed class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<Guid, Student> _items = [];

    public IReadOnlyList<Student> List() => _items.Values.ToList();
    public Student? GetById(Guid id) => _items.TryGetValue(id, out var s) ? s : null;
    public void Save(Student student) => _items[student.Id] = student;
    public void Delete(Guid id) => _items.Remove(id);
}

internal sealed class InMemoryRubricRepository : IRubricRepository
{
    private Rubric _rubric = new();

    public Rubric Get() => _rubric;
    public void Save(Rubric rubric) => _rubric = rubric;
}

internal sealed class InMemoryGradesRepository : IGradesRepository
{
    private readonly Dictionary<Guid, StudentGrades> _items = [];

    public IReadOnlyList<StudentGrades> List() => _items.Values.ToList();
    public StudentGrades? GetByStudent(Guid studentId) => _items.TryGetValue(studentId, out var g) ? g : null;
    public void Save(StudentGrades grades) => _items[grades.StudentId] = grades;
    public void Delete(Guid studentId) => _items.Remove(studentId);
}
