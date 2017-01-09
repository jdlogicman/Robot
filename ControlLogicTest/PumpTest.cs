using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControlLogicMF;
using System.Threading;

namespace ControlLogicTest
{
    [TestClass]
    public class PumpTest
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
        public void TestTimeIn()
        {
			_pump.PumpIn(TimeSpan.FromMilliseconds(150));
			Assert.IsTrue(_in.State);
			Assert.IsFalse(_out.State);
			Thread.Sleep(250);
			Assert.IsFalse(_in.State);
			Assert.IsFalse(_out.State);
			Assert.IsTrue(Math.Abs(_in.Duration.TotalMilliseconds - 150) < 50);
        }
		[TestMethod]
        public void TestTimeOut()
        {
            _pump.PumpOut(TimeSpan.FromMilliseconds(150));
            Assert.IsFalse(_in.State);
            Assert.IsTrue(_out.State);
            Thread.Sleep(250);
            Assert.IsFalse(_in.State);
            Assert.IsFalse(_out.State);
            Assert.IsTrue(Math.Abs(_out.Duration.TotalMilliseconds - 150) < 50);
        }
		[TestMethod]
        public void TestChangeDirectionShutsOffOther()
        {
            _pump.PumpIn(TimeSpan.FromMilliseconds(1500));
            Assert.IsTrue(_in.State);
            _pump.PumpOut(TimeSpan.FromMilliseconds(1500));
            Assert.IsFalse(_in.State);

            _pump.PumpOut(TimeSpan.FromMilliseconds(1500));
            Assert.IsTrue(_out.State);
            _pump.PumpIn(TimeSpan.FromMilliseconds(1500));
            Assert.IsFalse(_out.State);
        }
		[TestMethod]
        public void TestInitialStates()
		{
            Assert.IsFalse(_out.State);
            Assert.IsFalse(_in.State);
		}
    }
}
