using System;
using Microsoft.SPOT;

namespace ControlLogicMF
{
    public class Averager : IHasValue
    {
        int _numReadings;
        IHasValue _src;
        public Averager(int numReadings, IHasValue src)
        {
            _numReadings = numReadings;
            _src = src;
        }
        public float Get()
        {
            float total = 0;
            for (var i = 0; i < _numReadings; i++)
                total += _src.Get();
            return total / _numReadings;
        }
    }
}
