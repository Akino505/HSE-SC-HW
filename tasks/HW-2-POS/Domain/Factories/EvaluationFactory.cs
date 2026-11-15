using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Factories
{
    public static class EvaluationFactory
    {
        public static Evaluation Create(Rubric rubric, Guid criterionId, decimal score, string? comment = null)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(rubric));
            var criterion = rubric.GetCriterion(criterionId) ?? throw new InvalidOperationException("Criterion is not in the rubric");
            var value = new Score(score);
            if(!criterion.MaxScore.Allows(value))
                throw new ArgumentOutOfRangeException(nameof(score), $"Score {score} is above the maximum {criterion.MaxScore} for '{criterion.Name}'");
            return new Evaluation(criterionId, value, DateTimeOffset.UtcNow, comment);
        }
    }
}
