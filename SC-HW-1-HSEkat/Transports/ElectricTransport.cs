using SC_HW_1_HSEkat.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Transports
{
    public abstract class ElectricTransport : Transport, IEnergyConsumer
    {
        public double DailyEnergy { get; }
        protected ElectricTransport(int inventoryNumber, string name, int easeOfUse, int statePercent, double dailyEnergy) : base(inventoryNumber, name, easeOfUse, statePercent)
        {
            if (dailyEnergy < 0) throw new ArgumentOutOfRangeException(nameof(dailyEnergy), "Daily energy consumption parameter cannot be negative");
            DailyEnergy = dailyEnergy;
        }
    }
}
