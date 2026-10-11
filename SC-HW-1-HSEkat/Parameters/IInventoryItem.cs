using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Parameters
{
    public interface IInventoryItem
    {
        int InventoryNumber { get; }
        string Name { get; }
    }
}
