using System;

namespace ControlLogicMF
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
            return _targetValue - _src.Get(); ;
        }
    }
}
