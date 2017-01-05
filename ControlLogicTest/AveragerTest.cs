using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;

namespace ControlLogicTest
{
    [TestClass]
    public class AveragerTest
    {
        [TestMethod]
        public void Averager()
        {
            var src = new ValueSrc(new[] { 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f });
            Averager s = new Averager(3, src);
            Assert.AreEqual(2.5, s.Get());
            Assert.AreEqual(5.5, s.Get());
        }
    }
}
