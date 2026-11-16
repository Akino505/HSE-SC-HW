namespace HW_2_POS.Domain.Models
{
    public sealed class Criterion
    {
        public Guid Id { get; }
        public string Name { get; }
        public MaxScore MaxScore { get; }
        public Weight Weight { get; }
        public Criterion(Guid id, string name, MaxScore maxScore, Weight weight)
        {
            if(id == Guid.Empty)
                throw new ArgumentException("Criterion ID cannt be empty", nameof(id));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Criterion name cannot be empty", nameof(name));
            Id = id;
            Name = name;
            MaxScore = maxScore ?? throw new ArgumentNullException(nameof(maxScore));
            Weight = weight ?? throw new ArgumentNullException(nameof(weight));
        }
    }
}
