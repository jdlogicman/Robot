using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simulation;
using System.Collections.Generic;
using System.Linq;
namespace SimulationTest
{
    [TestClass]
    public class VelocitySimulationTest
    {
        [TestMethod]
        public void VerifyAccelerationPositiveMassNoDrag()
        {
            float newVelocity = VelocitySimulation.CalculateNewVelocity(0, volume:0, areaFacingDirectionOfTravel: 0);
            Assert.IsTrue(Math.Abs(newVelocity - VelocitySimulation.GRAVITY) < 0.01);
            newVelocity = VelocitySimulation.CalculateNewVelocity(-1 * VelocitySimulation.GRAVITY, volume: 0, areaFacingDirectionOfTravel: 0);
            Assert.IsTrue(Math.Abs(newVelocity) < 0.01);
 

        }

        [TestMethod]
        public void VerifyDragForceDirection()
        {
            Assert.IsTrue(VelocitySimulation.CalculateDragForce(1, 1) < 0);
            Assert.IsTrue(VelocitySimulation.CalculateDragForce(-1, 1) > 0);
        }

        [TestMethod]
        public void VerifyTerminalVelocityReached()
        {
            List<float> velocities = new List<float> { 0 };
            const int MAX_ITER = 10000;
            while (velocities.Count < MAX_ITER)
                velocities.Add(VelocitySimulation.CalculateNewVelocity(velocities.Last()));
            Assert.IsTrue(Math.Abs(velocities.Skip(MAX_ITER - 100).Take(100).Average() - velocities.Last()) < 0.001); 

        }
        
    }
}
