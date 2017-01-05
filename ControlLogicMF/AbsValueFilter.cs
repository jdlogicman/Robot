using System;

namespace ControlLogicMF
{
    public class AbsValueFilter : IHasValue
    {
        readonly float _min;
        readonly IHasValue _src;
        public AbsValueFilter(IHasValue src, float minAbsValue)
        {
            _min = minAbsValue;
            _src = src;
        }


        public float Get()
        {
            var val = _src.Get();
            if (System.Math.Abs(val) <= _min)
                return 0;
            return val;
        }
    }
}
