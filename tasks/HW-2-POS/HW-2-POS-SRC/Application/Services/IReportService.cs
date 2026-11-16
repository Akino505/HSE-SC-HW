using HW_2_POS.Application.Models;
using HW_2_POS.Domain.Models;

namespace HW_2_POS.Application.Services
{
    public interface IReportService
    {
        FinalScore GetFinal(Guid studentId);
        IReadOnlyList<StudentResult> GetAllResults();
        IReadOnlyList<string> ExportFormats { get; }
        void Export(string format, string path);
    }
}
