namespace HW_2_POS.Infrastructure.DTOs
{
    internal sealed record CriterionDTO(Guid Id, string Name, decimal MaxScore, decimal Weight);
}
