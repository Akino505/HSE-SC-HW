using HW_2_POS.Domain.Models;

namespace HW_2_POS.Application.Models
{
    public sealed record StudentResult(Guid StudentId, string StudentName, FinalScore Score);
}
