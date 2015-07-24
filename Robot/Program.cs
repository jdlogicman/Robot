using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ControlLogic;
using Simulation;

namespace Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            // given a target pressure, initial velocity, and mass 
            
            const double TARGET_PRESSURE = 2; // bar, relative to surface
            const double MAX_ABS_VELOCITY = 2; // m/s
            const uint DELTA_T_MS = 10;
            const uint CYCLES_PER_SECOND = 1000 / DELTA_T_MS;
            double pressureNow = 0;
            double velocityNow = 0;
            double correction = 0;
            BouyancySystem bs = new BouyancySystem();

            var velocitySimulator = new LambdaObserver(v =>
                {
                    var newVelocity = VelocitySimulation.CalculateNewVelocity(v,
                        deltaTInMilliSeconds: DELTA_T_MS,
                        mass: VelocitySimulation.DEFAULT_MASS_KG + bs.WaterMass);
                    if (pressureNow == 0.0 && newVelocity < 0.0)
                        newVelocity = 0;
                    return newVelocity;
                });
            
            var pressureSimulator = new LambdaObserver(v =>
                {
                    return PressureSimulation.CalculatePressure(v, pressureNow, deltaTInMilliSeconds:DELTA_T_MS);
                });
            
            var pressureErrorCalculator = new LambdaObserver(p => p - TARGET_PRESSURE);
            
            var pressureController = new Pid(-2, 0, 0);

            var velocityErrorCalculator = new LambdaObserver(v =>
                {
                    return velocityNow - v;
                });

            var velocityController = new Pid(-5, 0, 0);


            velocitySimulator.Chain(new LambdaObserver(v => velocityNow = v))
                .Chain(new LogObserver("velocity"))
                .Chain(pressureSimulator)
                .Chain(new LambdaObserver(p => pressureNow = p))
                .Chain(new LogObserver("pressure"))
                .Chain(pressureErrorCalculator)
                .Chain(new LogObserver("pressureError"))
                .Chain(pressureController)
                .Chain(new LogObserver("pressureCorrection"))
                .Chain(velocityErrorCalculator)
                .Chain(new Clamper(-MAX_ABS_VELOCITY, MAX_ABS_VELOCITY))
                .Chain(new LogObserver("velocityError"))
                .Chain(velocityController)
                .Chain(new LogObserver("velocityCorrection"))
                .Chain(new LambdaObserver(c => correction = c));

                
            int cycles = 0;
            int stableCycles = 0;
            
            while (cycles++ < 1000000 && stableCycles < 20 )
            {
                LogObserver.Values["ballastMass"] = bs.WaterMass;
                //LogObserver.Values["displacedMass"] = VelocitySimulation.MASS_OF_DISPLACED_WATER_KG;
                if (0 == cycles % CYCLES_PER_SECOND)
                {
                    Console.WriteLine(string.Join("\t", from kvp in LogObserver.Values select string.Format("{0}:{1:F3}", kvp.Key, kvp.Value)));
                }
                    
                velocitySimulator.OnNext(velocityNow);
                bs.OnNext(correction);
                if (Math.Abs(pressureNow - TARGET_PRESSURE) <= 0.005)
                    stableCycles++;
                else
                    stableCycles = 0;
            }
            Console.WriteLine("Total Seconds: {0}", cycles / CYCLES_PER_SECOND);

        }
    }
}
