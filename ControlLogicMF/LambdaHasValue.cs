using System;
using static ControlLogic.Types;

namespace ControlLogic
{
    public class LambdaHasValue : IHasValue
    {
        public LambdaHasValue(FloatFunc processor)
        {
            _processor = processor;
        }

        public float Get()
        {
            return _processor();
        }

        private FloatFunc _processor;

    }
}
