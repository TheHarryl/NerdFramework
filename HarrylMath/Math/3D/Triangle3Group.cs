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

        public Triangle3Group Clone()
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

            Triangle3Group clone = new Triangle3Group(clonedTriangles);
            clone.origin = oldOrigin;
            clone.scale = oldScale;

            return clone;
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

        public static Triangle3Group FromCube(Vector3 origin, double sideLength)
        {
            double v = sideLength / 2.0;
            Vector3 a = new Vector3(v, v, -v);
            Vector3 b = new Vector3(v, v, v);
            Vector3 c = new Vector3(-v, v, -v);
            Vector3 d = new Vector3(-v, v, v);
            Vector3 e = new Vector3(v, -v, -v);
            Vector3 f = new Vector3(v, -v, v);
            Vector3 g = new Vector3(-v, -v, -v);
            Vector3 h = new Vector3(-v, -v, v);

            Triangle3Group cube = new Triangle3Group(new List<Triangle3>()
            {
                new Triangle3(b, d, f),
                new Triangle3(h, f, d),
                new Triangle3(b, a, d),
                new Triangle3(c, d, a),
                new Triangle3(d, c, h),
                new Triangle3(g, h, c),
                new Triangle3(h, g, f),
                new Triangle3(e, f, g),
                new Triangle3(f, e, b),
                new Triangle3(a, b, e),
                new Triangle3(a, e, c),
                new Triangle3(g, c, e),
            });

            cube.origin = origin;
            return cube;
        }

        public static Triangle3Group FromIcophere(Vector3 origin, double radius, int iterations = 2)
        {
            double goldenRatio = (1.0 + Math.Sqrt(5)) / 2.0;
            Vector3 a = new Vector3(goldenRatio, 1.0, 0.0).Normalized();
            Vector3 b = new Vector3(goldenRatio, -1.0, 0.0).Normalized();
            Vector3 c = new Vector3(-goldenRatio, -1.0, 0.0).Normalized();
            Vector3 d = new Vector3(-goldenRatio, 1.0, 0.0).Normalized();
            Vector3 e = new Vector3(1.0, 0.0, goldenRatio).Normalized();
            Vector3 f = new Vector3(-1.0, 0.0, goldenRatio).Normalized();
            Vector3 g = new Vector3(-1.0, 0.0, -goldenRatio).Normalized();
            Vector3 h = new Vector3(1.0, 0.0, -goldenRatio).Normalized();
            Vector3 i = new Vector3(0.0, goldenRatio, 1.0).Normalized();
            Vector3 j = new Vector3(0.0, goldenRatio, -1.0).Normalized();
            Vector3 k = new Vector3(0.0, -goldenRatio, -1.0).Normalized();
            Vector3 l = new Vector3(0.0, -goldenRatio, 1.0).Normalized();

            List<Triangle3> icosahedron = new List<Triangle3>()
            {
                new Triangle3(e, l, b),
                new Triangle3(e, b, a),
                new Triangle3(e, a, i),
                new Triangle3(f, i, d),
                new Triangle3(f, d, c),
                new Triangle3(f, c, l),
                new Triangle3(e, i, f),
                new Triangle3(f, l, e),
                new Triangle3(b, k, h),
                new Triangle3(b, h, a),
                new Triangle3(a, h, j),
                new Triangle3(d, j, g),
                new Triangle3(d, g, c),
                new Triangle3(c, g, k),
                new Triangle3(g, j, h),
                new Triangle3(g, h, k),
                new Triangle3(i, a, j),
                new Triangle3(i, j, d),
                new Triangle3(l, c, k),
                new Triangle3(l, k, b)
            };

            List<Triangle3> Subdivide(List<Triangle3> tris)
            {
                List<Triangle3> newTriangles = new List<Triangle3>();

                foreach (Triangle3 triangle in tris)
                {
                    Vector3 a = triangle.a;
                    Vector3 b = triangle.b;
                    Vector3 c = triangle.c;
                    Vector3 d = ((a + b) / 2).Normalized();
                    Vector3 e = ((a + c) / 2).Normalized();
                    Vector3 f = ((b + c) / 2).Normalized();

                    newTriangles.Add(new Triangle3(a, d, e));
                    newTriangles.Add(new Triangle3(d, b, f));
                    newTriangles.Add(new Triangle3(e, f, c));
                    newTriangles.Add(new Triangle3(d, f, e));
                }

                return newTriangles;
            }

            for (int v = 0; v < iterations; v++)
            {
                icosahedron = Subdivide(icosahedron);
            }

            foreach (Triangle3 triangle in icosahedron)
            {
                triangle.a *= radius;
                triangle.b *= radius;
                triangle.c *= radius;
            }

            Triangle3Group group = new Triangle3Group(icosahedron);
            group.origin = origin;

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
            return new Triangle3Group(a.triangles.Concat(new List<Triangle3>(){ b }).ToList());
        }

        public static Triangle3Group operator +(Triangle3Group a, Triangle3Group b)
        {
            return new Triangle3Group(a.triangles.Concat(b.triangles).ToList());
        }
    }
}
