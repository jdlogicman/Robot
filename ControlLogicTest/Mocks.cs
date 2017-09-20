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
        IClock _clock;
        public int Activations { get; set; }


        public MockDigitalPort(IClock clock)
        {
            _clock = clock;
            _triggered = _clock.Now;

            State = true;
        }
        public void Set(bool val)
        {
            if (!State && val)
            {
                _triggered = _clock.Now;
                Activations++;
            }
            else if (State && !val)
                Duration = _clock.Now - _triggered;

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
    
    class ClockMock : IClock, IDisposable
    {
        event Types.Action _listeners;
        bool _running = true;
        readonly uint _interval;
        readonly DateTime _start = DateTime.Now;
        uint _elapsedMillis;
        public ClockMock(uint pollIntervalMs)
        {
            _interval = pollIntervalMs;
            _elapsedMillis = 0;
        }

        public void Elapse(uint millis)
        {
            while (millis > 0)
            {
                var listeners = _listeners;
                var interval = Math.Min(millis, _interval);

                _elapsedMillis += interval;
                millis -= interval;

                if (_running)
                {
                    listeners?.Invoke();
                }
            }
        }

        public DateTime Now
        {
            get
            {
                return _start.AddMilliseconds(_elapsedMillis);
            }
        }

        public void Register(Types.Action callback)
        {
            _listeners += callback;
        }

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }

}
