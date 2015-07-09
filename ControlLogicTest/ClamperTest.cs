using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
using Simulation;

namespace ControlLogicTest
{
    [TestClass]
    public class ClamperTest
    {
        [TestMethod]
        public void TestMinMaxMiddle()
        {
            Clamper c = new Clamper(-0.5, 3.5);
            double value = 0;
            c.AddObserver(new LambdaObserver((d) => value = d));

            c.OnNext(-10);
            Assert.AreEqual(-0.5, value);
            c.OnNext(10);
            Assert.AreEqual(3.5, value);
            c.OnNext(2.1);
            Assert.AreEqual(2.1, value);
        }
    }
}
