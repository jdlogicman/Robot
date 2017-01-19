using System;
using System.Linq;
using ControlLogic;
using Simulation;
using System.Collections.Generic;
using System.Threading;

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
            var controlArgs = new ControlParameters
            {
                PressureP = -0.3f, PressureD = -0.3f,
                // VelocityP = -9.3f, VelocityD = -7.9f
                VelocityP = -4f,
                VelocityD = -4f
            };
            RunSimulation(controlArgs, true);
#endif
        }

        static int RunSimulation(ControlParameters p, bool printResults = false)
        {
            var bs = new BouyancySystem();
            var clock = new Clock(300);
            var pressureSensor = new SimPressureSensor(clock, bs);
            var loop = new PressureControlLoop(clock, bs, pressureSensor, p, TimeSpan.FromMilliseconds(3000));

            var values = new Dictionary<string, float>();

            loop.Enable(TARGET_PRESSURE);
            
            int cycles = 0;
            int stableCycles = 0;

            
            while (cycles++ < 100 && stableCycles < 5 )
            {
                values["bm"] = bs.WaterMass;
                values["velocity"] = pressureSensor.Velocity;
                var pressureNow = values["pressure"] = pressureSensor.Get();
                values["control"] = loop.LastCorrection;
                if (printResults)
                    Console.WriteLine(cycles.ToString() + " " + string.Join("\t", from kvp in values select string.Format("{0}:{1:F3}", kvp.Key, kvp.Value)));
                
                    
                if (Math.Abs(pressureNow - TARGET_PRESSURE) <= 0.05)
                    stableCycles++;
                else
                    stableCycles = 0;
                Thread.Sleep(1000);
            }
            if (printResults) Console.WriteLine("Total cycles: {0}", cycles);
            return cycles;

        }
    }
}
