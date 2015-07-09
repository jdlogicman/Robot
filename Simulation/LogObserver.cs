using ControlLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation
{
    public class LogObserver : ObserverBase
    {
        public LogObserver(string name) { _name = name;  }
        public override void OnNext(double value)
        {
            _values[_name] = value;
            base.OnNext(value);
        }

        string _name;
        static Dictionary<string, double> _values = new Dictionary<string, double>();
        static public Dictionary<string, double> Values { get { return _values; }}
        
    }
}
