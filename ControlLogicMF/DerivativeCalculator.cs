using System;
using Microsoft.SPOT;

namespace ControlLogicMF
{
    public class DerivativeCalculator : IFilterValue
    {
        float _lastValue;
        DateTime _lastReading = DateTime.MinValue;
        public DerivativeCalculator()
        {
        }
        public float Get(float input)
        {
            var last = _lastReading;
            _lastReading = DateTime.Now;
            if (last == DateTime.MinValue)
            {
                _lastValue = input;
                return 0;
            }
            // convert from ticks to milliseconds
            var result = (_lastValue - input) / ((_lastReading - last).Ticks / 10000.0);
            _lastValue = input;
            return (float)result;
        }
    }
}
