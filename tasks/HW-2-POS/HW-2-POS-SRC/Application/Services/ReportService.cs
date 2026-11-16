using HW_2_POS.Application.Models;
using HW_2_POS.Application.Ports;
using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace HW_2_POS.Application.Services
{
    public class ReportService(IStudentRepository studentRepository, IRubricRepository rubricRepository, 
        IGradesRepository gradesRepository, IFinalScoreCalculator calculator, 
        IEnumerable<IResultExporter> exporters) : IReportService
    {
        public IReadOnlyList<string> ExportFormats => exporters.Select(e => e.Format).ToList();
        public FinalScore GetFinal(Guid studentId)
        {
            var student = studentRepository.GetById(studentId) ?? throw new InvalidOperationException($"Student with ID {studentId} not found");
            return Calculate(rubricRepository.Get(), student.Id);
        }
        public IReadOnlyList<StudentResult> GetAllResults()
        {
            var rubric = rubricRepository.Get();
            return studentRepository.List()
                .Select(s => new StudentResult(s.Id, s.Name, Calculate(rubric, s.Id)))
                .OrderBy(r => r.StudentName)
                .ToList();
        }
        public void Export(string format, string path)
        {
            var exporter = exporters.FirstOrDefault(e => string.Equals(e.Format, format, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                $"Unknown export format '{format}'. Available: {string.Join(", ", ExportFormats)}");
            exporter.Export(GetAllResults(), path);
        }
        private FinalScore Calculate(Rubric rubric, Guid studentId)
        {
            var sheet = gradesRepository.GetByStudent(studentId) ?? new StudentGrades(studentId);
            return calculator.Calculate(rubric, sheet);
        }
    }
}
