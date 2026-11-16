using HW_2_POS.Domain.Models;

namespace HW_2_POS.Application.Services
{
    public interface IRubricService
    {
        IReadOnlyList<Criterion> List();
        Criterion AddCriterion(string name, decimal maxScore, decimal weight);
        int DeleteCriterion(Guid criterionId);
    }
}
