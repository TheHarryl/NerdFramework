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

        public bool Contains(Vector3 point)
        {
            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * (x - p.x)/v.x = (y - p.y)/v.y = (z - p.z)/v.z
             * 
             * 
             */

            return true;
        }

        public double Min(Vector3 point)
        {
            /* P = Shortest path from a point to a Line
             * P ⊥ Line
             * 
             */

            return 0.0;
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
