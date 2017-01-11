using ControlLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogicTest
{
    class MockDigitalPort : IDigitalOutputPort
    {
        public bool State { get; set; }
        public TimeSpan Duration { get; private set; }
        DateTime _triggered;
        public int Activations { get; set; }

        public MockDigitalPort() { State = true; }
        public void Set(bool val)
        {
            if (!State && val)
            {
                _triggered = DateTime.Now;
                Activations++;
            }
            else if (State && !val)
                Duration = DateTime.Now - _triggered;

            State = val;
        }
    }
    class MockLog : ILogger
    {
        public void Log(string s) { }
    }

    class MockHasOneValue : IHasValue
    {
        float _value;

        public MockHasOneValue(float val)
        {
            _value = val;
        }
        public float Value { set { _value = value; } }

        public float Get()
        {
            return _value;
        }
    }
	
}
