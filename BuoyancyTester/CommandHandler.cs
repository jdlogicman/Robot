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
        readonly Pump _pump;
        readonly IHasValue _pressureSensor;
        readonly PressureControlLoop _controlLoop;
        

        public CommandHandler(IClock clock, Pump pump, IHasValue pressureSensor)
        {
            _pump = pump;
            _pressureSensor = new Averager(4, pressureSensor);
            _controlLoop = new PressureControlLoop(clock, _pump, _pressureSensor, new TimeSpan(0, 0, 3));
        }

        public void OnButtonPress(Command cmd)
        {
            switch (cmd)
            {
                case Command.MaintainPosition:
                    ChangeState(State.MaintainingPosition);
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
                if (newState == State.MaintainingPosition)
                {
                    // change tom maintain whatever the current pressure is
                    _controlLoop.Enable(_pressureSensor.Get());
                }
                else
                {
                    _controlLoop.Disable();
                }
                _state = newState;
                return true;
            }
            Debug.Print("Already in " + _state);
            return false;
        }

    }
}
