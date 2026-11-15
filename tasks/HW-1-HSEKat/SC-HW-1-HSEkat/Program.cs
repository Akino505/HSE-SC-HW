using Microsoft.Extensions.DependencyInjection;
using SC_HW_1_HSEkat;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

var services = new ServiceCollection().AddHSEKat();
using var provider = services.BuildServiceProvider();

provider.GetRequiredService<ConsoleApp>().Run();