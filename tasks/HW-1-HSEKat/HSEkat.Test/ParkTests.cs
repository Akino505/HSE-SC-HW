using SC_HW_1_HSEkat.Items;
using SC_HW_1_HSEkat.Transports;

namespace HSEKat.Test;

public class ParkTests
{
    [Fact]
    public void TryAddTransport_ServiceCenterRejects_TransportIsNotAdded()
    {
        var Park = new SC_HW_1_HSEkat.Services.Park(new FakeServiceCenter(accept: false));

        var result = Park.AddTransport(new KickScooter(1, "S", 8, 100, 0.5));

        Assert.False(result.Accepted);
        Assert.Empty(Park.Transports);
        Assert.Empty(Park.AllInventory());
    }

    [Fact]
    public void TryAddTransport_ServiceCenterAccepts_TransportIsAdded()
    {
        var Park = new SC_HW_1_HSEkat.Services.Park(new FakeServiceCenter(accept: true));

        var result = Park.AddTransport(new KickScooter(1, "S", 8, 10, 0.5));

        Assert.True(result.Accepted);
        Assert.Single(Park.Transports);
    }

    [Fact]
    public void TryAddTransport_SendsTheSameTransportToServiceCenter()
    {
        var serviceCenter = new FakeServiceCenter(accept: true);
        var Park = new SC_HW_1_HSEkat.Services.Park(serviceCenter);
        var bike = new Bicycle(1, "B", 9, 90);

        Park.AddTransport(bike);

        Assert.Same(bike, Assert.Single(serviceCenter.Inspected));
    }

    [Fact]
    public void TotalDailyEnergyKwh_SumsOnlyEnergyConsumers()
    {
        var Park = new SC_HW_1_HSEkat.Services.Park(new FakeServiceCenter(accept: true));
        Park.AddTransport(new KickScooter(1, "Scooter", 8, 90, 0.5));
        Park.AddTransport(new ElectroBicycle(2, "E-bike", 6, 90, 1.0));
        Park.AddTransport(new Bicycle(3, "Bike", 9, 90));
        Park.AddEquipment(new Helmet(4, "Helmet"));
        Park.AddEquipment(new Flashlight(5, "Cabinet", 4.0));
        Park.AddEquipment(new DockStation(6, "Dock", 1.5));

        Assert.Equal(7.0, Park.TotalEnergy(), precision: 6);
    }

    [Theory]
    [InlineData(1, false)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    [InlineData(7, true)]
    [InlineData(10, true)]
    public void BeginnerFriendlyTransport_UsesThresholdOfSix(int easeOfUse, bool expected)
    {
        var Park = new SC_HW_1_HSEkat.Services.Park(new FakeServiceCenter(accept: true));
        Park.AddTransport(new Bicycle(1, "B", easeOfUse, 90));

        Assert.Equal(expected, Park.BeginnerFriendlyTransport().Any());
    }

    [Fact]
    public void AllInventory_ContainsTransportAndEquipmentSortedByNumber()
    {
        var Park = new SC_HW_1_HSEkat.Services.Park(new FakeServiceCenter(accept: true));
        Park.AddEquipment(new Helmet(3, "Helmet"));
        Park.AddTransport(new Bicycle(2, "Bike", 9, 90));
        Park.AddTransport(new KickScooter(1, "Scooter", 8, 90, 0.5));

        var numbers = Park.AllInventory().Select(i => i.InventoryNumber);

        Assert.Equal(new[] { 1, 2, 3 }, numbers);
    }
}
