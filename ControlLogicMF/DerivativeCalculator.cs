using System;

namespace ControlLogic
{
    public class DerivativeCalculator : IHasValue
    {
        float _lastValue;
        DateTime _lastSample = DateTime.MinValue;
        IHasValue _src;

        public DerivativeCalculator(IHasValue src)
        {
            _src = src;
        }

        public float Get()
        {
            var result = 0f;
            var newValue = _src.Get();
            var now = DateTime.Now;
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
