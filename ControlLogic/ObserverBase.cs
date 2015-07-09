using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class ObserverBase 
    {
        protected List<ObserverBase> _clients = new List<ObserverBase>();
        
        public virtual void OnNext(double value)
        {
            foreach (var c in _clients)
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
            public MyObserverCleanup(Action doit)
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
            Action _doit;
        }

    }

}
