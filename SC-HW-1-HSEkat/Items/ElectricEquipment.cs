using SC_HW_1_HSEkat.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Items
{
    public abstract class ElectricEquipment : Equipment, IEnergyConsumer
    {
        public double DailyEnergy { get; }
        protected ElectricEquipment(int inventoryNumber, string name, double dailyEnergy) : base(inventoryNumber, name)
        {
            if(dailyEnergy < 0) throw new ArgumentOutOfRangeException(nameof(dailyEnergy), "Daily energy consumption parameter cannot be negative");
            DailyEnergy = dailyEnergy;
        }
    }
}
