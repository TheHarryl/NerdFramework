using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct Color4
    {
        public Color3 color;
        public double distance;

        public Color4(double distance, int r, int g, int b, double alpha = 1.0)
        {
            this.color = new Color3(r, g, b, alpha);
            this.distance = distance;
        }
        public Color4(double distance, Color3 color)
        {
            this.color = color;
            this.distance = distance;
        }
    }
}
