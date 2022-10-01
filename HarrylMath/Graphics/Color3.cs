using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct Color3
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

        public static Color3 Lerp(Color3 a, Color3 b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        public Color3 Grayscale()
        {
            int avg = Average();
            return new Color3(avg, avg, avg, alpha);
        }

        public int Average()
        {
            return (r + g + b) / 3;
        }

        public static Color3 operator +(Color3 a, Color3 b)
        {
            return new Color3(a.r + b.r, a.g + b.g, a.b + b.b, a.alpha + b.alpha);
        }

        public static Color3 operator +(Color3 a, double b)
        {
            return new Color3(a.r + (int)b, a.g + (int)b, a.b + (int)b, a.alpha);
        }

        public static Color3 operator -(Color3 a, Color3 b)
        {
            return new Color3(a.r - b.r, a.g - b.g, a.b - b.b, a.alpha - b.alpha);
        }

        public static Color3 operator -(Color3 a, double b)
        {
            return new Color3(a.r - (int)b, a.g - (int)b, a.b - (int)b, a.alpha);
        }

        public static Color3 operator -(Color3 a)
        {
            return new Color3(255 - a.r, 255 - a.g, 255 - a.b, a.alpha);
        }

        public static Color3 operator *(Color3 a, Color3 b)
        {
            return new Color3(a.r * b.r, a.g * b.g, a.b * b.b, a.alpha * b.alpha);
        }

        public static Color3 operator *(Color3 a, double b)
        {
            return new Color3((int)(a.r * b), (int)(a.g * b), (int)(a.b * b), a.alpha * b);
        }

        public static Color3 operator /(Color3 a, Color3 b)
        {
            return new Color3((int)(a.r / b.r), (int)(a.g / b.g), (int)(a.b / b.b), a.alpha / b.alpha);
        }

        public static Color3 operator /(Color3 a, double b)
        {
            return new Color3((int)(a.r / b), (int)(a.g / b), (int)(a.b / b), a.alpha / b);
        }

        public static bool operator ==(Color3 a, Color3 b)
        {
            return a.r == b.r && a.b == b.b && a.g == b.g;
        }

        public static bool operator !=(Color3 a, Color3 b)
        {
            return a.r != b.r || a.b != b.b || a.g != b.g;
        }
    }
}
