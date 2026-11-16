using HW_2_POS.Domain.Models;
using HW_2_POS.Infrastructure.DTOs;

namespace HW_2_POS.Infrastructure.Mappers
{
    internal static class GradesMapper
    {
        public static StudentGrades ToDomain(this StudentGradesDTO dto)
        {
            var evaluations = dto.Evaluations
                .Select(e => new Evaluation(e.CriterionId, new Score(e.Score), e.Date, e.Comment));
            return StudentGrades.Restore(dto.StudentId, evaluations);
        }
        public static StudentGradesDTO ToDTO(this StudentGrades grades)
        {
            var evaluations = grades.Evaluations
                .Select(e => new EvaluationDTO(e.CriterionId, e.Score.Value, e.Date, e.Comment))
                .ToList();
            return new StudentGradesDTO(grades.StudentId, evaluations);
        }
    }
}
