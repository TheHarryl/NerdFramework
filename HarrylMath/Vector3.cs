using System;

namespace HarrylMath
{
    class Vector3
    {
        public double x;
        public double y;
        public double z;

        public static Vector3 Zero = new Vector3();
        public static Vector3 One = new Vector3(1.0, 1.0, 1.0);
        public static Vector3 xAxis = new Vector3(1.0, 0.0, 0.0);
        public static Vector3 yAxis = new Vector3(0.0, 1.0, 0.0);
        public static Vector3 zAxis = new Vector3(0.0, 0.0, 1.0);

        public Vector3(double x = 0.0, double y = 0.0, double z = 0.0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 a, Vector3 b)
        {
            this.x = b.x - a.x;
            this.y = b.y - a.y;
            this.z = b.z - a.z;
        }

        public Vector3(double s)
        {
            this.x = s;
            this.y = s;
            this.z = s;
        }

        public double Magnitude()
        {
            return Math.Sqrt(x*x + y*y + z*z);
        }

        public Vector3 Normalized()
        {
            return this / Magnitude();
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3((a.y * b.z) - (a.z * b.y), -(a.x * b.z) + (a.z * b.x), (a.x * b.y) - (a.y * b.x));
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
        }

        public static double Angle(Vector3 a, Vector3 b)
        {
            return Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3 vector &&
                   x == vector.x &&
                   y == vector.y &&
                   z == vector.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator +(Vector3 a, double b)
        {
            return new Vector3(a.x + b, a.y + b, a.z + b);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(b, a);
        }

        public static Vector3 operator -(Vector3 a, double b)
        {
            return new Vector3(a.x - b, a.y - b, a.z - b);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }
    }
}
