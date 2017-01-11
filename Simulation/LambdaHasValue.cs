using System;
using ControlLogicMF;

namespace Simulation
{
    public class LambdaHasValue : IHasValue
    {
        public LambdaHasValue(Func<float> processor)
        {
            _processor = processor;
        }

        public float Get()
        {
            return _processor();
        }

        private Func<float> _processor;

    }
}
