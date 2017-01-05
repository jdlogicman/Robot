using System;

namespace ControlLogicMF
{
    public class AbsValueFilter : IFilterValue
    {
        readonly float _min;
        readonly IFilterValue _src;
        public AbsValueFilter(IFilterValue src, float minAbsValue)
        {
            _min = minAbsValue;
            _src = src;
        }


        public float Get(float input)
        {
            var val = _src.Get(input);
            if (System.Math.Abs(val) <= _min)
                return 0;
            return val;
        }
    }
}
