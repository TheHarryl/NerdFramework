namespace HarrylMath._2D
{
    public class Triangle3Group
    {
        protected System.Collections.Generic.List<Triangle3> triangles;
        private Vector3 _origin;
        public Vector3 origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;

                foreach (Triangle3 triangle in triangles)
                {
                    triangle.Move(value - _origin);
                }
                _origin = value;
            }
        }

        public Triangle3Group(System.Collections.Generic.List<Triangle3> triangles)
        {
            this.triangles = triangles;
        }

        public static void Move(System.Collections.Generic.List<Triangle3> triangles, Vector3 offset)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Move(offset);
            }
        }

        public void RotateX(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateX(radians, origin);
            }
        }

        public void RotateY(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateY(radians, origin);
            }
        }

        public void RotateZ(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateZ(radians, origin);
            }
        }

        public void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Rotate(r1, r2, r3, origin);
            }
        }
    }
}
