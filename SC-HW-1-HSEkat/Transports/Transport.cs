using SC_HW_1_HSEkat.Parameters;

namespace SC_HW_1_HSEkat.Transports
{
    public abstract class Transport : InventoryItemBase
    {
        public int EaseOfUse { get; }
        public int StatePercent { get; }
        protected Transport(int inventoryNumber, string name, int easeOfUse, int statePercent)
            : base(inventoryNumber, name)
        {
            if (easeOfUse < 1 || easeOfUse > 10) throw new ArgumentOutOfRangeException(nameof(easeOfUse), "Ease for beginner is in range from 1 to 10");
            if (statePercent < 0 || statePercent > 100) throw new ArgumentOutOfRangeException(nameof(statePercent), "State is in range from 0% to 100%");
            EaseOfUse = easeOfUse;
            StatePercent = statePercent;
        }
    }
}
