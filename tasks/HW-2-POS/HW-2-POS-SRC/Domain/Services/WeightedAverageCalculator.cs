using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Services
{
    public sealed class WeightedAverageCalculator : IFinalScoreCalculator
    {
        public FinalScore Calculate(Rubric rubric, StudentGrades grades)
        {
            ArgumentNullException.ThrowIfNull(rubric);
            ArgumentNullException.ThrowIfNull(grades);
            if (rubric.TotalWeight == 0)
                return new FinalScore(0);
            decimal sum = 0;
            foreach (var criterion in rubric.Criteria)
            {
                var evaluation = grades.Find(criterion.Id);
                if (evaluation is null)
                    continue;

                sum += evaluation.Score.Value / criterion.MaxScore.Value * criterion.Weight.Value;
            }
            return new FinalScore(sum / rubric.TotalWeight * FinalScore.MaxValue);
        }

    }
}