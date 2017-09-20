using System;

namespace ControlLogic
{
    /// <summary>
    /// A simple class that generates notifications approximately at the polling interval.
    /// Guarantees that all handlers have completed before the next notification, even if that
    /// means delaying notification (i.e. guarantees no reentrancy issues).
    /// </summary>
    public class Clock : IClock, IDisposable
    {
        readonly System.Threading.Timer _timer;
        event Types.Action _listeners;
        bool _running = true;
        readonly int _interval;
        public void Dispose()
        {
            if (_running)
            {
                _running = false;
                _timer.Dispose();
            }
        }
        public Clock(int pollIntervalMs)
        {
            _interval = pollIntervalMs;
            _timer = new System.Threading.Timer(Callback, null, _interval, -1);
        }

        public DateTime Now {  get { return DateTime.Now;  } }

        private void Callback(object state)
        {
            var listeners = _listeners;
                
            if (_running && listeners != null)
            {
                listeners();
            }
            _timer.Change(_interval, -1);
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
    }
}
