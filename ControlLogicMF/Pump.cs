using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoMini;
using Microsoft.SPOT;

namespace ControlLogicMF
{
    public class Pump 
    {
        OutputPort _pumpOut;
        OutputPort _pumpIn;
        DateTime _lastOperationStarted = DateTime.MinValue;
        TimeSpan _duration;

        public Pump(IClock clock, Cpu.Pin outPin, Cpu.Pin inPin)
        {
            _pumpOut = new OutputPort(outPin, false);
            _pumpIn = new OutputPort(inPin, false);
            clock.Register(Poll);
        }

        public void PumpOut(TimeSpan duration)
        {
            Start(_pumpOut, duration);
            Debug.Print("Pumping out for " + duration.Ticks / 10000 + "ms");
        }
        public void PumpIn(TimeSpan duration)
        {
            Start(_pumpOut, duration);
            Debug.Print("Pumping in for " + duration.Ticks / 10000 + "ms");
        }

        void Start(OutputPort port, TimeSpan duration)
        {
            Stop();
            _duration = duration;
            _lastOperationStarted = DateTime.Now;
            port.Write(true);
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
                _pumpIn.Write(false);
                _pumpOut.Write(false);
                _lastOperationStarted = DateTime.MinValue;
                Debug.Print("Stopped pumping");
            }
        }
    }
}
