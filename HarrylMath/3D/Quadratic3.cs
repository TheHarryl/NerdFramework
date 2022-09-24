using System;
using System.Collections.Generic;
using System.Text;
namespace HarrylMath
{
    public class Quadratic3
    {
        readonly double? a;
        readonly double? b;
        readonly double? c;
        readonly double? d;
        readonly double? e;
        readonly double? f;
        readonly double? g;
        readonly double? h;
        readonly double? i;
        readonly double? j;

        public Quadratic3(double? a, double? b, double? c, double? d, double? e, double? f, double? g, double? h, double? i, double? j)
        {
            // Ax^2 + By^2 + Cz^2 + Dxy + Exz + Fyz + Gx + Hy + Iz + J

            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
            this.g = g;
            this.h = h;
            this.i = i;
            this.j = j;
        }

        public double Equals(double x, double y, double z)
        {
            return (double)a*x*x + (double)b*y*y + (double)c*z*z + (double)d*x*y + (double)e*x*z + (double)f*y*z + (double)g*x + (double)h*y + (double)i*z + (double)j;
        }
    }
}
