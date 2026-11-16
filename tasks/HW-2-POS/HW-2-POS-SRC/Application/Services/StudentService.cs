using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;

namespace HW_2_POS.Application.Services
{
    public sealed class StudentService(IStudentRepository studentRepository, IGradesRepository gradesRepository) : IStudentService
    {
        public IReadOnlyList<Student> List()
        {
            return studentRepository.List();
        }
        public Student? GetById(Guid id)
        {
            return studentRepository.GetById(id);
        }
        public Student Add(string fullName)
        {
            var student = new Student(Guid.NewGuid(), fullName);
            studentRepository.Save(student);
            return student;
        }
        public void Delete(Guid id)
        {
            if (studentRepository.GetById(id) is null)
                throw new InvalidOperationException($"Student with ID {id} not found");
            gradesRepository.Delete(id);
            studentRepository.Delete(id);
        }
    }
}
