using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Color3
    {
        public int r;
        public int g;
        public int b;
        public double alpha;

        public static Color3 White = new Color3(255, 255, 255);
        public static Color3 Black = new Color3(0, 0, 0);
        public static Color3 Red = new Color3(255, 0, 0);
        public static Color3 Green = new Color3(0, 255, 0);
        public static Color3 Blue = new Color3(0, 0, 255);

        public Color3(int r, int g, int b, double alpha = 1.0)
        {
            this.r = r;
            this.b = b;
            this.g = g;
            this.alpha = alpha;
        }
    }
}
