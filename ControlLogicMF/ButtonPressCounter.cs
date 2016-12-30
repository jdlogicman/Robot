using System;
using System.Text;

namespace ControlLogicMF
{
    public class ButtonPressCounter 
    {
        TimeSpan _buttonPressGroupFinalization;
        DateTime _lastPress;
        uint _count;

        public event Types.ActionUint OnButtonStreamComplete;

        public ButtonPressCounter(IClock clock, int buttonPressGroupFinalizationMs)
        {
            _buttonPressGroupFinalization = new TimeSpan(0,0,0,0, buttonPressGroupFinalizationMs);
            _lastPress = DateTime.MinValue;
            clock.Register(Tick);
        }
        public void RecordPress()
        {
            _lastPress = DateTime.Now;
            _count++;
        }
        void Tick()
        {
            if (_count > 0 && (DateTime.Now - _lastPress) > _buttonPressGroupFinalization)
            {
                var listeners = OnButtonStreamComplete;
                var count = _count;
                _count = 0;
                if (listeners != null)
                    listeners(count);
            }
        }
        
    }
}
