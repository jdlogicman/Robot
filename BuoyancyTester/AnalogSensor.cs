using System;
using Microsoft.SPOT;
using ControlLogicMF;
using Microsoft.SPOT.Hardware;

namespace BuoyancyTester
{
    class AnalogSensor : IHasValue
    {
        readonly SecretLabs.NETMF.Hardware.AnalogInput _input;
        public AnalogSensor(Cpu.Pin pin)
        {
            _input = new SecretLabs.NETMF.Hardware.AnalogInput(pin);
        }
        private const float ANALOG_REFERENCE_VOLTS = 3.30f;
        private const int MAX_COUNT = 1024;
        private const float VOLTS_PER_COUNT = ANALOG_REFERENCE_VOLTS / MAX_COUNT;

        
        public float Get()
        {
            return _input.Read();
        }
    }
}
