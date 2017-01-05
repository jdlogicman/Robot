using System;

namespace ControlLogicMF
{
    public class Pid : IHasValue
    {
        public Pid(IHasValue src, double p, double i, double d)
        {
            _pidFactor = p;
            _integralFactor = i;
            _derivativeFactor = d;
            _src = src;
        }

  
        readonly double _pidFactor;
        readonly double _integralFactor;
        readonly double _derivativeFactor;
        readonly IHasValue _src;

        double _lastError;
        double _integral;

        public float Get()
        {
            var error = _src.Get();
            _integral += error;
            var derivative = error - _lastError;

            double control = _pidFactor * error + _integralFactor * _integral + _derivativeFactor * derivative;
            _lastError = error;
            return (float)control;
        }
    }
}
