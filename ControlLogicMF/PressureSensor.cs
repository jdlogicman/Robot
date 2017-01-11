using System;



namespace ControlLogic
{
    /// <summary>
    /// This class models the Dwyer relative pressure sensor in that the analog
    /// voltage varies linearly with pressure.
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
        
        /// <summary>
        /// Gets the current value as atmospheric pressure (bar) ATA units
        /// </summary>
        /// <returns></returns>
        public float Get()
        {
            var volts = _input.Get();
            return volts * _voltsPerBar;
        }
    }
}
