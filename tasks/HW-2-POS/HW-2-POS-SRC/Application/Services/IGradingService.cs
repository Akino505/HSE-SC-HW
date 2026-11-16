using HW_2_POS.Application.Models;
using HW_2_POS.Domain.Models;

namespace HW_2_POS.Application.Services
{
    public interface IGradingService
    {
        Evaluation Assign(Guid studentId, Guid criterionId, decimal score, string? comment = null);
        void Cancel(Guid studentId, Guid criterionId);
        IReadOnlyList<GradeLine> ListForStudent(Guid studentId);
    }
}
