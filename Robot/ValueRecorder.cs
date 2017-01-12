using ControlLogic;
using System;

namespace Robot
{
    class ValueRecorder : IHasValue
    {
        IHasValue _src;
        Action<float> _callback;
        public ValueRecorder(Action<float> callback, IHasValue src)
        {
            _src = src;
            _callback = callback;
        }
        public float Get()
        {
            var val = _src.Get();
            _callback(val);
            return val;
        }
    }
}
