using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    public class Triangle3Group
    {
        public List<Triangle3> triangles;
        private Vector3 _origin = Vector3.Zero;
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
        private Vector3 _scale = Vector3.One;
        public Vector3 scale
        {
            get => _scale;
            set
            {
                if (_scale == value) return;

                foreach (Triangle3 triangle in triangles)
                {
                    triangle.Scale(value / _scale, _origin);
                }
                _scale = value;
            }
        }

        public Triangle3Group(List<Triangle3> triangles)
        {
            this.triangles = triangles;
        }

        public static Triangle3Group FromFile(string fileLocation)
        {
            string[] lines = System.IO.File.ReadAllLines(fileLocation);
            List<Triangle3> triangles = new List<Triangle3>();

            foreach (string line in lines)
            {

            }

            Triangle3Group group = new Triangle3Group(triangles);



            return group;
        }

        public static void Move(List<Triangle3> triangles, Vector3 offset)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Move(offset);
            }
        }

        public static void Scale(List<Triangle3> triangles, Vector3 scale, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Scale(scale, origin);
            }
        }

        public bool Meets(Ray3 ray)
        {
            foreach (Triangle3 triangle in triangles)
            {
                if (triangle.Meets(ray))
                    return true;
            }
            return false;
        }

        public void RotateX(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateX(radians, origin + _origin);
            }
        }

        public void RotateY(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateY(radians, origin + _origin);
            }
        }

        public void RotateZ(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateZ(radians, origin + _origin);
            }
        }

        public void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Rotate(r1, r2, r3, origin + _origin);
            }
        }

        public static Triangle3Group operator +(Triangle3Group a, Triangle3 b)
        {
            a.triangles.Add(b);
            return a;
        }

        public static Triangle3Group operator +(Triangle3Group a, Triangle3Group b)
        {
            a.triangles = a.triangles.Concat(b.triangles).ToList();
            return a;
        }
    }
}
