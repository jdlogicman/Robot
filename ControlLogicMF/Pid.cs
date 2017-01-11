using System;

namespace ControlLogicMF
{
    public class Pid : IHasValue
    {
        public Pid(IHasValue src, float p, float i, float d)
        {
            _pidFactor = p;
            _integralFactor = i;
            _derivativeFactor = d;
            _src = src;
        }

  
        readonly float _pidFactor;
        readonly float _integralFactor;
        readonly float _derivativeFactor;
        readonly IHasValue _src;

        float _lastError;
        float _integral;

        public float Get()
        {
            var error = _src.Get();
            _integral += error;
            var derivative = error - _lastError;

            float control = _pidFactor * error + _integralFactor * _integral + _derivativeFactor * derivative;
            _lastError = error;
            return (float)control;
        }
    }
}
