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
            ButtonPressCounter counter = new ButtonPressCounter(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(100));

            counter.OnButtonStreamComplete += (c) => count = c;
            counter.RecordPress();
            counter.Tick();
            counter.RecordPress();
            counter.RecordPress();
            counter.Tick();
            Thread.Sleep(120);
            counter.Tick();
            Assert.AreEqual<uint>(1, count);
        }

        [TestMethod]
        public void TestStream()
        {
            uint count = 0;
            ButtonPressCounter counter = new ButtonPressCounter(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(300));

            counter.OnButtonStreamComplete += (c) => count = c;
            counter.RecordPress();
            Thread.Sleep(11);
            counter.RecordPress();
            Thread.Sleep(11);
            Assert.AreEqual(0u, count);
            counter.Tick();
            Assert.AreEqual(0u, count);
            Thread.Sleep(300);
            counter.Tick();
            Assert.AreEqual(2u, count);
        }
    }
}
