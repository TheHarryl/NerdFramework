using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    public class Triangle3Collection
    {
        public List<Triangle3> triangles;

        private Vector3 _origin = Vector3.Zero;
        private Vector3 _scale = Vector3.One;

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

        public Triangle3Collection(List<Triangle3> triangles)
        {
            this.triangles = triangles;
        }

        public Triangle3Collection Clone()
        {
            Vector3 oldOrigin = this.origin;
            Vector3 oldScale = this.scale;
            this.origin = Vector3.Zero;
            this.scale = Vector3.One;
            List<Triangle3> clonedTriangles = new List<Triangle3>();
            foreach (Triangle3 triangle in triangles)
            {
                clonedTriangles.Add(new Triangle3(triangle.a, triangle.b, triangle.c));
            }
            this.origin = oldOrigin;
            this.scale = oldScale;

            Triangle3Collection clone = new Triangle3Collection(clonedTriangles);
            clone.origin = oldOrigin;
            clone.scale = oldScale;

            return clone;
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
            //_origin = (_origin - origin).RotateX(radians) + origin;
        }

        public void RotateY(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateY(radians, origin + _origin);
            }
            //_origin = (_origin - origin).RotateY(radians) + origin;
        }

        public void RotateZ(double radians, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.RotateZ(radians, origin + _origin);
            }
            //_origin = (_origin - origin).RotateZ(radians) + origin;
        }

        public void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            foreach (Triangle3 triangle in triangles)
            {
                triangle.Rotate(r1, r2, r3, origin + _origin);
            }
            //_origin = (_origin - origin).Rotate(r1, r2, r3) + origin;
        }

        public static Triangle3Collection operator +(Triangle3Collection a, Triangle3 b)
        {
            return new Triangle3Collection(a.triangles.Concat(new List<Triangle3>(){ b }).ToList());
        }

        public static Triangle3Collection operator +(Triangle3Collection a, Triangle3Collection b)
        {
            return new Triangle3Collection(a.triangles.Concat(b.triangles).ToList());
        }
    }
}
