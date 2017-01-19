using System;


namespace ControlLogic
{
    /// <summary>
    /// The pump in this case allows pumping in our out. It is open loop, and runs for a
    /// specified period of time.
    /// Relies on the proved clock for notifications. The actual low-level control for the
    /// pump is injected as constructor params.
    /// </summary>
    public class Pump : IPump
    {
        IDigitalOutputPort _pumpOut;
        IDigitalOutputPort _pumpIn;
        ILogger _log;
        DateTime _lastOperationStarted = DateTime.MinValue;
        TimeSpan _duration;

        public Pump(IClock clock, ILogger log, IDigitalOutputPort output, IDigitalOutputPort input)
        {
            _pumpOut = output;
            _pumpIn = input;
            _log = log;
            _pumpIn.Set(false);
            _pumpOut.Set(false);
            
            clock.Register(Poll);
        }

        public void PumpOut(TimeSpan duration)
        {
            Start(_pumpOut, duration);
            _log.Log("Pumping out for " + duration.Ticks / 10000 + "ms");
        }
        public void PumpIn(TimeSpan duration)
        {
            Start(_pumpIn, duration);
            _log.Log("Pumping in for " + duration.Ticks / 10000 + "ms");
        }

        void Start(IDigitalOutputPort port, TimeSpan duration)
        {
            Stop();
            _duration = duration;
            _lastOperationStarted = DateTime.Now;
            port.Set(true);
        }

        void Poll()
        {
            if (_lastOperationStarted != DateTime.MinValue)
            {
                var currentDuration = DateTime.Now - _lastOperationStarted;
                if (currentDuration >= _duration)
                {
                    Stop();
                }
            }
        }
        public void Stop()
        {
            if (_lastOperationStarted != DateTime.MinValue)
            {
                _pumpIn.Set(false);
                _pumpOut.Set(false);
                _lastOperationStarted = DateTime.MinValue;
                _log.Log("Stopped pumping");
            }
        }
    }
}
