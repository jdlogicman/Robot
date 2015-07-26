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
        List<MockNotificationHandle> _timers = new List<MockNotificationHandle>();
        public INotificationHandle Register(TimeSpan interval, Action<INotificationHandle> callback)
        {
            var newTimer = new MockNotificationHandle(interval, callback);
            _timers.Add(newTimer);
            return newTimer;
        }


        public void Start()
        {
            foreach (var t in _timers)
            {
                t.Start();
            }
        }

        public void Stop()
        {
            foreach (var t in _timers)
            {
                t.Cancel();
            }
            _timers.Clear();
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
            }, TaskCreationOptions.LongRunning);
        }

        public void Start()
        {
            _task.Start();
        }
        public void Cancel()
        {
            if (!_sync.IsSet)
            {
                _sync.Set();
                _task.Wait();
            }
        }
    }

}
