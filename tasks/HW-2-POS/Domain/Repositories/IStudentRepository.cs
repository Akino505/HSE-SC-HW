using HW_2_POS.Domain.Models;
namespace HW_2_POS.Domain.Repositories
{
    public interface IStudentRepository
    {
        IReadOnlyList<Student> List();
        Student? GetById(Guid id);
        void Save(Student student);
    }
}
