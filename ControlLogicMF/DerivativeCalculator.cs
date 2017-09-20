using System;

namespace ControlLogic
{
    public class DerivativeCalculator : IHasValue
    {
        float _lastValue;
        DateTime _lastSample = DateTime.MinValue;
        IClock _clock;
        IHasValue _src;

        public DerivativeCalculator(IClock clock, IHasValue src)
        {
            _clock = clock;
            _src = src;
        }

        public float Get()
        {
            var result = 0f;
            var newValue = _src.Get();
            var now = _clock.Now;
            if (_lastSample != DateTime.MinValue && _lastSample != now)
            {
                result = (newValue - _lastValue) / (float)(now - _lastSample).TotalSeconds();
            }
            _lastSample = now;
            _lastValue = newValue;
            return result;
             
        }
    }
}
