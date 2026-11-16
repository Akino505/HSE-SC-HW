namespace HW_2_POS.Infrastructure.DTOs
{
    internal sealed record EvaluationDTO(
    Guid CriterionId,
    decimal Score,
    DateTimeOffset Date,
    string? Comment = null);

    internal sealed record StudentGradesDTO(
        Guid StudentId,
        List<EvaluationDTO> Evaluations);
}
