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
        public CommandHandler(IClock clock, Pump pump)
        {
            _pump = pump;
            clock.Register(Tick);
        }

        void Tick()
        {
            if (_state == State.MaintainingPosition)
            {
                Debug.Print("Maintaining position not implemented");
                ChangeState(State.ManualMode);
            }
        }

        public void OnButtonPress(Command cmd)
        {
            switch (cmd)
            {
                case Command.MaintainPosition:
                    // TODO: read current pressure
                    // initialize/reset state machine
                    // blah blah blah
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
