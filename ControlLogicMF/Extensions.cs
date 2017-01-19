using System;

namespace ControlLogic
{
    public static class Extensions
    {
        public static double TotalMilliseconds(this TimeSpan ts)
        {
            return ts.Ticks / 10000.0;
        }
        public static double TotalSeconds(this TimeSpan ts)
        {
            return ts.Ticks / 10000000.0;
        }
    }
}
