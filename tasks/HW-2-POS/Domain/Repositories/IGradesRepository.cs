using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Repositories
{
    public interface IGradesRepository
    {
        IReadOnlyList<StudentGrades> List();
        StudentGrades? GetByStudent(Guid studentId);
        void Save(StudentGrades grades);
    }
}
