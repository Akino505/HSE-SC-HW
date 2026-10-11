namespace SC_HW_1_HSEkat.Parameters
{
    public abstract class InventoryItemBase : IInventoryItem
    {
        public int InventoryNumber { get; }
        public string Name { get; }
        protected InventoryItemBase(int inventoryNumber, string name)
        {
            if(inventoryNumber <= 0) throw new ArgumentOutOfRangeException(nameof(inventoryNumber), "Inventory number must be positive");
            if(name == null) throw new ArgumentNullException(nameof(name), "Name cannot be empty");
            InventoryNumber = inventoryNumber;
            Name = name.Trim();
        }
    }
}
