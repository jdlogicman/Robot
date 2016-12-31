using System;
using Microsoft.SPOT;

namespace ControlLogicMF
{
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
