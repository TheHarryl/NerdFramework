namespace HarrylMath
{
    class Plane3
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
            p = (a + b + c) / 3;

            /* A, B, and C are COPLANAR, all are solutions of Plane
             * n ⊥ Plane
             * 
             * n ⊥ A, B, and C
             * n ⊥ AB and AC
             * 
             * c ⊥ a AND c ⊥ b <=> c = a×b
             * n = a×b
             */
            n = Vector3.Cross(new Vector3(a, b), new Vector3(a, c));
        }

        public bool Contains(Vector3 point)
        {
            /* n ⊥ Plane
             * v1 ⊥ v2 <=> v1⋅v2 = 0
             * 
             * p1 = any vector ∥ Plane
             * n ⊥ p1 <=> n⋅p1 = 0
             * 
             * IF n ⊥ Plane AND n ⊥ p1
             *  => p1 is a solution of the plane
             */

            return Vector3.Dot(n, new Vector3(p, point)) == 0.0;
        }

        public Vector3 Intersection(Line3 line)
        {
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

        public double Min(Vector3 point)
        {
            /* P = Shortest path from a point to a Plane
             * P ⊥ Plane
             * THUS: n = P
             * 
             * 
             */

            return Vector3.Dot(n, point - p) / n.Magnitude();
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
             */

            if (Vector3.Dot(n, line.v) == 0.0)
            {

            }

            return 0.0;
        }
        public double Min(Plane3 plane)
        {
            if (n == plane.n)
            {
                /* v⋅v = v1*v1 + v2*v2 + v3*v3 = |v||v|
                 * 
                 */
            }

            return 0.0;
        }
    }
}
