using System;
using System.Text;


namespace ControlLogic
{
    public interface INotificationHandle
    {
        void Cancel();
    }

    public interface IClock
    {
        void Register(Types.Action callback);
        void Start();
        void Stop();
        DateTime Now { get; }
    }
}
