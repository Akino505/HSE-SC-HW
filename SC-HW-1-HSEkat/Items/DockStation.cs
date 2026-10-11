using SC_HW_1_HSEkat.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Items
{
    public class DockStation : ElectricEquipment
    {
        public DockStation(int inventoryNumber, string name, double dailyEnergy) : base(inventoryNumber, name, dailyEnergy)
        {
        }
    }
}
