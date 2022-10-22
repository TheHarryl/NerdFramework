namespace NerdFramework
{
    public unsafe class Quad3
    {
        public Vector3* a;
        public Vector3* b;
        public Vector3* c;
        public Vector3* d;

        public Quad3(Vector3* a, Vector3* b, Vector3* c, Vector3* d)
        {
           /* A ----- D
            * |       |
            * |       |
            * |       |
            * B ----- C
            * 
            * Normal points out of the page
            */
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public Triangle3 GetTriangle1()
        {
            return new Triangle3(a, b, c);
        }

        public Triangle3 GetTriangle2()
        {
            return new Triangle3(a, c, d);
        }
    }
}
