using System.Collections.Generic;

namespace NerdFramework
{
    public struct Ray3
    {
        public Vector3 p;
        public Vector3 v;

        public Ray3(Vector3 position, Vector3 vector)
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

        public Ray3 March(List<Triangle3> triangles)
        {
            double distance = (triangles[0].a - p).Magnitude();
            foreach (Triangle3 triangle in triangles)
            {
                double d1 = (triangle.a - p).Magnitude();
                double d2 = (triangle.b - p).Magnitude();
                double d3 = (triangle.c - p).Magnitude();
                double min = Math.Min(d1, d2, d3);

                if (min < distance)
                    distance = min;
            }

            return new Ray3(p + v * distance, v);
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

        public void RotateAbout(Vector3 rotand, double radians)
        {
            v = v.RotateAbout(rotand, radians);
        }

        public static Ray3 Lerp(Ray3 a, Ray3 b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        public override string ToString()
        {
            return "{ " + p + ", " + v + " }";
        }

        public override bool Equals(object obj)
        {
            return obj is Ray3 ray &&
                   p == ray.p &&
                   v == ray.v;
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(p, v);
        }

        public static Ray3 operator +(Ray3 a, Ray3 b)
        {
            return new Ray3(a.p + b.p, a.v + b.v);
        }

        public static Ray3 operator -(Ray3 a, Ray3 b)
        {
            return new Ray3(a.p - b.p, a.v - b.v);
        }

        public static Ray3 operator *(Ray3 a, Ray3 b)
        {
            return new Ray3(a.p * b.p, a.v * b.v);
        }

        public static Ray3 operator *(Ray3 a, double b)
        {
            return new Ray3(a.p * b, a.v * b);
        }

        public static Ray3 operator /(Ray3 a, Ray3 b)
        {
            return new Ray3(a.p / b.p, a.v / b.v);
        }

        public static Ray3 operator /(Ray3 a, double b)
        {
            return new Ray3(a.p / b, a.v / b);
        }

        public static bool operator ==(Ray3 a, Ray3 b)
        {
            return a.p == b.p && a.v == b.v;
        }

        public static bool operator !=(Ray3 a, Ray3 b)
        {
            return a.p != b.p || a.v != b.v;
        }
    }
}
