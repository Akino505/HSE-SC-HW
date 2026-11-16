namespace HW_2_POS.Domain.Models
{
    public sealed class StudentGrades
    {
        private readonly Dictionary<Guid, Evaluation> _evaluations = [];
        public Guid StudentId { get; }
        public IReadOnlyList<Evaluation> Evaluations => _evaluations.Values.ToList();
        public StudentGrades(Guid studentId)
        {
            if(studentId == Guid.Empty)
                throw new ArgumentException("Student ID cannot be empty", nameof(studentId));
            StudentId = studentId;
        }
        public void Put(Evaluation evaluation)
        {
            ArgumentNullException.ThrowIfNull(evaluation, nameof(evaluation));
            _evaluations[evaluation.CriterionId] = evaluation;
        }
        public Evaluation? Find(Guid criterionId)
        {
            return _evaluations.TryGetValue(criterionId, out var evaluation) ? evaluation : null;
        }
        public void Cancel(Guid criterionId)
        {
            if (!_evaluations.Remove(criterionId))
                throw new InvalidOperationException("There is no grade for this criterion");
        }
        public bool CancelIfPresent(Guid criterionId)
        {
            return _evaluations.Remove(criterionId);
        }
        public static StudentGrades Restore(Guid studentId, IEnumerable<Evaluation> evaluations)
        {
            var grades = new StudentGrades(studentId);
            foreach(var evaluation in evaluations)
                grades._evaluations[evaluation.CriterionId] = evaluation;
            return grades;
        }
    }
}
