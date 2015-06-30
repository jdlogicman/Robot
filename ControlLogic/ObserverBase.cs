using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class ObserverBase : IObservable<double>, IObserver<double>
    {
        protected List<IObserver<double>> _clients = new List<IObserver<double>>();
        public void OnCompleted()
        {
            foreach (var c in _clients)
                c.OnCompleted();
        }

        public virtual void OnError(Exception error)
        {
            foreach (var c in _clients)
                c.OnError(error);
        
        }

        public virtual void OnNext(double value)
        {
            foreach (var c in _clients)
                c.OnNext(value);
        }
        public virtual IDisposable Subscribe(IObserver<double> observer)
        {
            _clients.Add(observer);
            return new MyObserverCleanup(() => _clients.Remove(observer));
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
