using System;
using Microsoft.SPOT;

namespace ControlLogicMF
{
    public class Clock : IClock, IDisposable
    {
        System.Threading.Timer _timer;
        event Types.Action _listeners;
        bool _running = true;
        public void Dispose()
        {
            if (_running)
            {
                _running = false;
                _timer.Dispose();
                _timer = null;
            }
        }
        public Clock(int pollIntervalMs)
        {
            _timer = new System.Threading.Timer(Callback, null, 0, pollIntervalMs);
        }

        private void Callback(object state)
        {
            var listeners = _listeners;
                
            if (_running && listeners != null)
            {
                listeners();
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
    }
}
