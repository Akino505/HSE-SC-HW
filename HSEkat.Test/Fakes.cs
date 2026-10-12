using SC_HW_1_HSEkat.Services;
using SC_HW_1_HSEkat.Transports;

public class FakeServiceCenter : IServiceCenter
{
    private readonly bool _accept;

    public List<Transport> Inspected { get; } = new();

    public FakeServiceCenter(bool accept) => _accept = accept;

    public CheckResult Check(Transport transport)
    {
        Inspected.Add(transport);
        return _accept ? CheckResult.Accept("fake: accept") : CheckResult.Reject("fake: reject");
    }
}
