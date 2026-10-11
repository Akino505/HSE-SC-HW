using SC_HW_1_HSEkat.Transports;

namespace SC_HW_1_HSEkat.Services
{
    public interface IServiceCenter
    {
        CheckResult Check(Transport transport);
    }
}
