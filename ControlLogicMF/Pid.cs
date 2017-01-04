using System;

namespace ControlLogicMF
{
    public class Pid : IFilterValue
    {
        public Pid(double p, double i, double d)
        {
            _pidFactor = p;
            _integralFactor = i;
            _derivativeFactor = d;
        }

  
        readonly double _pidFactor;
        readonly double _integralFactor;
        readonly double _derivativeFactor;

        double _lastError;
        double _integral;

        public float Get(float error)
        {
            _integral += error;
            var derivative = error - _lastError;

            double control = _pidFactor * error + _integralFactor * _integral + _derivativeFactor * derivative;
            _lastError = error;
            return (float)control;
        }
    }
}
