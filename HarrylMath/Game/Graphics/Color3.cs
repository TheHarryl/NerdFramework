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

        public static Color3 None = new Color3(0, 0, 0, 0.0);
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

        public static Color3 FromVector3(Vector3 v)
        {
            return new Color3((int)(v.x * 255.0), (int)(v.y * 255.0), (int)(v.z * 255.0));
        }

        public static Color3 Lerp(Color3 a, Color3 b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        public static Color3 Average(Color3 color1, Color3 color2)
        {
            return (color1 + color2) / 2.0;
        }

        public static Color3 Average(params Color3[] colors)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            double alpha = 0.0;

            for (int i = 0; i < colors.Length; i++)
            {
                r += colors[i].r;
                g += colors[i].g;
                b += colors[i].b;
                alpha += colors[i].alpha;
            }

            return new Color3(r / colors.Length, g / colors.Length, b / colors.Length, alpha / colors.Length);
        }

        public static Color3 Flatten(Color3 bottomLayer, Color3 topLayer)
        {
            return new Color3(
                (int)(topLayer.r * topLayer.alpha + bottomLayer.r * (1.0 - topLayer.alpha)),
                (int)(topLayer.g * topLayer.alpha + bottomLayer.g * (1.0 - topLayer.alpha)),
                (int)(topLayer.b * topLayer.alpha + bottomLayer.b * (1.0 - topLayer.alpha)),
                topLayer.alpha * topLayer.alpha + bottomLayer.alpha * (1.0 - topLayer.alpha)
            );
        }

        /*public static Color3 Flatten(params Color3[] layers)
        {
            double r = layers[layers.Length - 1].r;
            double g = layers[layers.Length - 1].g;
            double b = layers[layers.Length - 1].b;
            double alpha = layers[layers.Length - 1].alpha;

            for (int i = layers.Length - 2; i >= 0 && alpha < 1.0; i--)
            {
                r += layers[i].r * (1.0 - alpha);
                g += layers[i].g * (1.0 - alpha);
                b += layers[i].b * (1.0 - alpha);
                alpha += layers[i].alpha * (1.0 - alpha);
            }
            return new Color3((int)r, (int)g, (int)b, alpha);
        }*/

        public static Color3 Flatten(params Color4[] layers)
        {
            double r = layers[layers.Length - 1].color.r;
            double g = layers[layers.Length - 1].color.g;
            double b = layers[layers.Length - 1].color.b;
            double alpha = layers[layers.Length - 1].color.alpha;

            for (int i = layers.Length - 2; i >= 0 && alpha < 1.0; i--)
            {
                r += layers[i].color.r * (1.0 - alpha) * layers[i].color.alpha;
                g += layers[i].color.g * (1.0 - alpha) * layers[i].color.alpha;
                b += layers[i].color.b * (1.0 - alpha) * layers[i].color.alpha;
                alpha += layers[i].color.alpha * (1.0 - alpha);
            }
            return new Color3((int)r, (int)g, (int)b, alpha);
        }

        public static Color3 FromParameterization3(double t, double s, Color3 a, Color3 b, Color3 c)
        {
            double u = 1.0 - t - s;
            return new Color3(
                (int)(a.r*u + b.r*t + c.r*s),
                (int)(a.g*u + b.g*t + c.g*s),
                (int)(a.b*u + b.b*t + c.b*s),
                a.alpha*u + b.alpha*t + c.alpha*s
            );
        }

        public static int ValueFromParameterization3(double t, double s, Color3 a, Color3 b, Color3 c)
        {
            return (int)((a.r + a.g + a.b)*(1.0 - t - s) + (b.r + b.g + b.b)*t + (c.r + c.g + c.b)*s) / 3;
        }

        public double Luma()
        {
            return Math.Sqrt(Vector3.Dot(new Vector3(r / 255.0, g / 255.0, b / 255.0), new Vector3(0.299, 0.587, 0.114)));
        }

        public Color3 WithAlpha(double alpha)
        {
            return new Color3(r, g, b, this.alpha * alpha);
        }

        public Color3 Baked()
        {
            return new Color3((int)(r * alpha), (int)(g * alpha), (int)(b * alpha));
        }

        public Color3 Grayscaled()
        {
            int avg = Value();
            return new Color3(avg, avg, avg, alpha);
        }

        public int Value()
        {
            return (r + g + b) / 3;
        }

        public Color3 Opaque()
        {
            return new Color3(r, g, b);
        }

        public Color3 Invisible()
        {
            return new Color3(r, g, b, 0.0);
        }

        public override string ToString()
        {
            return "(" + r + ", " + g + ", " + b + ": " + alpha + ")";
        }

        public override bool Equals(object obj)
        {
            return obj is Color3 color &&
                   r == color.r &&
                   g == color.g &&
                   b == color.b &&
                   alpha == color.alpha;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(r, g, b, alpha);
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
            return new Color3((a.r * b.r) / 255, (a.g * b.g) / 255, (a.b * b.b) / 255, a.alpha * b.alpha);
        }

        public static Color3 operator *(Color3 a, double b)
        {
            return new Color3((int)(a.r * b), (int)(a.g * b), (int)(a.b * b), a.alpha * b);
        }

        public static Color3 operator /(Color3 a, Color3 b)
        {
            return new Color3((a.r / b.r) * 255, (a.g / b.g) * 255, (a.b / b.b) * 255, a.alpha / b.alpha);
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
