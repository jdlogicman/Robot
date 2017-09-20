using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogic;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class TestControlLoop
    {
        MockDigitalPort _in;
        MockDigitalPort _out;
        Pump _pump;
        ClockMock _clock;
        ControlParameters _controlArgs;
        [TestInitialize]
        public void Init()
        {
            _clock = new ClockMock(50);
            _in = new MockDigitalPort(_clock);
            _out = new MockDigitalPort(_clock);
            _pump = new Pump(_clock, new MockLog(), _out, _in);
            _controlArgs = new ControlParameters { PressureP = -0.3f, PressureD = -0.3f, VelocityP = -9.3f, VelocityD = -7.9f };

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
            
            var loop = new PressureControlLoop(_clock, _pump, pressure, _controlArgs, TimeSpan.FromMilliseconds(100));
            _clock.Elapse(500);
            Assert.AreEqual(0, _in.Activations + _out.Activations);
            loop.Enable(10f);
            _clock.Elapse(500);
            Assert.AreNotEqual(0, _in.Activations + _out.Activations);
        }
		
        [TestMethod]
        public void TestNoCorrectionAtStablePressure()
        {
            var pressure = new MockHasOneValue(0.5f);
            var loop = new PressureControlLoop(_clock, _pump, pressure, _controlArgs, TimeSpan.FromMilliseconds(100));
            loop.Enable(pressure.Get());
            _clock.Elapse(500);
            Assert.AreEqual(0, _in.Activations + _out.Activations);
        }

        [TestMethod]
        public void TestCorrectionPositive()
        {
            var pressure = new MockHasOneValue(0.5f);
            var loop = new PressureControlLoop(_clock, _pump, pressure, _controlArgs, TimeSpan.FromMilliseconds(100));
            loop.Enable(0.6f);
            _clock.Elapse(500);
            Assert.AreNotEqual(0, _in.Activations);
            Assert.AreEqual(0, _out.Activations);
        }
        [TestMethod]
        public void TestCorrectionNegative()
        {
            var pressure = new MockHasOneValue(0.5f);
            var loop = new PressureControlLoop(_clock, _pump, pressure, _controlArgs, TimeSpan.FromMilliseconds(100));
            loop.Enable(0.4f);
            _clock.Elapse(500);
            Assert.AreEqual(0, _in.Activations);
            Assert.AreNotEqual(0, _out.Activations);
        }
    }
}
