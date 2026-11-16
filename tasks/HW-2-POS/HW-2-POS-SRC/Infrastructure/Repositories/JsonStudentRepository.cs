using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Infrastructure.Mappers;
using HW_2_POS.Infrastructure.Storage;

namespace HW_2_POS.Infrastructure.Repositories
{
    internal sealed class JsonStudentRepository(IDataStorage storage) : IStudentRepository
    {
        public IReadOnlyList<Student> List()
        {
            return storage.Load().Students.Values.Select(dto => dto.ToDomain()).ToList();
        }
        public Student? GetById(Guid id)
        {
            return storage.Load().Students.TryGetValue(id, out var dto)
                ? dto.ToDomain()
                : null;
        }
        public void Save(Student student)
        {
            var database = storage.Load();
            database.Students[student.Id] = student.ToDTO();
            storage.Save(database);
        }
        public void Delete(Guid id)
        {
            var database = storage.Load();
            if (database.Students.Remove(id))
                storage.Save(database);
        }
    }

}
