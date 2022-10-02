using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class UDim
    {
        double scale;
        double offset;

        public UDim()
        {
            this.scale = 0.0;
            this.offset = 0.0;
        }

        public UDim(double scale, double offset)
        {
            this.scale = scale;
            this.offset = offset;
        }

        public double Absolute(int max)
        {
            return max * scale + offset;
        }
    }
}
