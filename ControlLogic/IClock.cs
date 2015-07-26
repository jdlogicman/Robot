using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlLogic
{
    public interface INotificationHandle
    {
        void Cancel();
    }

    public interface IClock
    {
        INotificationHandle Register(TimeSpan interval, Action<INotificationHandle> callback);
    }
}
