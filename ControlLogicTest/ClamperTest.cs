using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;
using System.Collections.Generic;

namespace ControlLogicTest
{
    [TestClass]
    public class ClamperTest
    {
        [TestMethod]
        public void TestMinMaxMiddle()
        {
            var src = new ValueSrc(new float[] { -10f, 10f, 2.1f });
            Clamper c = new Clamper(src, -0.5f, 3.5f);
            Assert.AreEqual(-0.5, c.Get());
            Assert.AreEqual(3.5, c.Get());
            Assert.IsTrue(Math.Abs(2.1 - c.Get()) < 0.1);
        }
    }
}
