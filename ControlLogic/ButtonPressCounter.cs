using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public class ButtonPressCounter 
    {
        TimeSpan _debounceInterval;
        TimeSpan _buttonPressGroupFinalization;
        DateTime _lastPress;
        INotificationHandle _notifyHandle;
        bool _debouncing;
        uint _count;

        public event Action<uint> OnButtonStreamComplete;

        public ButtonPressCounter(IClock clock, TimeSpan debounceInterval, TimeSpan buttonPressGroupFinalization)
        {
            if (buttonPressGroupFinalization.TotalMilliseconds <= 2*debounceInterval.TotalMilliseconds)
                throw new ArgumentOutOfRangeException("debounceInterval", "Group finalization interval must be GT 2*debounce interval");
            _debounceInterval = debounceInterval;
            _buttonPressGroupFinalization = buttonPressGroupFinalization;
            _lastPress = DateTime.MinValue;
            _notifyHandle = clock.Register(buttonPressGroupFinalization, (nc) => Tick());
        }
        public void RecordPress()
        {
            FinishDebounce();
            if (!_debouncing)
            {
                _lastPress = DateTime.Now;
                _debouncing = true;
            } 
            
        }
        void Tick()
        {
            FinishDebounce();
            if (_count > 0 && !_debouncing && (DateTime.Now - _lastPress) > _buttonPressGroupFinalization)
            {
                var listeners = OnButtonStreamComplete;
                var count = _count;
                _count = 0;
                if (listeners != null)
                    listeners(count);
            }
        }
        void FinishDebounce()
        {
            if (_debouncing && (DateTime.Now - _lastPress) > _debounceInterval)
            {
                _count++;
                _debouncing = false;
            }
        }
    }
}
