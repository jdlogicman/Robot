using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class PressureSimulation
    {
        public const float BAR_PER_M = 0.1f;
        public static float CalculatePressure(float velocityNow, float pressureNow, uint deltaTInMilliSeconds = 1000)
        {
            return (float)Math.Max(0, pressureNow + velocityNow * (deltaTInMilliSeconds / 1000.0) * BAR_PER_M);

        }
    }
}
