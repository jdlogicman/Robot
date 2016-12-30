using System;

namespace ControlLogicMF
{
    public class Pid : ObserverBase
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

        public override void OnNext(double error)
        {
            _integral += error;
            var derivative = error - _lastError;

            double control = _pidFactor * error + _integralFactor * _integral + _derivativeFactor * derivative;
            base.OnNext(control);
            _lastError = error;
        }
    }
}
