using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SC_HW_1_HSEkat;
using SC_HW_1_HSEkat.Services;
using SC_HW_1_HSEkat.Transports;

namespace HSEKat.Test;

public class DependencyInjectionTests
{
    [Theory]
    [InlineData(false, 100)]
    [InlineData(true, 0)]
    public void Container_WithFakeServiceCenter_FleetFollowsFakeDecision(bool fakeAccepts, int condition)
    {
        var services = new ServiceCollection().AddHSEKat();
        services.Replace(ServiceDescriptor.Singleton<IServiceCenter>(new FakeServiceCenter(fakeAccepts)));
        using var provider = services.BuildServiceProvider();
        var park = provider.GetRequiredService<Park>();

        var result = park.AddTransport(new Bicycle(1, "B", 9, condition));

        Assert.Equal(fakeAccepts, result.Accepted);
        Assert.Equal(fakeAccepts ? 1 : 0, park.Transports.Count);
    }
}
