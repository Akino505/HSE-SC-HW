using SC_HW_1_HSEkat.Transports;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Equipment
{
    public class Bicycle : Transport
    {
        public Bicycle(int inventoryNumber, string name, int easeOfUse, int statePercente)
            : base(inventoryNumber, name, easeOfUse, statePercente) { }
    }
}
