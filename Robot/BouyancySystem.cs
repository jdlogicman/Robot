using ControlLogic;
using System;

namespace Robot
{
    public class BouyancySystem : IPump
    {
        public const float MAX_LOAD_KG = 10;
        public const float KG_PUMPED_PER_MS = 0.05f;

        volatile float _waterMass;

        public float WaterMass => _waterMass;

        
        void Pump(TimeSpan duration, int direction)
        {
            _waterMass = (float)Math.Min(Math.Max(0, _waterMass + direction * Math.Abs(duration.TotalMilliseconds) * KG_PUMPED_PER_MS), MAX_LOAD_KG);
        }

        public void PumpIn(TimeSpan duration)
        {
            Pump(duration, 1);
        }

        public void PumpOut(TimeSpan duration)
        {
            Pump(duration, -1);
        }

        public void Stop()
        {
            // noop in simulation
        }
    }
}
