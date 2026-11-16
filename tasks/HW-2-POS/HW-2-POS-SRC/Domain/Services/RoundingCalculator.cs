using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Services
{
    public sealed class RoundingCalculator : IFinalScoreCalculator
    {
        private readonly IFinalScoreCalculator _calculator;
        private readonly int _digits;
        public RoundingCalculator(IFinalScoreCalculator calculator, int digits = 2)
        {
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            if(digits < 0 ) 
                throw new ArgumentOutOfRangeException(nameof(digits), "Digits cannot be negative");
            _digits = digits;
        }
        public FinalScore Calculate(Rubric rubric, StudentGrades grades)
        {
            var result = _calculator.Calculate(rubric, grades);
            return new FinalScore(Math.Round(result.Value, _digits, MidpointRounding.AwayFromZero));
        }

    }
}
