using System;

namespace ControlLogicMF
{
    public class Clamper : IHasValue
    {
        public Clamper(IHasValue src, float min, float max)
        {
            _min = min;
            _max = max;
            _src = src;
        }
        float _min;
        float _max;
        IHasValue _src;
        public float Get()
        {
            return (float)Math.Max(_min, Math.Min(_max, _src.Get()));
        }
    }
}
