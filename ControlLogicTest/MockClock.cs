using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlLogic;
using System.Threading;

namespace ControlLogicTest
{
    class MockClock : IClock
    {
        public INotificationHandle Register(TimeSpan interval, Action<INotificationHandle> callback)
        {
            return new MockNotificationHandle(interval, callback);
        }
    }

    class MockNotificationHandle : INotificationHandle
    {
        ManualResetEventSlim _sync = new ManualResetEventSlim(false);
        Task _task;
        public MockNotificationHandle(TimeSpan interval, Action<INotificationHandle> callback)
        {
            _task = new Task(() =>
            { 
                while (!_sync.Wait(interval))
                {
                    callback(this);
                }
            });
            _task.Start();
        }
        public void Cancel()
        {
            _sync.Set();
            _task.Wait();
        }
    }

}
