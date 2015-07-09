using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
using Simulation;

namespace ControlLogicTest
{
    [TestClass]
    public class SmootherTest
    {
        [TestMethod]
        public void TestSmoother()
        {
            Smoother s = new Smoother(3);
            double value = 0.0;
            s.AddObserver(new LambdaObserver(d => value = d));

            s.OnNext(1.5);
            s.OnNext(2.5);
            s.OnNext(3.5);
            Assert.AreEqual(2.5, value);
            s.OnNext(4.5);
            Assert.AreEqual(3.5, value);
            s.OnNext(4.5);
            Assert.AreEqual((3.5 + 9)/3, value);
            
        }
    }
}
