using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ControlLogic;

namespace Simulation
{
    public class LambdaObserver : ObserverBase
    {
        public LambdaObserver(Func<double, double> processor)
        {
            _processor = processor;
        }

        // this class observes the velocit changes
        public override void OnNext(double value)
        {
            base.OnNext(_processor(value));
        }

        private Func<double, double> _processor;
        
    }
}
