using SC_HW_1_HSEkat.Items;
using SC_HW_1_HSEkat.Parameters;
using SC_HW_1_HSEkat.Services;
using SC_HW_1_HSEkat.Transports;
using System.Globalization;

namespace SC_HW_1_HSEkat
{
    public class ConsoleApp
    {
        private readonly Park _park;
        private readonly NumberGenerator _numbers;
        public ConsoleApp(Park park, NumberGenerator numbers)
        {
            _numbers = numbers ?? throw new ArgumentNullException(nameof(numbers));
            _park = park ?? throw new ArgumentNullException(nameof(park));
        }
        public void Run()
        {
            Console.WriteLine("===== Вышкат project start =====");
            try
            {
                while (true)
                {
                    PrintMenu();
                    var choice = ReadLine("Choose the option: ").Trim();
                    Console.WriteLine();

                    switch (choice)
                    {
                        case "1": AddTransport(); break;
                        case "2": AddEquipment(); break;
                        case "3": ShowCounts(); break;
                        case "4": ShowEnergy(); break;
                        case "5": ShowBeginnerTransport(); break;
                        case "6": ShowInventory(); break;
                        case "0":
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Try again");
                            break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine();
                Console.WriteLine("Exit");
            }
        }
        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Accept transport (with technical inspection)");
            Console.WriteLine("2. Put an item on balance (helmet, flashlight, docking station)");
            Console.WriteLine("3. Report: number of units");
            Console.WriteLine("4. Report: total daily energy consumption");
            Console.WriteLine("5. Transport for beginners");
            Console.WriteLine("6. All equipment and items with inventory numbers");
            Console.WriteLine("0. Exit");
        }
        private void AddTransport()
        {
            Console.WriteLine("Vehicle type: 1 - KickScooter, 2 - Bicycle, 3 - Electric bicycle");
            var kind = ReadInt("Kind: ", 1, 3);
            var name = ReadNonEmpty("Name: ");
            var ease = ReadInt("Ease of use: ", 1, 10);
            var condition = ReadInt("Technical state, % (0-100): ", 0, 100);

            var number = _numbers.Next();
            Transport transport = kind switch
            {
                1 => new KickScooter(number, name, ease, condition, ReadEnergy()),
                2 => new Bicycle(number, name, ease, condition),
                _ => new ElectroBicycle(number, name, ease, condition, ReadEnergy()),
            };

            Register(transport);
        }
        private void AddEquipment()
        {
            Console.WriteLine("Item: 1 - Helmet, 2 - Flashlight, 3 - Dock Station");
            var kind = ReadInt("Kind: ", 1, 3);
            var name = ReadNonEmpty("Name: ");

            var number = _numbers.Next();
            Equipment equipment = kind switch
            {
                1 => new Helmet(number, name),
                2 => new Flashlight(number, name, ReadEnergy()),
                _ => new DockStation(number, name, ReadEnergy()),
            };

            _park.AddEquipment(equipment);
            Console.WriteLine($"Capitalized: {Describe(equipment)}");
        }

        private void ShowCounts()
        {
            Console.WriteLine("Number of units by type: ");
            var counts = _park.CountByKind();
            if (counts.Count == 0)
            {
                Console.WriteLine("  Park is empty");
                return;
            }

            foreach (var (kind, count) in counts)
                Console.WriteLine($"  {kind,-16} {count}");
            Console.WriteLine($"  {"TOTAL:",-16} {counts.Values.Sum()}");
        }

        private void ShowEnergy() =>
            Console.WriteLine($"Total daily consumption: {Format(_park.TotalEnergy())} Kw/h");

        private void ShowBeginnerTransport()
        {
            Console.WriteLine($"Vehicle for beginners (Simplicity level >= {Park.BeginnerBorder}):");
            var list = _park.BeginnerFriendlyTransport().ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("  There is no suitable equipment available.");
                return;
            }

            foreach (var t in list)
                Console.WriteLine($"  {Describe(t)}, Simplicity level {t.EaseOfUse}");
        }

        private void ShowInventory()
        {
            Console.WriteLine("Inventory (equipment and items):");
            var items = _park.AllInventory();
            if (items.Count == 0)
            {
                Console.WriteLine("  balance is empty");
                return;
            }

            foreach (var item in items)
                Console.WriteLine($"  {Describe(item)}");
        }
        private static string ReadLine(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? throw new OperationCanceledException();
        }
        private static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                var text = ReadLine(prompt).Trim();
                if (text.Length > 0)
                    return text;
                Console.WriteLine("The value cannot be empty");
            }
        }

        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                if (int.TryParse(ReadLine(prompt).Trim(), out var value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Enter integer from {min} to {max}");
            }
        }

        private static double ReadEnergy()
        {
            while (true)
            {
                var text = ReadLine("Daily energy consumption, Kw/h: ").Trim().Replace(',', '.');
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value) && value >= 0)
                    return value;
                Console.WriteLine("Enter non-negative number");
            }
        }
        private void Register(Transport transport)
        {
            var result = _park.AddTransport(transport);
            Console.WriteLine(result.Accepted
                ? $"  [ACCEPT]   {Describe(transport)} - {result.Reason}"
                : $"  [REJECT] {Describe(transport)} - {result.Reason}");
        }

        private static string Describe(IInventoryItem item) =>
            $"№{item.InventoryNumber} {item.GetType().Name} '{item.Name}'";

        private static string Format(double value) => value.ToString("0.00", CultureInfo.InvariantCulture);

    }
}

