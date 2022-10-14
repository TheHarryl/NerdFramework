using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct NumberRange
    {
        public double min;
        public double max;

        public NumberRange(double min, double max)
        {
            this.min = min;
            this.max = max;
        }
    }
}
