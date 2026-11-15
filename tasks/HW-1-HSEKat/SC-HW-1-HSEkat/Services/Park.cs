using SC_HW_1_HSEkat.Items;
using SC_HW_1_HSEkat.Parameters;
using SC_HW_1_HSEkat.Transports;

namespace SC_HW_1_HSEkat.Services
{
    public class Park
    {
        public const int BeginnerBorder = 6;
        public Park(IServiceCenter serviceCenter)
        {
            _serviceCenter = serviceCenter ?? throw new ArgumentNullException(nameof(serviceCenter));
        }
        private readonly IServiceCenter _serviceCenter;
        private readonly List<Transport> _transports = new();
        private readonly List<Equipment> _equipments = new();
        public IReadOnlyList<Transport> Transports => _transports;
        public IReadOnlyList<Equipment> Equipments => _equipments;
        public CheckResult AddTransport(Transport transport)
        {
            ArgumentNullException.ThrowIfNull(transport);
            var res = _serviceCenter.Check(transport);
            if (res.Accepted) _transports.Add(transport);
            return res;
        }
        public void AddEquipment(Equipment equipment)
        {
            ArgumentNullException.ThrowIfNull(equipment);
            _equipments.Add(equipment);
        }
        public IEnumerable<Transport> BeginnerFriendlyTransport() =>
            _transports.Where(t => t.EaseOfUse >= BeginnerBorder);
        public IReadOnlyList<IInventoryItem> AllInventory() =>
            _transports.Cast<IInventoryItem>().Concat(_equipments).OrderBy(t => t.InventoryNumber).ToList();

        public double TotalEnergy() => AllInventory().OfType<IEnergyConsumer>().Sum(t => t.DailyEnergy);
        public IReadOnlyDictionary<string, int> CountByKind() =>
            AllInventory().GroupBy(i => i.GetType().Name).OrderBy(j => j.Key).ToDictionary(j => j.Key, j => j.Count());
    }
}
