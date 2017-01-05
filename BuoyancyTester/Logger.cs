using System;
using Microsoft.SPOT;
using ControlLogicMF;

namespace BuoyancyTester
{
    class Logger : ILogger
    {
        public void Log(string msg)
        {
            Debug.Print(msg);
        }
    }
}
