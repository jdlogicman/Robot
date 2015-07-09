using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class Smoother : ObserverBase
    {
        public Smoother(uint capacity) 
        {
            _values = new double[(int)capacity];
        }

        override public void OnNext(double value)
        { 
            _values[_position++ % _values.Length] = value;
            if (_position >= _values.Length)
                base.OnNext(_values.Average());
        }
        double[] _values;
        uint _position;
    }
}
