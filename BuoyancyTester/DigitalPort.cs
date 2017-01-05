using System;
using Microsoft.SPOT;
using ControlLogicMF;
using Microsoft.SPOT.Hardware;

namespace BuoyancyTester
{
    class DigitalOutputPort : IDigitalOutputPort
    {
        readonly OutputPort _port;
        public DigitalOutputPort(Cpu.Pin pin, bool initialState = false)
        {
            _port = new OutputPort(pin, initialState);
        }
        public void Set(bool value)
        {
            _port.Write(value);
        }
    }
}
