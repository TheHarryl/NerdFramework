using System.Collections.Generic;
using System.Linq;

namespace NerdEngine
{
    public class Triangle3Group
    {
        public List<Triangle3> triangles;
        private Vector3 _origin = new Vector3();
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

        public Triangle3Group(List<Triangle3> triangles)
        {
            this.triangles = triangles;
        }

        public static void Move(List<Triangle3> triangles, Vector3 offset)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Move(offset);
            }
        }

        public double Angle(Ray3 ray)
        {
            foreach (Triangle3 triangle in triangles)
            {
                if (triangle.Meets(ray))
                    return Vector3.Angle(triangle.Normal(), ray.v);
            }
            return 0.0;
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
