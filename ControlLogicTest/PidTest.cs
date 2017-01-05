using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;

namespace ControlLogicTest
{
    [TestClass]
    public class PidTest
    {
        [TestMethod]
        public void TestP()
        {
            var src = new ValueSrc(new[] { 2.0f, -2.0f, 0f });
            var err = new ErrorCalculator(src, 0);
            var pid = new Pid(err, 2, 0, 0);
            Assert.IsTrue(Math.Abs(pid.Get() - 4) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get() + 4) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get()) < 0.01);
        }

        [TestMethod]
        public void TestI()
        {
            var src = new ValueSrc(new[] { 2.0f, 2.0f, 0f, -2f, -2f, 0f });
            var err = new ErrorCalculator(src, 0);
            var pid = new Pid(err, 0, 2, 0);
            Assert.IsTrue(Math.Abs(pid.Get() - 4) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get() - 8) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get() - 8) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get() - 4) < 0.01);
            Assert.IsTrue(Math.Abs(pid.Get()) < 0.01);
        }
    }
}
