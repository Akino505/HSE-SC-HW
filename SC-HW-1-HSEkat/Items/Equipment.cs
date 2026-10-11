using SC_HW_1_HSEkat.Parameters;

namespace SC_HW_1_HSEkat.Items
{
    public abstract class Equipment : InventoryItemBase
    {
        protected Equipment(int inventoryNumber, string name) : base(inventoryNumber, name) { }
    }
}
