using System;
using System.Linq;
using ControlLogicMF;
using Simulation;
using System.Collections.Generic;

namespace Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            // given a target pressure, initial velocity, and mass 
            
            const float TARGET_PRESSURE = 2; // bar, relative to surface
            const float MAX_ABS_VELOCITY = 2; // m/s
            const uint DELTA_T_MS = 3000;
            float pressureNow = 0;
            float velocityNow = 0;
            float correction = 0;
            BouyancySystem bs = new BouyancySystem();

            var velocitySimulator = new LambdaHasValue(() =>
                {
                    var newVelocity = VelocitySimulation.CalculateNewVelocity(velocityNow,
                        deltaTInMilliSeconds: DELTA_T_MS,
                        mass: VelocitySimulation.DEFAULT_MASS_KG + bs.WaterMass);
                    if (pressureNow == 0.0 && newVelocity < 0.0)
                        newVelocity = 0;
                    velocityNow = newVelocity;
                    return newVelocity;
                });
            
            var pressureSimulator = new LambdaHasValue(() =>
                {
                    pressureNow = PressureSimulation.CalculatePressure(velocitySimulator.Get(), pressureNow, deltaTInMilliSeconds:DELTA_T_MS);
                    return pressureNow;
                });
            
            var pressureErrorCalculator = new LambdaHasValue(() => pressureSimulator.Get() - TARGET_PRESSURE);
            
            var pressureController = new Pid(pressureErrorCalculator, -2, 0, 0);

            var velocityErrorCalculator = new Clamper(new LambdaHasValue(() =>
                {
                    return velocityNow - pressureController.Get();
                }), - MAX_ABS_VELOCITY, MAX_ABS_VELOCITY);

            var velocityController = new Pid(velocityErrorCalculator, -0.2f, 0, -1f);


            int cycles = 0;
            int stableCycles = 0;

            var values = new Dictionary<string, float>();
            
            while (cycles++ < 1000000 && stableCycles < 5 )
            {
                correction = velocityController.Get();
                bs.OnNext(correction);
                values["correction"] = correction;
                values["velocity"] = velocityNow;
                values["pressure"] = pressureNow;
                values["ballastMass"] = bs.WaterMass;
                Console.WriteLine(cycles.ToString() + " " + string.Join("\t", from kvp in values select string.Format("{0}:{1:F3}", kvp.Key, kvp.Value)));
                
                    
                bs.OnNext(correction);
                if (Math.Abs(pressureNow - TARGET_PRESSURE) <= 0.05)
                    stableCycles++;
                else
                    stableCycles = 0;
            }
            Console.WriteLine("Total cycles: {0}", cycles);

        }
    }
}
