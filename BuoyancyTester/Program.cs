using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoMini;
using ControlLogicMF;

namespace BuoyancyTester
{
    public class Program
    {
        public static bool _run = true;
        public static void Main()
        {
            var button = new Button(Pins.GPIO_PIN_20);
            var clock = new Clock(50);
            var buttonPressCounter = new ButtonPressCounter(clock, 500);
            button.OnPress += buttonPressCounter.RecordPress;
            buttonPressCounter.OnButtonStreamComplete += buttonPressCounter_OnButtonStreamComplete;

            while (_run)
            {
                System.Threading.Thread.Sleep(500);
            }

        }

        static void buttonPressCounter_OnButtonStreamComplete(uint arg)
        {
            Debug.Print("Got " + arg.ToString());
            if (arg > 1)
                _run = false;
        }

    }
}
