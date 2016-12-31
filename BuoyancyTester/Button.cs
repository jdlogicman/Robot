using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoMini;


namespace BuoyancyTester
{
    class Button
    {
        InterruptPort _port;
        public Button(Cpu.Pin pin, bool risingEdge = true)
        {
            _port = new InterruptPort(pin, false,Port.ResistorMode.Disabled,
                risingEdge ? Port.InterruptMode.InterruptEdgeHigh : Port.InterruptMode.InterruptEdgeLow);
            _port.OnInterrupt += _port_OnInterrupt;
        }

        void _port_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            var handler = OnPress;
            if (handler != null)
                handler();
        }

        public delegate void Action();

        public event Action OnPress;
    }
}
