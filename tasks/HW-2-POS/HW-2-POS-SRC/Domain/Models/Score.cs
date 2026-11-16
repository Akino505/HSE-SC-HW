namespace HW_2_POS.Domain.Models
{
    public sealed record Score
    {
        public decimal Value { get; }
        public Score(decimal value)
        {
            if(value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Score cannot be negastive");
            Value = value;
        }
        public override string ToString() => Value.ToString();
    }
}
