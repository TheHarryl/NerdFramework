using System;
namespace NerdFramework
{
    public class Vector2
    {
        public readonly double x;
        public readonly double y;

        public static Vector2 Zero = new Vector2();
        public static Vector2 One = new Vector2(1.0, 1.0);

        public Vector2(double x = 0.0, double y = 0.0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 a, Vector2 b)
        {
            this.x = b.x - a.x;
            this.y = b.y - a.y;
        }

        public Vector2(double s)
        {
            this.x = s;
            this.y = s;
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public Vector2 Normalized()
        {
            return this / Magnitude();
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        public static Vector2 operator +(Vector2 a, double b)
        {
            return new Vector2(a.x + b, a.y + b);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(b, a);
        }

        public static Vector2 operator -(Vector2 a, double b)
        {
            return new Vector2(a.x - b, a.y - b);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.x, -a.y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }

        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.x * b, a.y * b);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }

        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.x / b, a.y / b);
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
