using System;
using System.Collections.Generic;
using System.Text;

namespace SC_HW_1_HSEkat.Services
{
    public class NumberGenerator
    {
        private int _current;
        public int Next() => ++_current;
    }
}
