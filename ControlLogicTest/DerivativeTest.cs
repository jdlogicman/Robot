﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class DerivativeCalculatorTest
    {
        [TestMethod]
        public void Test()
        {
            var valueSrc = new ValueSrc(new float[] { 1.0f, 1.0f, 2.0f, 1.0f, 1.0f });
            var testObj = new DerivativeCalculator(valueSrc);
            Assert.AreEqual(0f, testObj.Get());
            Thread.Sleep(100);
            Assert.AreEqual(0f, testObj.Get());
            Thread.Sleep(100);
            Assert.AreEqual(1.0f/0.1f, testObj.Get(), 0.1);
            Thread.Sleep(100);
            Assert.AreEqual(-1.0f / 0.1f, testObj.Get(), 0.1);
            Thread.Sleep(100);
            Assert.AreEqual(0f, testObj.Get(), 0.1);
        }
    }
}
