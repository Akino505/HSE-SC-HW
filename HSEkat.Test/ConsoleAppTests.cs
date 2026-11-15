using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SC_HW_1_HSEkat;
using SC_HW_1_HSEkat.Services;


namespace HSEKat.Test;

[Collection("Console")]
public class ConsoleAppTests : IDisposable
{
    private readonly TextReader _originalIn = Console.In;
    private readonly TextWriter _originalOut = Console.Out;

    public void Dispose()
    {
        Console.SetIn(_originalIn);
        Console.SetOut(_originalOut);
    }

    private static string RunApp(string input, IServiceCenter? serviceCenter = null)
    {
        var services = new ServiceCollection().AddHSEKat();
        if (serviceCenter is not null)
            services.Replace(ServiceDescriptor.Singleton(serviceCenter));

        using var provider = services.BuildServiceProvider();
        var output = new StringWriter();
        Console.SetIn(new StringReader(input));
        Console.SetOut(output);

        provider.GetRequiredService<ConsoleApp>().Run();
        return output.ToString();
    }

    [Fact]
    public void AddTransport_WithFakeRejectingServiceCenter_IsRejected()
    {
        // 1 - добавить транспорт: велосипед, имя, простота 9, состояние 100; 0 - выход
        var output = RunApp("1\n2\nBike\n9\n100\n0\n", new FakeServiceCenter(accept: false));

        Assert.Contains("[REJECT]", output);
        Assert.Contains("fake: reject", output);
    }
}
