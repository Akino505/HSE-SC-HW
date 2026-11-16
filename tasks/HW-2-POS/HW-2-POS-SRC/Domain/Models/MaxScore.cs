namespace HW_2_POS.Domain.Models
{
    public sealed record MaxScore
    {
        public decimal Value { get; }
        public MaxScore(decimal value)
        {  
            if(value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }
    public bool Allows(Score score) => score.Value <= Value;
    public override string ToString() => Value.ToString();
    }
}
