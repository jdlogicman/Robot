using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class TimeAdapter: ObserverBase
    {
        public TimeAdapter(uint entriesToCollapse) 
        {
            _values = new double[(int)entriesToCollapse];
        }

        override public void OnNext(double value)
        {
            bool notify = _position > 0 && _position % _values.Length == 0;
            _values[_position++ % _values.Length] = value;
            if (notify)
            {
                base.OnNext(_values.Skip(_values.Length - 10).Take(10).Average());
            }
        }
        double[] _values;
        uint _position;
    }
}
