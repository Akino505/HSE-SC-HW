using SC_HW_1_HSEkat.Services;
using SC_HW_1_HSEkat.Transports;

namespace HSEKat.Test;

public class ServiceCenterTests
{
    [Theory]
    [InlineData(100, true)]
    [InlineData(70, true)]
    [InlineData(69, false)]
    [InlineData(0, false)]
    public void Inspect_DecidesByConditionThreshold(int condition, bool expectedAccepted)
    {
        var result = new ServiceCenter().Check(new Bicycle(1, "B", 9, condition));

        Assert.Equal(expectedAccepted, result.Accepted);
        Assert.False(string.IsNullOrWhiteSpace(result.Reason));
    }
}
