using HW_2_POS.Application.Models;
using HW_2_POS.Domain.Factories;
using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace HW_2_POS.Application.Services
{
    public class GradingService(IStudentRepository studentRepository, IRubricRepository rubricRepository, IGradesRepository gradesRepository) : IGradingService
    {
        public Evaluation Assign(Guid studentId, Guid criterionId, decimal score, string? comment = null)
        {
            EnsureStudentExists(studentId);
            var rubric = rubricRepository.Get();
            var evaluation = EvaluationFactory.Create(rubric, criterionId, score, comment);
            var sheet = gradesRepository.GetByStudent(studentId) ?? new StudentGrades(studentId);
            sheet.Put(evaluation);
            gradesRepository.Save(sheet);
            return evaluation;
        }
        public void Cancel(Guid studentId, Guid criterionId)
        {
            EnsureStudentExists(studentId);
            var sheet = gradesRepository.GetByStudent(studentId) ?? throw new InvalidOperationException("This student has no grades yet");
            sheet.Cancel(criterionId);
            gradesRepository.Save(sheet);
        }
        public IReadOnlyList<GradeLine> ListForStudent(Guid studentId)
        {
            EnsureStudentExists(studentId);
            var sheet = gradesRepository.GetByStudent(studentId);
            if(sheet == null)
                return Array.Empty<GradeLine>();
            var rubric = rubricRepository.Get();
            var lines = new List<GradeLine>();
            foreach (var evaluation in sheet.Evaluations)
            {
                var criterion = rubric.GetCriterion(evaluation.CriterionId);
                if (criterion == null)
                    continue;
                lines.Add(new GradeLine(
                    criterion.Id,
                    criterion.Name,
                    evaluation.Score.Value,
                    criterion.MaxScore.Value,
                    evaluation.Comment,
                    evaluation.Date
                    ));
            }
            return lines;
        }

        private void EnsureStudentExists(Guid studentId)
        {
            if (studentRepository.GetById(studentId) == null)
            {
                throw new InvalidOperationException($"Student with ID {studentId} not found");
            }
        }
    }
}
