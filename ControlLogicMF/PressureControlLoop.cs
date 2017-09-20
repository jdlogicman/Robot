using System;


namespace ControlLogic
{
    public class ControlParameters
    {
        public float PressureP { get; set; }
        public float PressureI { get; set; }
        public float PressureD { get; set; }
        public float VelocityP { get; set; }
        public float VelocityI { get; set; }
        public float VelocityD { get; set; }
    }
    /// <summary>
    /// A control loop to maintain a target pressure.
    /// Since I want a gradual convergence, I'm doing control on desired target velocity:
    /// - calculate the pressure error
    /// - calculate the desired target velocity by applying a scalar and clamping to +/- MAX_VELOCITY
    /// - use the desired target velocity - actual velocity as the error in a PID control loop
    /// Note that the PID parameters are tied to the control loop period.
    /// </summary>
    public class PressureControlLoop
    {
        readonly IPump _pump;
        readonly IHasValue _pressureSensor;
        readonly IClock _clock;
        DateTime _lastControl;
        TimeSpan _controlInterval;
        float _currentPressureReading;
        volatile float _lastCorrection;

        IHasValue _velocityController;

        const float PUMP_CORRECTION_SCALE_VALUE = 1000; // transform from sec to millisec
        const float MAX_ABS_VELOCITY = 0.2f; // bar/s

        private readonly ControlParameters _controlParameters;

        public PressureControlLoop(IClock clock, IPump pump, IHasValue pressureSensor,
            ControlParameters controlParams, TimeSpan controlInterval)
        {
            _pressureSensor = pressureSensor;
            _pump = pump;
            _controlInterval = controlInterval;
            _controlParameters = controlParams;
            _clock = clock;
            _clock.Register(Poll);
        }

        public float LastCorrection => _lastCorrection;

        public void Enable(float targetPressure)
        {
            _pump.Stop();
            var pressureErrorCalculator = new LambdaHasValue(() => _currentPressureReading - targetPressure);

            var pressureController = new Pid(pressureErrorCalculator,
                _controlParameters.PressureP, _controlParameters.PressureI, _controlParameters.PressureD);

            var velocity = new DerivativeCalculator(_clock, new LambdaHasValue(() => _currentPressureReading));

            var velocityErrorCalculator = new Clamper(new LambdaHasValue(() =>
                {
                    return velocity.Get() - pressureController.Get();

                }), -MAX_ABS_VELOCITY, MAX_ABS_VELOCITY);

            _velocityController = new Pid(velocityErrorCalculator, 
                _controlParameters.VelocityP, _controlParameters.VelocityI, _controlParameters.VelocityD);

            _lastControl = _clock.Now;
        }
        public void Disable()
        {
            _pump.Stop();
            _lastControl = DateTime.MinValue;
        }


        void Poll()
        {
            if (_lastControl != DateTime.MinValue)
            {
                var interval = _clock.Now - _lastControl;
                if (interval >= _controlInterval)
                {
                    _currentPressureReading = _pressureSensor.Get();
                    _lastCorrection = _velocityController.Get();
                    
                    if (_lastCorrection > 0)
                        _pump.PumpIn(new TimeSpan((long)(System.Math.Abs(_lastCorrection) * PUMP_CORRECTION_SCALE_VALUE)));
                    else if (_lastCorrection < 0)
                        _pump.PumpOut(new TimeSpan((long)(_lastCorrection * PUMP_CORRECTION_SCALE_VALUE)));
        
                }
            }
        }

    }
}
