using System;
using Microsoft.SPOT;

namespace ControlLogicMF
{
    public class ErrorCalculator : IFilterValue
    {
        public ErrorCalculator(float targetValue)
        {
            _targetValue = targetValue;
        }
        readonly float _targetValue;
        public float Get(float input)
        {
            return _targetValue - input;
        }
    }
}
