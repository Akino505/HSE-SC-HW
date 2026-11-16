using HW_2_POS.Domain.Models;
using HW_2_POS.Domain.Repositories;
using HW_2_POS.Infrastructure.Storage;

namespace HW_2_POS.Infrastructure.Repositories
{
    internal sealed class JsonRubricRepository(IDataStorage storage) : IRubricRepository
    {
        public Rubric Get()
        {
            return storage.Load().Criteria.ToDomain();
        }
        public void Save(Rubric rubric)
        {
            var database = storage.Load();
            database.Criteria = rubric.ToDtos();
            storage.Save(database);
        }
    }
}
