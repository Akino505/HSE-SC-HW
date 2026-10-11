using SC_HW_1_HSEkat.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Equipment
{
    public abstract class Equipment : InventoryItemBase
    {
        protected Equipment(int inventoryNumber, string name) : base(inventoryNumber, name) { }
    }
}
