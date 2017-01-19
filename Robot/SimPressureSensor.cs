using ControlLogic;
using Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public class SimPressureSensor : IHasValue
    {
        readonly BouyancySystem _bs;
        volatile float _pressure;
        volatile float _velocity;
        DateTime _lastPoll;
        readonly uint UPDATE_INTERVAL_MS = 300;
        public SimPressureSensor(IClock clock, BouyancySystem bs, float startingPressure=0, float currentVelocity = 0)
        {
            _bs = bs;
            _pressure = startingPressure;
            _velocity = currentVelocity;
            _lastPoll = DateTime.Now;
            clock.Register(Poll);
        }

        void Poll()
        {
            var now = DateTime.Now;
            var elapsed = (now - _lastPoll);
            var millis = (uint)elapsed.TotalMilliseconds;
            if (millis >= UPDATE_INTERVAL_MS)
            {
                var newVelocity = VelocitySimulation.CalculateNewVelocity(_velocity,
                            deltaTInMilliSeconds: millis,
                            mass: VelocitySimulation.DEFAULT_MASS_KG + _bs.WaterMass);
                _pressure = PressureSimulation.CalculatePressure(newVelocity, _pressure, deltaTInMilliSeconds: millis);
                if (_pressure == 0.0 && newVelocity < 0.0)
                    newVelocity = 0;
                _velocity = newVelocity;
                
            }
        }
        public float Get()
        {
            return _pressure;
        }
        public float Velocity => _velocity;
    }
}
