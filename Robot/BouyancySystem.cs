using ControlLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    public class BouyancySystem : ObserverBase
    {
        public const double MAX_LOAD_KG = 5;

        public double WaterMass { get; private set; }

        
        public override void OnNext(double amount)
        {
            WaterMass = Math.Min(Math.Max(0, WaterMass + amount), MAX_LOAD_KG);
            base.OnNext(WaterMass);
        }
    }
}
