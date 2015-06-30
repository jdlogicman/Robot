using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class Clamper : ObserverBase
    {
        public Clamper(double min, double max)
        {
            _min = min;
            _max = max;
        }
        double _min;
        double _max;
        override public void OnNext(double value)
        {
            base.OnNext(Math.Max(_min, Math.Min(_max, value)));
        }
    }
}
