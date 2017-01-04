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
        static CommandHandler _handler;
        public static void Main()
        {
            var button = new Button(Pins.GPIO_PIN_20);
            var clock = new Clock(50);
            var buttonPressCounter = new ButtonPressCounter(clock, 500);
            var pump = new Pump(clock, Pins.GPIO_PIN_18, Pins.GPIO_PIN_19);
            var pressureSensor = new PressureSensor(Pins.GPIO_PIN_5);
            _handler = new CommandHandler(clock, pump, pressureSensor);
            button.OnPress += buttonPressCounter.RecordPress;
            buttonPressCounter.OnButtonStreamComplete += buttonPressCounter_OnButtonStreamComplete;

            Debug.Print("+++ started");

            while (_run)
            {
                System.Threading.Thread.Sleep(500);
            }

        }

        static void buttonPressCounter_OnButtonStreamComplete(uint arg)
        {
            Debug.Print("Got " + arg.ToString() + " presses");
            switch (arg)
            {
                case 1:
                    _handler.OnButtonPress(Command.PumpIn);
                    break;
                case 2:
                    _handler.OnButtonPress(Command.PumpOut);
                    break;
                case 3:
                    _handler.OnButtonPress(Command.Cancel);
                    break;
                default:
                    _handler.OnButtonPress(Command.MaintainPosition);
                    _run = false;
                    break;
            }
        }

    }
}
