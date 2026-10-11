using SC_HW_1_HSEkat.Transports;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Services
{
    public class ServiceCenter : IServiceCenter
    {
        public const int MinState = 70;
        public CheckResult Check(Transport transport)
        {
            ArgumentNullException.ThrowIfNull(transport);
            return transport.StatePercent >= MinState
                ? CheckResult.Accept()
                : CheckResult.Reject(
                        $"State {transport.StatePercent}% < than minimum accepatble: {MinState}%");
        }
    }
}
