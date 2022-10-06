namespace NerdFramework
{
    public struct Plane3
    {
        public Vector3 p;
        public Vector3 n;

        public Plane3(Vector3 position, Vector3 normal)
        {
            /* Plane:
             * n.x(x - p.x) + n.y(y - p.y) + n.z(z - p.z) = 0
             * 
             * n ⊥ Plane
             * p is a solution of the plane
             */

            p = position;
            n = normal;
        }

        public Plane3(Vector3 a, Vector3 b, Vector3 c)
        {
            /* A, B, and C are COPLANAR, all are solutions of Plane
             * n ⊥ Plane
             * 
             * n ⊥ A, B, and C
             * n ⊥ AB and AC
             * 
             * c ⊥ a AND c ⊥ b <=> c = a×b
             * n = a×b
             */

            p = a;
            n = Vector3.Cross(new Vector3(a, b), new Vector3(a, c));
        }

        public bool Meets(Vector3 point)
        {
            /* n ⊥ Plane
             * v1 ⊥ v2 <=> v1⋅v2 = 0
             * 
             * p1 = any vector ∥ Plane
             * IF n ⊥ Plane AND n ⊥ p1
             *  => p1 is a solution of the plane
             * 
             * n ⊥ p1 <=> n⋅p1 = 0
             */

            return Vector3.Dot(n, new Vector3(p, point)) == 0.0;
        }

        public bool Meets(Line3 Line)
        {
            return Min(Line) == 0.0;
        }

        public bool Meets(Plane3 Plane)
        {
            return Min(Plane) == 0.0;
        }

        public Vector3 Intersection(Line3 line)
        {
            // In order to maximize performance, this code assumes you will only
            //   use this function if you already know the plane and line intersect at a point.
            // This condition can be checked for by checking if the plane and line are not parallel.
            // A line is parallel to a line if its directional vector is perpendicular to the plane's normal.

            /* Plane:
             * n.x(x - q.x) + n.y(y - q.y) + n.z(z - q.z) = 0
             * 
             * Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * Intersection:
             * n.x([p.x + v.xt] - q.x) + n.y([p.y + v.yt] - q.y) + n.z([p.z + v.zt] - q.z) = 0
             * n⋅p + n⋅vt - n⋅q = 0
             * (n⋅v)t = n⋅(q - p)
             * t = [n⋅(q - p)]/(n⋅v)
             */

            double t = Vector3.Dot(n, p - line.p) / Vector3.Dot(n, line.v);
            return line.p + (line.v * t);
        }

        public Line3 Intersection(Plane3 Plane)
        {
            // In order to maximize performance, this code assumes you will only
            //   use this function if you already know the two planes intersect at a Line.
            // This condition can be checked for by checking if the planes are not parallel.

            /* Planes:
             * n0.x(x - p.x) + n0.y(y - p.y) + n0.z(z - p.z) = 0
             * n1.x(x - q.x) + n1.y(y - q.y) + n1.z(z - q.z) = 0
             * 
             * c = a×b <=> c ⊥ a AND c ⊥ b
             * 
             * v = Vector of line of intersection
             * v ⊥ n0
             * v ⊥ n1
             * v ⊥ n0, n1 <=> v = n0×n1
             * 
             * Point of line of intersection:
             * p = ANY point solution of plane0
             * q = ANY point solution of plane1
             * n0.x(x - p.x) + n0.y(y - p.y) + n0.z(z - p.z) = n1.x(x - q.x) + n1.y(y - q.y) + n1.z(z - q.z)
             * n0⋅<x,y,z> - n0⋅p = n1⋅<x,y,z> - n1⋅q
             * (n0-n1)⋅<x,y,z> = n0⋅p - n1⋅q
             * n0.x*x - n1.x*x + n0.y*y - n1.y*y + n0.z*z - n1.z*z = n0.x*p.x - n1.x*q.x + n0.y*p.y - n1.y*q.y + n0.z*p.z - n1.z*q.z
             * (n0 - n1).x*x + (n0 - n1).y*y + (n0 - n1).z*z = n0.x*p.x - n1.x*q.x + n0.y*p.y - n1.y*q.y + n0.z*p.z - n1.z*q.z
             * (n0 - n1).x*x = n0.x*p.x - n1.x*q.x
             * (n0 - n1).y*y = n0.y*p.y - n1.y*q.y
             * (n0 - n1).z*z = n0.z*p.z - n1.z*q.z
             * 
             * A point of intersection exists where:
             * x = (n0.x*p.x - n1.x*q.x) / (n0 - n1).x
             * y = (n0.y*p.y - n1.y*q.y) / (n0 - n1).y
             * z = (n0.z*p.z - n1.z*q.z) / (n0 - n1).z
             * <x, y, z> = (n0*p - n1*q) / (n0 - n1)
             */

            return new Line3((n * p - Plane.n * Plane.p) / (n - Plane.n), Vector3.Cross(n, Plane.n));
        }

        public double Min(Vector3 point)
        {
            /* d = p0 - p = Path between any point of Plane and specified point
             * n ⊥ Plane
             * 
             * Minimum path between point and Plane is ⊥ to the Plane
             * d, n, and the line form a right triangle
             * 
             * IF theta = angle between d and n:
             * |d|cos(theta) = Projection of d onto n
             * 
             * Turns it ⊥ to the Plane, shortest path between point and Plane
             * 
             * a⋅b = |a||b|cos(theta)
             * d⋅n = |d||n|cos(theta)
             * (d⋅n)/|n| = |d|cos(theta)
             */

            return Math.Abs(Vector3.Dot(n, point - p) / n.Magnitude());
        }

        public double Min(Line3 line)
        {
            /* v ∥ Line
             * n ⊥ Plane
             * 
             * n ⊥ v && n ⊥ Plane <=> v ∥ Plane
             * n ⊥ v <=> v⋅n = 0
             * 
             * THUS distance between Plane and Line is CONSTANT along Line
             * 
             * THEREFORE any point within the Line yields the minimum distance
             */

            if (Vector3.Dot(n, line.v) == 0.0)
            {
                // Returns the minimum using the formula between the Plane and a point, using a point of the Line
                return Min(line.p);
            }

            /* v ∥ Line
             * n ⊥ Plane
             * 
             * IF n !⊥ v <=> v⋅n != 0:
             * 
             * THUS Plane and Line meet somewhere in space, and minimum distance is ZERO
             */

            return 0.0;
        }

        public double Min(Plane3 plane)
        {
            /* n0 ⊥ Plane0
             * n1 ⊥ Plane1
             * 
             * IF n0 = n1 * s:
             * Plane0 ∥ Plane1
             * 
             * THUS distance between Planes are CONSTANT along all directions
             * 
             * THEREFORE any point within the Plane yields the minimum distance
             */

            //Vector3 n0 = n.Normalized();
            //Vector3 n1 = plane.n.Normalized();
            //if (n0 == n1 || n0 == -n1)
            if (Vector3.Parallel(n, plane.n))
            {
                // Returns the minimum using the formula between the Plane and a point, using a point of the other Plane
                return Min(plane.p);
            }

            /* n0 ⊥ Plane0
             * n1 ⊥ Plane1
             * 
             * IF n0 != n1 * s:
             * Plane0 !∥ Plane1
             * 
             * THUS Planes meet somewhere in space, and minimum distance is ZERO
             */

            return 0.0;
        }
    }
}
