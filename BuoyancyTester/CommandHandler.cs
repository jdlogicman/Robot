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
        IFilterValue _pressureController;
        IFilterValue _velocityController;
        
        const float MAX_PRESSURE_CORRECTION = 0.2F;
        const float MIN_PRESSURE_CORRECTION = -0.2F;
        const float MIN_ABS_VELOCITY_TO_CORRECT = 0.001F; // TODO: standardize time units
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

                var currentPressure = _pressureSensor.Get();
                var pressureError = _targetPressure - currentPressure;
                // clamp the pressure
                var pressureCorrection = (float)System.Math.Min(MAX_PRESSURE_CORRECTION, 
                                            System.Math.Max(MIN_PRESSURE_CORRECTION, 
                                            _pressureController.Get(pressureError)));
                var velocityCorrection = _velocityController.Get(pressureCorrection);
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
                    _targetPressure = _pressureSensor.Get();
                    _pressureController = new Clamper(new Pid(-2, 0, 0), MIN_PRESSURE_CORRECTION, MAX_PRESSURE_CORRECTION);
                    _velocityController = new AbsValueFilter(new Pid(-0.5, 0, 0), MIN_ABS_VELOCITY_TO_CORRECT);
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
