namespace HarrylMath
{
    class Triangle3
    {
        Vector3 a;
        Vector3 b;
        Vector3 c;

        public Triangle3(Vector3 a, Vector3 b, Vector3 c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Vector3 Normal()
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

            return Vector3.Cross(new Vector3(a, b), new Vector3(a, c));
        }

        public double Area()
        {
            /* |a×b| = |A||B|sin(theta) = Area of Parallelogram
             * Parallelogram is two triangles
             */

            return Vector3.Cross(b - a, c - a).Magnitude() / 2.0;
        }
    }
}
