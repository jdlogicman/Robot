using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class PressureSimulation
    {
        public const double BAR_PER_M = 0.1;
        public static double CalculatePressure(double velocityNow, double pressureNow, uint deltaTInMilliSeconds = 1000)
        {
            return Math.Max(0, pressureNow + velocityNow * (deltaTInMilliSeconds / 1000.0) * BAR_PER_M);

        }
    }
}
