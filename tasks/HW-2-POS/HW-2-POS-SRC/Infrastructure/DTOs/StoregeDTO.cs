namespace HW_2_POS.Infrastructure.DTOs
{
    internal sealed class StorageDTO
    {
        public Dictionary<Guid, StudentDTO> Students { get; set; } = [];
        public List<CriterionDTO> Criteria { get; set; } = [];
        public Dictionary<Guid, StudentGradesDTO> Grades { get; set; } = [];
    }
}
