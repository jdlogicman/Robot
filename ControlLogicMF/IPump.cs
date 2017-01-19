using System;

namespace ControlLogic
{
    public interface IPump
    {
        void PumpOut(TimeSpan duration);
        void PumpIn(TimeSpan duration);
        void Stop();

    }
}
