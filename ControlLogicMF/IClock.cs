using System;
using System.Text;


namespace ControlLogicMF
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
    }
}
