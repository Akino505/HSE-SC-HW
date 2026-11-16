namespace HW_2_POS.Domain.Models
{
    public sealed class Evaluation
    {
        public Guid CriterionId { get; }
        public Score Score { get; }
        public DateTimeOffset Date { get; }
        public string? Comment { get; }
        internal Evaluation(Guid criterionId, Score score, DateTimeOffset date, string? comment)
        {
            if(criterionId == Guid.Empty)
                throw new ArgumentException("Criterion ID cannot be empty", nameof(criterionId));
            CriterionId = criterionId;
            Score = score ?? throw new ArgumentNullException(nameof(score));
            Date = date;
            Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
        }
    }
}
