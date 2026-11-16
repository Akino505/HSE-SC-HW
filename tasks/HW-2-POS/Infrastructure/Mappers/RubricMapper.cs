using HW_2_POS.Domain.Models;
using HW_2_POS.Infrastructure.DTOs;

namespace HW_2_POS.Infrastructure.Mappers
{
    internal static class RubricMapper
    {
        public static Rubric ToDomain(this IEnumerable<CriterionDTO> dtos)
        {
            var criteria = dtos.Select(d => new Criterion(d.Id, d.Name, new MaxScore(d.MaxScore), new Weight(d.Weight)));
            return Rubric.Restore(criteria);
        }
        public static List<CriterionDTO> ToDtos(this Rubric rubric)
        {
            return rubric.Criteria
                .Select(c => new CriterionDTO(c.Id, c.Name, c.MaxScore.Value, c.Weight.Value))
                .ToList();
        }
    }
}
