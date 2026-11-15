namespace HW_2_POS.Domain.Models
{
    public sealed record Weight
    {
        public decimal Value { get; }
        public Weight(decimal value)
        {
            if(value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            Value = value;
        }
    public override string ToString() => Value.ToString();
    }
}
