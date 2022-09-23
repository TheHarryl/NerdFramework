namespace HarrylMath
{
    class Line3
    {
        public Vector3 p;
        public Vector3 v;

        public Line3(Vector3 position, Vector3 vector)
        {
            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * v ∥ Line
             * p is a solution of the line
             */

            p = position;
            v = vector;
        }

        public bool Meets(Vector3 point)
        {
            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * <x, y, z> = p + vt
             * <x, y, z> - p = vt
             * 
             * vt / |v| = ±v
             * (<x, y, z> - p) / |v| = ±v
             * 
             * IF the vector between the specified point and ANY point on the Line
             *   is ∥ to the Line's vector, the specified point exists on the Line
             */

            Vector3 v0 = v.Normalized();
            Vector3 v1 = (point - p).Normalized();

            return v0 == v1 || v0 == -v1;
        }

        public bool Meets(Line3 line)
        {
            /* 
             */

            return true;
        }

        public Vector3 Intersection(Line3 line)
        {
            /* 
             */

            return Vector3.Zero;
        }

        public double Min(Vector3 point)
        {
            /* d = p0 - p = Path between any point of Plane and specified point
             * v ∥ Line
             * 
             * Minimum path between point and Line is ⊥ to the Line
             * n = Minimum path
             * d, v, and n form a right triangle
             * 
             * IF theta = angle between d and v:
             * |d|sin(theta) = Projection of d onto n
             * 
             * Turns it ⊥ to the Plane, shortest path between point and Plane
             * 
             * |a×b| = |A||B|sin(theta)
             * |d×v| = |d||v|sin(theta)
             * (|d×v|)/|v| = |d|sin(theta)
            */

            return Vector3.Cross(point - p, v).Magnitude() / v.Magnitude();
        }

        public double Min(Line3 line)
        {
            if (v == line.v)
            {

            }

            return 0.0;
        }
    }
}
