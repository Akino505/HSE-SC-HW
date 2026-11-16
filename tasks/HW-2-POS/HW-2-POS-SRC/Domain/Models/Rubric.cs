namespace HW_2_POS.Domain.Models
{
    public sealed class Rubric
    {
        private readonly List<Criterion> _criteria = [];
        public IReadOnlyList<Criterion> Criteria => _criteria;
        public decimal TotalWeight => _criteria.Sum(c => c.Weight.Value);
        public Criterion? GetCriterion(Guid id) => _criteria.FirstOrDefault(c => c.Id == id);
        public Criterion AddCriterion(string name, decimal maxScore, decimal weight)
        {
            var criterion = new Criterion(Guid.NewGuid(), name, new MaxScore(maxScore), new Weight(weight));
            Add(criterion);
            return criterion;
        }
        public void RemoveCriterion(Guid id)
        {
            var criterion = GetCriterion(id) ?? throw new InvalidOperationException("Criterion is not in the rubric");
            _criteria.Remove(criterion);
        }
        public static Rubric Restore(IEnumerable<Criterion> criteria)
        {
            var rubric = new Rubric();
            foreach (var criterion in criteria)
                rubric.Add(criterion);
            return rubric;
        }
        public void Add(Criterion criterion)
        {
            if (_criteria.Any(c => c.Id == criterion.Id))
                throw new InvalidOperationException("Criterion already exists in the rubric");
            if (_criteria.Any(c => string.Equals(c.Name, criterion.Name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"Criterion '{criterion.Name}' already exists in the rubric");
            _criteria.Add(criterion);
        }
    }
}
