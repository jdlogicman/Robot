using System;


namespace ControlLogicMF
{
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
        readonly Pump _pump;
        readonly IHasValue _pressureSensor;
        DateTime _lastControl;
        TimeSpan _controlInterval;

        IHasValue _velocityController;

        const float MAX_PRESSURE_CORRECTION = 0.2F;
        const float MIN_PRESSURE_CORRECTION = -0.2F;
        const float MIN_ABS_PRESSURE_ERROR_TO_CORRECT = 1.0F / 33;
        const float PUMP_CORRECTION_SCALE_VALUE = 1000; // TODO


        public PressureControlLoop(IClock clock, Pump pump, IHasValue pressureSensor, TimeSpan controlInterval)
        {
            _pressureSensor = pressureSensor;
            _pump = pump;
            _controlInterval = controlInterval;
            clock.Register(Poll);
        }

        public void Enable(float targetPressure)
        {
            _pump.Stop();
            // clamp the error to a narrow range
            // we don't care to make large corrections, but rather to take time getting there
            var pressureErrorCalculator = new AbsValueFilter(
                new Clamper(
                    new ErrorCalculator(_pressureSensor, targetPressure), MIN_PRESSURE_CORRECTION, MAX_PRESSURE_CORRECTION),
                MIN_ABS_PRESSURE_ERROR_TO_CORRECT);
            // simple proportional control for pressure
            var pressureController = new Pid(pressureErrorCalculator, -2, 0, 0);
            // for velocity, we might need some I. TODO - tune parameters
            _velocityController = new Pid(pressureController, 0.5f, 0, 0);
            
            _lastControl = DateTime.Now;
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
                var interval = DateTime.Now - _lastControl;
                if (interval >= _controlInterval)
                {
                    var velocityCorrection = _velocityController.Get();
                    if (velocityCorrection < 0)
                        _pump.PumpIn(new TimeSpan((long)(System.Math.Abs(velocityCorrection) * PUMP_CORRECTION_SCALE_VALUE)));
                    else if (velocityCorrection > 0)
                        _pump.PumpOut(new TimeSpan((long)(velocityCorrection * PUMP_CORRECTION_SCALE_VALUE)));
        
                }
            }
        }

    }
}
