using System;
using Microsoft.SPOT;
using ControlLogicMF;

namespace BuoyancyTester
{
    public enum Command
    {
        PumpOut,
        PumpIn,
        Cancel,
        MaintainPosition
    }

    class CommandHandler
    {
        enum State
        {
            MaintainingPosition,
            ManualMode
        }
        State _state = State.ManualMode;
        Pump _pump;
        IHasValue _pressureSensor;
        
        float _targetPressure;
        IHasValue _velocityController;
        
        const float MAX_PRESSURE_CORRECTION = 0.2F;
        const float MIN_PRESSURE_CORRECTION = -0.2F;
        const float MIN_ABS_PRESSURE_ERROR_TO_CORRECT = 1.0F / 33;
        const float PUMP_CORRECTION_SCALE_VALUE = 1000; // TODO


        public CommandHandler(IClock clock, Pump pump, IHasValue pressureSensor)
        {
            _pump = pump;
            _pressureSensor = new Averager(4, pressureSensor);
            clock.Register(Tick);
        }

        void Tick()
        {
            if (_state == State.MaintainingPosition)
            {
                // TODO: only correct once every 5 seconds

                var velocityCorrection = _velocityController.Get();
                if (velocityCorrection < 0)
                    _pump.PumpIn(new TimeSpan((long)(System.Math.Abs(velocityCorrection) * PUMP_CORRECTION_SCALE_VALUE)));
                else if (velocityCorrection > 0)
                    _pump.PumpOut(new TimeSpan((long)(velocityCorrection * PUMP_CORRECTION_SCALE_VALUE)));
            }
        }

        public void OnButtonPress(Command cmd)
        {
            switch (cmd)
            {
                case Command.MaintainPosition:
                    // this mode maintains the current pressure, whatever that is - so read it
                    _targetPressure = _pressureSensor.Get();
                    {
                        // clamp the error to a narrow range
                        // we don't care to make large corrections, but rather to take time getting there
                        var pressureErrorCalculator = new AbsValueFilter(
                            new Clamper(
                                new ErrorCalculator(_pressureSensor, _targetPressure), MIN_PRESSURE_CORRECTION, MAX_PRESSURE_CORRECTION),
                            MIN_ABS_PRESSURE_ERROR_TO_CORRECT);
                        // simple proportional control for pressure
                        var pressureController = new Pid(pressureErrorCalculator, -2, 0, 0);
                        // for velocity, we might need some I. TODO - tune parameters
                        _velocityController = new Pid(pressureController, -0.5, 0, 0);
                    }
                    ChangeState(State.MaintainingPosition);
                    _pump.Stop();
                    break;
                case Command.PumpIn:
                    ChangeState(State.ManualMode);
                    _pump.PumpIn(new TimeSpan(0,0,0,0,500));
                    break;
                case Command.PumpOut:
                    ChangeState(State.ManualMode);
                    _pump.PumpOut(new TimeSpan(0,0,0,0,500));
                    break;
                case Command.Cancel:
                    ChangeState(State.ManualMode);
                    _pump.Stop();
                    break;
            }
        }

        private bool ChangeState(State newState)
        {
            if (newState != _state)
            {
                Debug.Print("State " + _state + " -> " + newState);
                _state = newState;
                return true;
            }
            Debug.Print("Already in " + _state);
            return false;
        }

    }
}
