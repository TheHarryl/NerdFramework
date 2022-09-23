namespace HarrylMath
{
    class Vector3
    {
        public readonly double x;
        public readonly double y;
        public readonly double z;

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

        public Vector3 RotateX(double radians)
        {
            return new Vector3(
                this.x,
                this.y * Math.Cos(radians) - this.z * Math.Sin(radians),
                this.y * Math.Sin(radians) + this.z * Math.Cos(radians)
            );
        }

        public Vector3 RotateY(double radians)
        {
            return new Vector3(
                this.x * Math.Cos(radians) + this.z * Math.Sin(radians),
                this.y,
                -this.x * Math.Sin(radians) + this.z * Math.Cos(radians)
            );
        }

        public Vector3 RotateZ(double radians)
        {
            return new Vector3(
                this.x * Math.Cos(radians) - this.y * Math.Sin(radians),
                this.x * Math.Sin(radians) + this.y * Math.Cos(radians),
                this.z
            );
        }

        public Vector3 Rotate(double r1, double r2, double r3)
        {
            return RotateX(r1).RotateY(r2).RotateZ(r3);
        }

        public static Vector3 Angle3(Vector3 a, Vector3 b)
        {
            // Create projections of vectors a and b onto the x-axis
            Vector3 Ax = new Vector3(0.0, a.y, a.z);
            Vector3 Bx = new Vector3(0.0, b.y, b.z);

            // Calculate 2D x-angle between projections
            double xRadians = Angle(Ax, Bx);

            // Create angle-adjusted a vector
            Vector3 a2 = a.RotateX(xRadians);

            // Create projections of vectors a2 and b onto the y-axis
            Vector3 Ay = new Vector3(a2.x, 0.0, a2.z);
            Vector3 By = new Vector3(b.x, 0.0, b.z);

            // Calculate 2D y-angle between projections
            double yRadians = Angle(Ay, By);

            // Create angle-adjusted a vector
            Vector3 a3 = a2.RotateY(yRadians);

            // Create projections of vectors a3 and b onto the z-axis
            Vector3 Az = new Vector3(a3.x, a3.y, 0.0);
            Vector3 Bz = new Vector3(b.x, b.y, 0.0);

            // Calculate 2D z-angle between projections
            double zRadians = Angle(Az, Bz);

            return new Vector3(xRadians, yRadians, zRadians);
        }

        public static double Angle(Vector3 a, Vector3 b)
        {
            /* a⋅b = |a||b|cos(theta)
             * 
             * cos(theta) = (a⋅b)/(|a||b|)
             * theta = acos((a⋅b)/(|a||b|))
             */

            return Math.Acos(Dot(a, b) / (a.Magnitude() * b.Magnitude()));
        }

        public static double Dot(Vector3 a, Vector3 b)
        {
            /* a⋅b = (a1*b1) + (a2*b2) + (a3*b3)
             * 
             * a⋅b = 0 <=> a ⊥ b
             * 
             * a⋅b = |a||b|cos(theta)
             * 
             * a⋅b = b⋅a
             */

            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            /*       | e1 e2 e3 |   | a2 a3 |     | a1 a3 |     | a1 a2 |
             * a×b = | a1 a2 a3 | = | b2 b3 |e1 - | b1 b3 |e2 + | b1 b2 |e3
             *       | b1 b2 b3 |
             * 
             *  = <(a2*b3) - (a3*b2), -[(a1*b3) - (a3*b1)], (a1*b2) - (a2*b1)>
             * 
             * c = a×b <=> c ⊥ a AND c ⊥ b
             * 
             * |a×b| = |A||B|sin(theta) = Area of Parallelogram
             * 
             * a×b = -b×a
             */

            return new Vector3((a.y * b.z) - (a.z * b.y), -(a.x * b.z) + (a.z * b.x), (a.x * b.y) - (a.y * b.x));
        }

        public static double Triple(Vector3 a, Vector3 b, Vector3 c)
        {
            /*           | a1 a2 a3 |   | b2 b3 |     | b1 b3 |     | b1 b2 |
             * a⋅(b×c) = | b1 b2 b3 | = | c2 c3 |a1 - | c1 c3 |a2 + | c1 c2 |a3
             *           | c1 c2 c3 |
             * 
             *  = [(b2*c3) - (b3*c2)]a1 -[(b1*c3) - (b3*c1)]a2 + [(b1*c2) - (b2*c1)]a3
             * 
             * |a⋅(b×c)| = Volume of Parallelepiped
             * 
             * a⋅(b×c) = c⋅(a×b) = b⋅(c×a)
             * a⋅(b×c) = -b⋅(a×c) = -c⋅(b×a)
             */

            return ((b.y * c.z) - (b.z * c.y))*a.x - ((b.x * c.z) - (b.z * c.x))*a.y + ((b.x * c.y) - (b.y * c.x))*a.z;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, double alpha)
        {
            return a * (1 - alpha) + b * alpha;
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
            return System.HashCode.Combine(x, y, z);
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

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.x, -a.y, -a.z);
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
