using HW_2_POS.Infrastructure.DTOs;
namespace HW_2_POS.Infrastructure.Storage
{
    internal interface IDataStorage
    {
        StorageDTO Load();
        void Save(StorageDTO storage);
    }
}
