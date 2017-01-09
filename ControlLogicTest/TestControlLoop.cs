using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class TestControlLoop
    {
        MockDigitalPort _in;
        MockDigitalPort _out;
        Pump _pump;
        Clock _clock;
        [TestInitialize]
        public void Init()
        {
            _clock = new Clock(50);
            _in = new MockDigitalPort();
            _out = new MockDigitalPort();
            _pump = new Pump(_clock, new MockLog(), _out, _in);

        }
        [TestCleanup]
        public void Cleanup()
        {
            _clock.Dispose();
        }
        [TestMethod]
        public void TestEnableDisable()
        {
            var pressure = new MockHasOneValue(0.5f);
            var loop = new PressureControlLoop(_clock, _pump, pressure, TimeSpan.FromMilliseconds(100));
            Thread.Sleep(500);
            Assert.AreEqual(0, _in.Activations + _out.Activations);
            loop.Enable(10f);
            Thread.Sleep(500);
            Assert.AreNotEqual(0, _in.Activations + _out.Activations);
        }
		
        [TestMethod]
        public void TestNoCorrectionAtStablePressure()
        {
            var pressure = new MockHasOneValue(0.5f);
            var loop = new PressureControlLoop(_clock, _pump, pressure, TimeSpan.FromMilliseconds(100));
            loop.Enable(pressure.Get());
            Thread.Sleep(500);
            Assert.AreEqual(0, _in.Activations + _out.Activations);
        }

        [TestMethod]
        public void TestNoCorrectionIfVelocityIsMiniscule()
        {
        }
        [TestMethod]
        public void TestCorrectionPositive()
        {
        }
        [TestMethod]
        public void TestCorrectionNegative()
        {
        }
        [TestMethod]
        public void TestCorrectionMaxesOut()
        {
        }
    }
}
