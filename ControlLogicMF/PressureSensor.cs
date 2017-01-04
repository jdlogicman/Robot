using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoMini;


namespace ControlLogicMF
{
    /// <summary>
    /// This class models the Dwyer relative pressure sensor.
    /// </summary>
    public class PressureSensor : IHasValue
    {
        readonly SecretLabs.NETMF.Hardware.AnalogInput _input;
        readonly float _voltsPerBar;
        public PressureSensor(Cpu.Pin pin, float voltsPerBar = 0.5f)
        {
            _input = new SecretLabs.NETMF.Hardware.AnalogInput(pin);
            _voltsPerBar = voltsPerBar;
        }
        private const float ANALOG_REFERENCE_VOLTS = 3.30f;
        private const int MAX_COUNT = 1024;
        private const float VOLTS_PER_COUNT = ANALOG_REFERENCE_VOLTS / MAX_COUNT;

        public float Get()
        {
            var value = _input.Read();
            var volts = value * VOLTS_PER_COUNT;
            return volts * _voltsPerBar;
        }
    }
}
