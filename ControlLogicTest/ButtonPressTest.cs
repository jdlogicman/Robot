using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class ButtonPressTest
    {
        [TestMethod]
        public void TestDebounce()
        {
            uint count = 0;
            MockClock mc = new MockClock();
            ButtonPressCounter counter = new ButtonPressCounter(mc, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(50));

            counter.OnButtonStreamComplete += (c) => count = c;
            mc.Start();

            counter.RecordPress();
            counter.RecordPress();
            counter.RecordPress();
            Thread.Sleep(100);
            Assert.AreEqual<uint>(1, count);
            mc.Stop();
        }

        [TestMethod]
        public void TestStream()
        {
            MockClock mc = new MockClock();
            
            uint count = 0;
            ButtonPressCounter counter = new ButtonPressCounter(mc, TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(100));

            counter.OnButtonStreamComplete += (c) => count = c;
            mc.Start();

            counter.RecordPress();
            Thread.Sleep(11);
            counter.RecordPress();
            Thread.Sleep(11);
            Assert.AreEqual(0u, count);
            Thread.Sleep(300);
            Assert.AreEqual(2u, count);
            mc.Stop();
        }
    }
}
