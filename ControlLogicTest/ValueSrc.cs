using ControlLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogicTest
{
    class ValueSrc : IHasValue
    {
        Queue<float> _values;
        public ValueSrc(IEnumerable<float> values)
        {
            _values = new Queue<float>(values);
        }
        public float Get()
        {
            return _values.Dequeue();
        }
    }
}
