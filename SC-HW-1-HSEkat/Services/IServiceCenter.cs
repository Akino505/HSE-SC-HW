using SC_HW_1_HSEkat.Transports;
using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Services
{
    public interface IServiceCenter
    {
        CheckResult Check(Transport transport);
    }
}
