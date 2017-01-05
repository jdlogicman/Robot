using System;



namespace ControlLogicMF
{
    /// <summary>
    /// This class models the Dwyer relative pressure sensor.
    /// </summary>
    public class PressureSensor : IHasValue
    {
        readonly IHasValue _input;
        readonly float _voltsPerBar;
        public PressureSensor(IHasValue input, float voltsPerBar = 0.5f)
        {
            _input = input;
            _voltsPerBar = voltsPerBar;
        }
        
        public float Get()
        {
            var volts = _input.Get();
            return volts * _voltsPerBar;
        }
    }
}
