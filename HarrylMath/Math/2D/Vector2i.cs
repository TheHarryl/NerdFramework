using System;
namespace NerdFramework
{
    public struct Vector2i
    {
        public int x;
        public int y;

        public static Vector2i Zero = new Vector2i();
        public static Vector2i One = new Vector2i(1, 1);

        public Vector2i(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2i(Vector2i a, Vector2i b)
        {
            this.x = b.x - a.x;
            this.y = b.y - a.y;
        }

        public Vector2i(int s)
        {
            this.x = s;
            this.y = s;
        }

        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public Vector3 AsVector3()
        {
            return new Vector3(x, y, 0.0);
        }

        public static Vector2i FromParameterization3(double t, double s, Vector2i a, Vector2i b, Vector2i c)
        {
            double u = 1.0 - t - s;
            return new Vector2i((int)(a.x*u + b.x*t + c.x*s), (int)(a.y*u + b.y*t + c.y*s));
        }

        public static Vector2i operator +(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.x + b.x, a.y + b.y);
        }

        public static Vector2i operator +(Vector2i a, int b)
        {
            return new Vector2i(a.x + b, a.y + b);
        }

        public static Vector2i operator -(Vector2i a, Vector2i b)
        {
            return new Vector2i(b, a);
        }

        public static Vector2i operator -(Vector2i a, int b)
        {
            return new Vector2i(a.x - b, a.y - b);
        }

        public static Vector2i operator -(Vector2i a)
        {
            return new Vector2i(-a.x, -a.y);
        }

        public static Vector2i operator *(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.x * b.x, a.y * b.y);
        }

        public static Vector2i operator *(Vector2i a, int b)
        {
            return new Vector2i(a.x * b, a.y * b);
        }

        public static Vector2i operator /(Vector2i a, Vector2i b)
        {
            return new Vector2i(a.x / b.x, a.y / b.y);
        }

        public static Vector2i operator /(Vector2i a, int b)
        {
            return new Vector2i(a.x / b, a.y / b);
        }

        public static bool operator ==(Vector2i a, Vector2i b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2i a, Vector2i b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public override string ToString()
        {
            return "(" + x + ", " + y + ")";
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2i vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
    }
}
