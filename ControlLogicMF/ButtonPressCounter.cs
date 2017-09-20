using System;
using System.Text;

namespace ControlLogic
{
    public class ButtonPressCounter 
    {
        TimeSpan _buttonPressGroupFinalization;
        DateTime _lastPress;
        IClock _clock;
        uint _count;

        public event Types.ActionUint OnButtonStreamComplete;

        public ButtonPressCounter(IClock clock, int buttonPressGroupFinalizationMs)
        {
            _buttonPressGroupFinalization = new TimeSpan(0,0,0,0, buttonPressGroupFinalizationMs);
            _lastPress = DateTime.MinValue;
            _clock = clock;
            _clock.Register(Tick);
        }
        public void RecordPress()
        {
            _lastPress = _clock.Now;
            _count++;
        }
        void Tick()
        {
            if (_count > 0 && (_clock.Now - _lastPress) > _buttonPressGroupFinalization)
            {
                var listeners = OnButtonStreamComplete;
                var count = _count;
                _count = 0;
                listeners?.Invoke(count);
            }
        }
        
    }
}
