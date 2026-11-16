using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Services
{
    public sealed class WeightedRatedOnlyCalculator : IFinalScoreCalculator
    {
        public FinalScore Calculate(Rubric rubric, StudentGrades grades)
        {
            ArgumentNullException.ThrowIfNull(rubric);
            ArgumentNullException.ThrowIfNull(grades);
            decimal sum = 0;
            decimal ratedWeight = 0;
            foreach (var criterion in rubric.Criteria)
            {
                var evaluation = grades.Find(criterion.Id);
                if (evaluation is null)
                    continue;
                sum += evaluation.Score.Value / criterion.MaxScore.Value * criterion.Weight.Value;
                ratedWeight += criterion.Weight.Value;
            }
            if (ratedWeight == 0)
                return new FinalScore(0);
            return new FinalScore(sum / ratedWeight * FinalScore.MaxValue);
        }
    }
}
