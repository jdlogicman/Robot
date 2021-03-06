﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
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
            using (var clock = new ClockMock(50))
            {
                ButtonPressCounter counter = new ButtonPressCounter(clock, 50);

                counter.OnButtonStreamComplete += (c) => count = c;
                clock.Start();

                counter.RecordPress();
                counter.RecordPress();
                counter.RecordPress();
                clock.Elapse(200);
                Assert.AreEqual<uint>(3, count);
                count = 0;
                counter.RecordPress();
                Assert.AreEqual<uint>(0, count);
                clock.Elapse(200);
                Assert.AreEqual<uint>(1, count);
            }
        }

    }
}
