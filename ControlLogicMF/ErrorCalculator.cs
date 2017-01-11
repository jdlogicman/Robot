using System;

namespace ControlLogic
{
    public class ErrorCalculator : IHasValue
    {
        public ErrorCalculator(IHasValue src, float targetValue)
        {
            _targetValue = targetValue;
            _src = src;
        }
        readonly float _targetValue;
        readonly IHasValue _src;
        public float Get()
        {
            return _src.Get() - _targetValue;
        }
    }
}
