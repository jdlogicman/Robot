using System;

namespace ControlLogicMF
{
    public class Clamper : IFilterValue
    {
        public Clamper(IFilterValue src, float min, float max)
        {
            _min = min;
            _max = max;
            _src = src;
        }
        float _min;
        float _max;
        IFilterValue _src;
        public float Get(float val)
        {
            return (float)Math.Max(_min, Math.Min(_max, _src.Get(val)));
        }
    }
}
