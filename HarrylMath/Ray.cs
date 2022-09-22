using System;
using System.Collections.Generic;
using System.Text;

namespace HarrylMath
{
    class Ray
    {
        public Vector3 p;
        public Vector3 v;

        public Ray(Vector3 position, Vector3 vector)
        {
            /* Ray:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * t >= 0
             * 
             * v ∥ Ray
             * p is a solution of the ray
             */

            p = position;
            v = vector;
        }

        public void RotateX(double radians)
        {
            v = v.RotateX(radians);
        }

        public void RotateY(double radians)
        {
            v = v.RotateY(radians);
        }

        public void RotateZ(double radians)
        {
            v = v.RotateZ(radians);
        }

        public void Rotate(double r1, double r2, double r3)
        {
            v = v.Rotate(r1, r2, r3);
        }

        public static Ray Lerp(Ray a, Ray b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        public override bool Equals(object obj)
        {
            return obj is Ray ray &&
                   p == ray.p &&
                   v == ray.v;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(p, v);
        }

        public static Ray operator +(Ray a, Ray b)
        {
            return new Ray(a.p + b.p, a.v + b.v);
        }

        public static Ray operator -(Ray a, Ray b)
        {
            return new Ray(a.p - b.p, a.v - b.v);
        }

        public static Ray operator *(Ray a, Ray b)
        {
            return new Ray(a.p * b.p, a.v * b.v);
        }

        public static Ray operator *(Ray a, double b)
        {
            return new Ray(a.p * b, a.v * b);
        }

        public static Ray operator /(Ray a, Ray b)
        {
            return new Ray(a.p / b.p, a.v / b.v);
        }

        public static Ray operator /(Ray a, double b)
        {
            return new Ray(a.p / b, a.v / b);
        }

        public static bool operator ==(Ray a, Ray b)
        {
            return a.p == b.p && a.v == b.v;
        }

        public static bool operator !=(Ray a, Ray b)
        {
            return a.p != b.p || a.v != b.v;
        }
    }
}
