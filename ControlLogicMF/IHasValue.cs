using System;

namespace ControlLogicMF
{
    public interface IHasValue
    {
        float Get();
    }
    public interface IFilterValue
    {
        float Get(float input);
    }

}
