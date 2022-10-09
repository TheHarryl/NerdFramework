using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NerdFramework
{
    public static class MeshParser
    {
        public static Triangle3Collection FromFile(string fileLocation)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Triangle3> triangles = new List<Triangle3>();

            if (fileLocation.ToLower().EndsWith(".obj"))
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);

                foreach (string line in lines)
                {
                    string[] args = line.Split(" ");
                    if (args[0] == "v")
                    {
                        if (args.Length == 4)
                        {
                            vertices.Add(new Vector3(Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3])));
                        }
                    }
                    else if (args[0] == "vn")
                    {
                        normals.Add(new Vector3(Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3])));
                    }
                    else if (args[0] == "f")
                    {
                        List<Vector3> faceVertices = new List<Vector3>();
                        List<Vector3> faceNormals = new List<Vector3>();
                        for (int i = 1; i < args.Length; i++)
                        {
                            string[] args1 = args[i].Split("/");
                            faceVertices.Add(vertices[Int32.Parse(args1[0]) - 1]);
                            if (args1.Length == 1) continue;
                            //faceNormals.Add(vertices[Int32.Parse(args1[2]) - 1]);
                        }
                        if (faceVertices.Count == 3)
                            triangles.Add(new Triangle3(faceVertices[0], faceVertices[1], faceVertices[2]));
                        else if (faceVertices.Count == 4)
                        {
                            Quad3 quad = new Quad3(faceVertices[0], faceVertices[1], faceVertices[2], faceVertices[3]);
                            triangles.Add(quad.GetTriangle1());
                            triangles.Add(quad.GetTriangle2());
                        }
                    }
                }
            }

            return new Triangle3Collection(triangles);
        }

        public static Triangle3Collection FromCube(Vector3 origin, double sideLength)
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

            Triangle3Collection group = new Triangle3Collection(new List<Triangle3>()
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
            group.origin = origin;

            return group;
        }

        public static Triangle3Collection FromUVSphere(Vector3 origin, double radius, int segments = 2, int rings = 4)
        {
            Vector3[] ring = new Vector3[segments];

            for (int i = 0; i < segments; i++)
            {
                ring[i] = Vector3.xAxis.RotateY(Math.TwoPI * ((double)i / segments));
            }

            List<Vector3> vertices = new List<Vector3>();

            vertices.Add(new Vector3(0.0, -radius, 0.0));
            for (int i = 1; i <= rings; i++)
            {
                foreach (Vector3 vertice in ring)
                {
                    /* a^2 + b^2 = c^2
                     * a = sqrt(c^2 - b^2)
                     */

                    double b = (((double)i / (rings + 1)) * 2.0 - 1.0) * radius;

                    vertices.Add(vertice + b);
                }
            }
            vertices.Add(new Vector3(0.0, radius, 0.0));

            List<Triangle3> triangles = new List<Triangle3>();
            for (int y = 0; y < rings - 1; y++)
            {
                for (int x = 0; x < segments; x++)
                {
                    Vector3 a = vertices[1 + y * segments + x];
                    Vector3 b = vertices[1 + (y + 1) * segments + x];
                    Vector3 c = vertices[2 + (y + 1) * segments + x];
                    Vector3 d = vertices[2 + y * segments + x];
                    Quad3 quad = new Quad3(a, b, c, d);
                    triangles.Add(quad.GetTriangle1());
                    triangles.Add(quad.GetTriangle2());
                }
            }

            Triangle3Collection group = new Triangle3Collection(triangles);
            group.origin = origin;

            return group;
        }

        public static Triangle3Collection FromIcoSphere(Vector3 origin, double radius, int iterations = 2)
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

            // This was NOT poggers to make
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
                new Triangle3(h, j, a),
                new Triangle3(d, j, g),
                new Triangle3(d, g, c),
                new Triangle3(c, g, k),
                new Triangle3(g, j, h),
                new Triangle3(h, k, g),
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
                    /*      A
                     *    /   \
                     *   /     \
                     *  /       \
                     * B ------- C
                     */

                    /*      A
                     *    /   \
                     *   D --- E
                     *  / \   / \
                     * B ---F--- C
                     */

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

            Triangle3Collection group = new Triangle3Collection(icosahedron);
            group.origin = origin;

            return group;
        }

        public static Triangle3Collection FromQuadSphere(Vector3 origin, double radius, int iterations = 2)
        {
            Vector3 a = new Vector3(1, 1, -1).Normalized();
            Vector3 b = new Vector3(1, 1, 1).Normalized();
            Vector3 c = new Vector3(-1, 1, -1).Normalized();
            Vector3 d = new Vector3(-1, 1, 1).Normalized();
            Vector3 e = new Vector3(1, -1, -1).Normalized();
            Vector3 f = new Vector3(1, -1, 1).Normalized();
            Vector3 g = new Vector3(-1, -1, -1).Normalized();
            Vector3 h = new Vector3(-1, -1, 1).Normalized();

            List<Quad3> hexahedron = new List<Quad3>()
            {
                new Quad3(c, d, b, a),
                new Quad3(g, h, d, c),
                new Quad3(e, f, h, g),
                new Quad3(a, b, f, e),
                new Quad3(d, h, f, b),
                new Quad3(g, c, a, e)
            };

            List<Quad3> Subdivide(List<Quad3> quads)
            {
                List<Quad3> newQuads = new List<Quad3>();

                foreach (Quad3 quad in quads)
                {
                    /* A ----- D
                     * |       |
                     * |       |
                     * |       |
                     * B ----- C
                     */

                    /* A - E - D
                     * |   |   |
                     * H - I - F
                     * |   |   |
                     * B - G - C
                     */

                    Vector3 a = quad.a;
                    Vector3 b = quad.b;
                    Vector3 c = quad.c;
                    Vector3 d = quad.d;
                    Vector3 e = ((a + d) / 2.0).Normalized();
                    Vector3 f = ((d + c) / 2.0).Normalized();
                    Vector3 g = ((c + b) / 2.0).Normalized();
                    Vector3 h = ((b + a) / 2.0).Normalized();
                    Vector3 i = ((a + d + c + b) / 4.0).Normalized();

                    newQuads.Add(new Quad3(a, h, i, e));
                    newQuads.Add(new Quad3(e, i, f, d));
                    newQuads.Add(new Quad3(h, b, g, i));
                    newQuads.Add(new Quad3(i, g, c, f));
                }

                return newQuads;
            }

            for (int v = 0; v < iterations; v++)
            {
                hexahedron = Subdivide(hexahedron);
            }

            List<Triangle3> quadSphere = new List<Triangle3>();

            foreach (Quad3 quad in hexahedron)
            {
                quad.a *= radius;
                quad.b *= radius;
                quad.c *= radius;
                quad.d *= radius;
                quadSphere.Add(quad.GetTriangle1());
                quadSphere.Add(quad.GetTriangle2());
            }

            Triangle3Collection group = new Triangle3Collection(quadSphere);
            group.origin = origin;

            return group;
        }
    }
}
