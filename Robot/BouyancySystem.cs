using System;

namespace Robot
{
    public class BouyancySystem 
    {
        public const float MAX_LOAD_KG = 10;

        public float WaterMass { get; private set; }

        
        public void OnNext(float amount)
        {
            WaterMass = Math.Min(Math.Max(0, WaterMass + amount), MAX_LOAD_KG);
        }
    }
}
