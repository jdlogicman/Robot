using System;
using System.Collections;

namespace ControlLogicMF
{
    public class ObserverBase 
    {
        protected ArrayList _clients = new ArrayList();
        
        public virtual void OnNext(double value)
        {
            foreach (ObserverBase c in _clients)
                c.OnNext(value);
        }
        public virtual void AddObserver(ObserverBase observer)
        {
            _clients.Add(observer);
        }
        public virtual ObserverBase Chain(ObserverBase observer)
        {
            _clients.Add(observer);
            return observer;
        }
        
        private class MyObserverCleanup : IDisposable
        { 
            public MyObserverCleanup(Types.Action doit)
            {
                _doit = doit;
            }


            public void Dispose()
            {
                if (_doit != null)
                {
                    _doit();
                    _doit = null;
                }
            }
            Types.Action _doit;
        }

    }

}
