using SC_HW_1_HSEkat.Items;
using SC_HW_1_HSEkat.Parameters;
using SC_HW_1_HSEkat.Transports;
namespace HSEKat.Test;

public class DomainTests
{
    [Fact]
    public void HelmetAndBicycle_AreNotEnergyConsumers_ButElectricBikeIs()
    {
        Assert.False(new Helmet(1, "H") is IEnergyConsumer);
        Assert.False(new Bicycle(2, "B", 9, 90) is IEnergyConsumer);
        Assert.True(new ElectroBicycle(3, "E", 6, 90, 1.0) is IEnergyConsumer);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Transport_EaseOfUseOutOfRange_Throws(int easeOfUse)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bicycle(1, "B", easeOfUse, 90));
    }

    [Fact]
    public void KickScooter_NegativeEnergy_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new KickScooter(1, "S", 5, 90, -0.1));
    }
}
