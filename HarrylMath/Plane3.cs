namespace HarrylMath
{
    class Plane3
    {
        public Vector3 p;
        public Vector3 n;

        public Plane3(Vector3 position, Vector3 normal)
        {
            p = position;
            n = normal;
        }

        public Plane3(Vector3 a, Vector3 b, Vector3 c)
        {
            p = (a + b + c) / 3;
            n = Vector3.Cross(new Vector3(a, b), new Vector3(a, c));
        }

        public bool IsInside(Vector3 point)
        {
            return Vector3.Dot(n, new Vector3(p, point)) == 0.0;
        }
    }
}
