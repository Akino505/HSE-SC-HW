namespace HW_2_POS.Domain.Models
{
    public sealed record FinalScore
    {
        public const decimal MaxValue = 10m;
        public decimal Value { get; }
        public FinalScore(decimal value)
        {
            if(value < 0 || value > MaxValue)
                throw new ArgumentOutOfRangeException(nameof(value), $"Final score must be between 0 and {MaxValue}");
            Value = value;
        }
        public override string ToString() => Value.ToString();
    }
}
