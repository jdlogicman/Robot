using System;
using System.Linq;
using ControlLogic;
using Simulation;
using System.Collections.Generic;

namespace Robot
{
    class Program
    {
        const float TARGET_PRESSURE = 2; // bar, relative to surface
        const float MAX_ABS_VELOCITY = 0.2f; // bar/s
        const float METERS_PER_BAR = 10f;
        const uint DELTA_T_MS = 3000;



        static void Main(string[] args)
        {
#if false
            float pressureStart = -0.3f;
            float pressureEnd = -5f;
            float velocityStart = -0.3f;
            float velocityEnd = -10f;
            float step = 0.1f;


            var results = new SortedDictionary<int, List<ControlParameters>>();
            for (var pp = pressureStart; pp >= pressureEnd; pp -= step)
                for (var pd = pressureStart; pd >= pressureEnd; pd -= step)
                    for (var vp = velocityStart; vp >= velocityEnd; vp -= step)
                        for (var vd = velocityStart; vd >= velocityEnd; vd -= step)
                        {
                            var p = new ControlParameters
                            {
                                PressureP = pp,
                                PressureD = pd,
                                VelocityP = vp,
                                VelocityD = vd

                            };
                            var result = RunSimulation(p);
                            List<ControlParameters> others;
                            if (!results.TryGetValue(result, out others))
                                results[result] = others = new List<ControlParameters> { p };
                            else
                                others.Add(p);
                            if (results.Count > 10)
                                foreach (var key in results.Keys.Skip(10).ToArray())
                                    results.Remove(key);
                        }
            using (var file = System.IO.File.CreateText(@"c:\users\jduddy\params.txt"))
            {
                foreach (var kvp in results)
                {
                    file.WriteLine(kvp.Key.ToString());
                    foreach (var v in kvp.Value)
                        file.WriteLine($"PressureP={v.PressureP},PressureD={v.PressureD},VelocityP={v.VelocityP},VelocityD={v.VelocityD}");
                }
            }
#else
            var controlArgs = new ControlParameters { PressureP = -0.3f, PressureD = -0.3f, VelocityP = -9.3f, VelocityD = -7.9f };
            RunSimulation(controlArgs, true);
#endif
        }

        static int RunSimulation(ControlParameters p, bool printResults = false)
        {
            float pressureNow = 0;
            float velocityNowMetersPerSecond = 0;
            float correction = 0;
            BouyancySystem bs = new BouyancySystem();
            var values = new Dictionary<string, float>();
            
            var velocitySimulator = new LambdaHasValue(() =>
                {
                    var newVelocity = VelocitySimulation.CalculateNewVelocity(velocityNowMetersPerSecond,
                        deltaTInMilliSeconds: DELTA_T_MS,
                        mass: VelocitySimulation.DEFAULT_MASS_KG + bs.WaterMass);
                    if (pressureNow == 0.0 && newVelocity < 0.0)
                        newVelocity = 0;
                    velocityNowMetersPerSecond = newVelocity;
                    values["velocity"] = velocityNowMetersPerSecond;
                    return newVelocity; 
                });
            
            var pressureSimulator = new LambdaHasValue(() =>
                {
                    pressureNow = PressureSimulation.CalculatePressure(velocitySimulator.Get(), pressureNow, deltaTInMilliSeconds:DELTA_T_MS);
                    values["pressure"] = pressureNow;
                    return pressureNow;
                });
            
            var pressureErrorCalculator = new ValueRecorder((v) => values["pe"] = v, 
                new LambdaHasValue(() => pressureSimulator.Get() - TARGET_PRESSURE));
            
            var pressureController = new ValueRecorder((v) => values["pc"] = v, 
                new Pid(pressureErrorCalculator, p.PressureP, p.PressureI, p.PressureD));

            var velocityErrorCalculator = new ValueRecorder((v) => values["ve"] = v,
                new Clamper(new LambdaHasValue(() =>
                {
                    var velocityBarPerSecond = velocityNowMetersPerSecond / METERS_PER_BAR;
                    return velocityBarPerSecond - pressureController.Get();
                    
                }), -MAX_ABS_VELOCITY, MAX_ABS_VELOCITY));

            var velocityController = new ValueRecorder((v) => values["vc"] = v, 
                new Pid(velocityErrorCalculator, p.VelocityP, p.VelocityI, p.VelocityD));


            int cycles = 0;
            int stableCycles = 0;

            
            while (cycles++ < 100 && stableCycles < 5 )
            {
                correction = velocityController.Get();
                bs.OnNext(correction);
                values["bm"] = bs.WaterMass;
                if (printResults)
                    Console.WriteLine(cycles.ToString() + " " + string.Join("\t", from kvp in values select string.Format("{0}:{1:F3}", kvp.Key, kvp.Value)));
                
                    
                bs.OnNext(correction);
                if (Math.Abs(pressureNow - TARGET_PRESSURE) <= 0.05)
                    stableCycles++;
                else
                    stableCycles = 0;
            }
            if (printResults) Console.WriteLine("Total cycles: {0}", cycles);
            return cycles;

        }
    }
}
