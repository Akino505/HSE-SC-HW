using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Infrastructure.Storage;

namespace HW_2_POS.Infrastructure.Repositories
{
    internal sealed class JsonGradesRepository(IDataStorage storage) : IGradesRepository
    {
        public IReadOnlyList<StudentGrades> List()
        {
            return storage.Load().Grades.Values.Select(dto => dto.ToDomain()).ToList();
        }
        public StudentGrades? GetByStudent(Guid studentId)
        {
            return storage.Load().Grades.TryGetValue(studentId, out var dto)
                ? dto.ToDomain()
                : null;
        }
        public void Save(StudentGrades grades)
        {
            var database = storage.Load();
            database.Grades[grades.StudentId] = grades.ToDto();
            storage.Save(database);
        }
    }
}
