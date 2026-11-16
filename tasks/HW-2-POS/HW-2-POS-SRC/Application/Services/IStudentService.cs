using HW_2_POS.Domain.Models;

namespace HW_2_POS.Application.Services
{
    public interface IStudentService
    {
        IReadOnlyList<Student> List();
        Student? GetById(Guid id);
        Student Add(string fullName);
        void Delete(Guid id);
    }
}
