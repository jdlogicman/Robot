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
            // while (actual pressure != target pressure)
            //  calculate velocity
            //  calculate drag
            //  calculate actual pressure
            //  calculate pressure error
            //  calculate correction
            //  calculate mass
            const double TARGET_PRESSURE = 1; // bar, relative to surface
            const double AREA = 0.129717; // m^2 value for 8 inch circle
            const double DRAG_COEFF = 1.05; // sea water
            const double FLUID_DENSITY = 1030; // sea water kg/m^3
            const double GRAVITY = 9.80665; // m/s^2
            const double DELTA_T = 1; // second
            const double CORRECTION_SCALE = 0.01; // kg
            double relativeMass = 2; // kilo
            double velocity = 2; // m/s, positive is down
            double actualPressure = 2;

            Func<double, double> dragFunc = (v) =>  0.5 * DRAG_COEFF * FLUID_DENSITY * AREA * v * v;
            Func<double, double> velocityFunc = (m) => 
                {
                    double newValue = velocity;
                    var dragAmt = dragFunc(velocity);
                    double gravityAmt = GRAVITY*relativeMass;
                    double totalForce = gravityAmt;
                    if (Math.Abs(gravityAmt) > dragAmt)
                    {
                        totalForce -= Math.Sign(totalForce) * dragAmt;
                    }
                    newValue += DELTA_T * totalForce; 
                    return newValue;
                };
            Func<double, double> actualPressureFunc = (v) => Math.Max(0, actualPressure + v * DELTA_T);
            Func<double, double> pressureErrorFunc = (p) => p - TARGET_PRESSURE;

            double correction = 0;
            var controller = new Pid(0.5, 0, 0.1);
            controller.Subscribe(new LambdaObserver(c => correction = c));

            int cycles = 0;
            
            while (cycles++ < 100)
            {
                velocity = velocityFunc(relativeMass);
                actualPressure = actualPressureFunc(velocity);
                var error = pressureErrorFunc(actualPressure);
                controller.OnNext(error);
                relativeMass += correction * CORRECTION_SCALE;
                Console.WriteLine("{0:F2}\t{1:F2}\t{2:F2}\t{3:F2}\t{4:F2}\t", error, correction, velocity, actualPressure, relativeMass);
                
            }

        }
    }
}
