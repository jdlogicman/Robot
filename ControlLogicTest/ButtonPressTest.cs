using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class ButtonPressTest
    {
        [TestMethod]
        public void Test()
        {
            uint count = 0;
            var clock = new Clock(50);
            ButtonPressCounter counter = new ButtonPressCounter(clock, 50);

            counter.OnButtonStreamComplete += (c) => count = c;
            clock.Start();

            counter.RecordPress();
            counter.RecordPress();
            counter.RecordPress();
            Thread.Sleep(200);
            Assert.AreEqual<uint>(3, count);
            count = 0;
            counter.RecordPress();
            Assert.AreEqual<uint>(0, count);
            Thread.Sleep(100);
            Assert.AreEqual<uint>(1, count);
            clock.Stop();
        }

    }
}
