using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class VelocitySimulation
    {
        const double DRAG_COEFF = 1.05; // sea water
        const double FLUID_DENSITY_KG_PER_M3 = 1030; // sea water kg/m^3
        public const double GRAVITY = 9.80665; // m/s^2
        public const double AREA_END_M2 = 0.129717; // m^2 value for 8 inch circle
        public const double LENGTH_M = 26 / 39.37;
        public const double VOLUME_M3 = AREA_END_M2 * LENGTH_M;
        public const double MASS_OF_DISPLACED_WATER_KG = VOLUME_M3 * FLUID_DENSITY_KG_PER_M3;
        public const double DEFAULT_MASS_KG = MASS_OF_DISPLACED_WATER_KG - 0.5; // assume buoyant

            

        public static double CalculateNewVelocity(double velocityNow, int deltaTInSeconds = 1, 
            double mass = DEFAULT_MASS_KG, double volume = VOLUME_M3, 
            double areaFacingDirectionOfTravel = AREA_END_M2)
        {
            const int ITERATIONS_PER_SECOND = 100;
            const double DELTA_T_PER_ITERATION = 1 / (double)ITERATIONS_PER_SECOND;
            
            double massOfDisplacedWater = volume * FLUID_DENSITY_KG_PER_M3;
            double gravityForce = (mass - massOfDisplacedWater) * GRAVITY;

            for (int i = 0; i < deltaTInSeconds * ITERATIONS_PER_SECOND; i++)
            {
                double dragForce = CalculateDragForce(velocityNow, areaFacingDirectionOfTravel);
                double totalForce = gravityForce + dragForce;
                double acceleration = totalForce / mass;

                velocityNow += DELTA_T_PER_ITERATION * acceleration;
            }
            return velocityNow;
        }

        public static double CalculateDragForce(double velocityNow /* m/s */, 
            double areaFacingDirectionOfTravel /* m^2 */)
        {
            //                                                       kg/m^3           m^2                           m/s           m/s
            // kg/m^3           m^2                           m^2/s^2
            // kg/m^3           m^4/s^2
            // kg           m/s^2
            return -1 * Math.Sign(velocityNow) * 0.5 * DRAG_COEFF * FLUID_DENSITY_KG_PER_M3 * areaFacingDirectionOfTravel * velocityNow * velocityNow;
        }
    }
}
