using HW_2_POS.Domain.Models;
using HW_2_POS.Infrastructure.DTOs;

namespace HW_2_POS.Infrastructure.Mappers
{
    internal static class StudentMapper
    {
        public static Student ToDomain(this StudentDTO dto)
        {
            return new Student(dto.Id, dto.FullName);
        }
        public static StudentDTO ToDto(this Student student)
        {
            return new StudentDTO(student.Id, student.Name);
        }
    }
}
