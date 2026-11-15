using HW_2_POS.Domain.Models;

namespace HW_2_POS.Domain.Repositories
{
    public interface IRubricRepository
    {
        Rubric Get();
        void Save(Rubric rubric);
    }
}
