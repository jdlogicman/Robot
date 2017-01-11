using System;


namespace Simulation
{
    public class VelocitySimulation
    {
        const float DRAG_COEFF = 1.05f; // sea water
        const float FLUID_DENSITY_KG_PER_M3 = 1030; // sea water kg/m^3
        public const float GRAVITY = 9.80665f; // m/s^2
        public const float AREA_END_M2 = 0.129717f; // m^2 value for 8 inch circle
        public const float LENGTH_M = 26 / 39.37f;
        public const float VOLUME_M3 = AREA_END_M2 * LENGTH_M;
        public const float MASS_OF_DISPLACED_WATER_KG = VOLUME_M3 * FLUID_DENSITY_KG_PER_M3;
        public const float DEFAULT_MASS_KG = MASS_OF_DISPLACED_WATER_KG - 0.5f; // assume buoyant

            

        public static float CalculateNewVelocity(float velocityNow, uint deltaTInMilliSeconds = 1000, 
            float mass = DEFAULT_MASS_KG, float volume = VOLUME_M3, 
            float areaFacingDirectionOfTravel = AREA_END_M2)
        {
            const float DELTA_T_PER_ITERATION = 0.001f;
            
            float massOfDisplacedWater = volume * FLUID_DENSITY_KG_PER_M3;
            float gravityForce = (mass - massOfDisplacedWater) * GRAVITY;

            for (int i = 0; i < deltaTInMilliSeconds; i++)
            {
                float dragForce = CalculateDragForce(velocityNow, areaFacingDirectionOfTravel);
                float totalForce = gravityForce + dragForce;
                float acceleration = totalForce / mass;

                velocityNow += DELTA_T_PER_ITERATION * acceleration;
            }
            return velocityNow;
        }

        public static float CalculateDragForce(float velocityNow /* m/s */, 
            float areaFacingDirectionOfTravel /* m^2 */)
        {
            return (float)(-1 * Math.Sign(velocityNow) * 0.5 * DRAG_COEFF * FLUID_DENSITY_KG_PER_M3 * areaFacingDirectionOfTravel * velocityNow * velocityNow);
        }
    }
}
