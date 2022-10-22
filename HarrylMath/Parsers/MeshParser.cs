using System;
using System.Collections.Generic;

namespace NerdFramework
{
    public static class MeshParser
    {
        public static Mesh FromFile(string fileLocation, bool overrideNormalInterpolation = false, string material = "")
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textureCoords = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<MeshTriangle3> triangles = new List<MeshTriangle3>();

            string currentMaterial = "None";
            if (currentMaterial != "")
                currentMaterial = material;

            /* Object (OBJ) Specifications
             * http://paulbourke.net/dataformats/obj/
             */
            if (fileLocation.ToLower().EndsWith(".obj"))
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);

                foreach (string line in lines)
                {
                    string[] args = line.Split(" ");
                    switch (args[0])
                    {
                        case "usemtl":
                            if (material == "")
                                currentMaterial = args[1];
                            break;
                        case "v":
                            vertices.Add(new Vector3(Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3])));
                            break;
                        case "vt":
                            textureCoords.Add(new Vector2(Convert.ToDouble(args[1]), Convert.ToDouble(args[2])));
                            break;
                        case "vn":
                            normals.Add(new Vector3(Convert.ToDouble(args[1]), Convert.ToDouble(args[2]), Convert.ToDouble(args[3])));
                            break;
                        case "f":
                            List<Vector3> faceVertices = new List<Vector3>();
                            List<Vector2> faceTextures = new List<Vector2>();
                            List<Vector3> faceNormals = new List<Vector3>();
                            for (int i = 1; i < args.Length; i++)
                            {
                                string[] args1 = args[i].Split("/");
                                faceVertices.Add(vertices[Int32.Parse(args1[0]) - 1]);
                                if (args1.Length >= 2 && !args1[1].Equals(""))
                                    faceTextures.Add(textureCoords[Int32.Parse(args1[1]) - 1]);
                                if (args1.Length >= 3)
                                    faceNormals.Add(normals[Int32.Parse(args1[2]) - 1]);
                            }
                            if (faceVertices.Count == 3)
                            {
                                MeshTriangle3 triangle = new MeshTriangle3(faceVertices[0], faceVertices[1], faceVertices[2]);
                                if (faceTextures.Count == 3)
                                {
                                    triangle.textureU = faceTextures[0];
                                    triangle.textureV = faceTextures[1];
                                    triangle.textureW = faceTextures[2];
                                }
                                if (faceNormals.Count == 3 && !overrideNormalInterpolation)
                                {
                                    triangle.normalA = faceNormals[0];
                                    triangle.normalB = faceNormals[1];
                                    triangle.normalC = faceNormals[2];
                                    triangle.normalType = NormalType.Interpolated;
                                }
                                triangle.material = currentMaterial;
                                triangles.Add(triangle);
                            }
                            else if (faceVertices.Count == 4)
                            {
                                MeshQuad3 quad = new MeshQuad3(faceVertices[0], faceVertices[1], faceVertices[2], faceVertices[3]);
                                if (faceTextures.Count == 4)
                                {
                                    quad.textureU = faceTextures[0];
                                    quad.textureV = faceTextures[1];
                                    quad.textureW = faceTextures[2];
                                    quad.textureX = faceTextures[3];
                                }
                                if (faceNormals.Count == 4 && !overrideNormalInterpolation)
                                {
                                    quad.normalA = faceNormals[0];
                                    quad.normalB = faceNormals[1];
                                    quad.normalC = faceNormals[2];
                                    quad.normalD = faceNormals[3];
                                    quad.normalType = NormalType.Interpolated;
                                }
                                quad.material = currentMaterial;
                                triangles.Add(quad.GetTriangle1());
                                triangles.Add(quad.GetTriangle2());
                            }
                            break;
                    }
                }
            }

            return new Mesh(new MeshTriangle3Collection(triangles));
        }

        public static Mesh FromCube(Vector3 origin, double sideLength)
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

            MeshTriangle3Collection group = new MeshTriangle3Collection(new List<MeshTriangle3>()
            {
                new MeshTriangle3(b, d, f),
                new MeshTriangle3(h, f, d),
                new MeshTriangle3(b, a, d),
                new MeshTriangle3(c, d, a),
                new MeshTriangle3(d, c, h),
                new MeshTriangle3(g, h, c),
                new MeshTriangle3(h, g, f),
                new MeshTriangle3(e, f, g),
                new MeshTriangle3(f, e, b),
                new MeshTriangle3(a, b, e),
                new MeshTriangle3(a, e, c),
                new MeshTriangle3(g, c, e),
            });
            group.origin = origin;

            return new Mesh(group);
        }

        public static Mesh FromUVSphere(Vector3 origin, double radius, int segments = 2, int rings = 4)
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

            List<MeshTriangle3> triangles = new List<MeshTriangle3>();
            for (int y = 0; y < rings - 1; y++)
            {
                for (int x = 0; x < segments; x++)
                {
                    Vector3 a = vertices[1 + y * segments + x];
                    Vector3 b = vertices[1 + (y + 1) * segments + x];
                    Vector3 c = vertices[2 + (y + 1) * segments + x];
                    Vector3 d = vertices[2 + y * segments + x];
                    MeshQuad3 quad = new MeshQuad3(a, b, c, d);
                    triangles.Add(quad.GetTriangle1());
                    triangles.Add(quad.GetTriangle2());
                }
            }

            MeshTriangle3Collection group = new MeshTriangle3Collection(triangles);
            group.origin = origin;

            return new Mesh(group);
        }

        public static Mesh FromIcoSphere(Vector3 origin, double radius, int iterations = 2, NormalType normalType = NormalType.Default)
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
            List<MeshTriangle3> icosahedron = new List<MeshTriangle3>()
            {
                new MeshTriangle3(e, l, b),
                new MeshTriangle3(e, b, a),
                new MeshTriangle3(e, a, i),
                new MeshTriangle3(f, i, d),
                new MeshTriangle3(f, d, c),
                new MeshTriangle3(f, c, l),
                new MeshTriangle3(e, i, f),
                new MeshTriangle3(f, l, e),
                new MeshTriangle3(b, k, h),
                new MeshTriangle3(b, h, a),
                new MeshTriangle3(h, j, a),
                new MeshTriangle3(d, j, g),
                new MeshTriangle3(d, g, c),
                new MeshTriangle3(c, g, k),
                new MeshTriangle3(g, j, h),
                new MeshTriangle3(h, k, g),
                new MeshTriangle3(i, a, j),
                new MeshTriangle3(i, j, d),
                new MeshTriangle3(l, c, k),
                new MeshTriangle3(l, k, b)
            };

            List<MeshTriangle3> Subdivide(List<MeshTriangle3> tris)
            {
                List<MeshTriangle3> newTriangles = new List<MeshTriangle3>();

                foreach (MeshTriangle3 triangle in tris)
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

                    newTriangles.Add(new MeshTriangle3(a, d, e));
                    newTriangles.Add(new MeshTriangle3(d, b, f));
                    newTriangles.Add(new MeshTriangle3(e, f, c));
                    newTriangles.Add(new MeshTriangle3(d, f, e));
                }

                return newTriangles;
            }

            for (int v = 0; v < iterations; v++)
            {
                icosahedron = Subdivide(icosahedron);
            }

            foreach (MeshTriangle3 triangle in icosahedron)
            {
                if (normalType == NormalType.Interpolated)
                {
                    triangle.normalType = normalType;
                    triangle.normalA = triangle.a;
                    triangle.normalB = triangle.b;
                    triangle.normalC = triangle.c;
                }
                triangle.a *= radius;
                triangle.b *= radius;
                triangle.c *= radius;
            }

            MeshTriangle3Collection group = new MeshTriangle3Collection(icosahedron);
            group.origin = origin;

            return new Mesh(group);
        }

        public static Mesh FromQuadSphere(Vector3 origin, double radius, int iterations = 2, NormalType normalType = NormalType.Default)
        {
            Vector3 a = new Vector3(1, 1, -1).Normalized();
            Vector3 b = new Vector3(1, 1, 1).Normalized();
            Vector3 c = new Vector3(-1, 1, -1).Normalized();
            Vector3 d = new Vector3(-1, 1, 1).Normalized();
            Vector3 e = new Vector3(1, -1, -1).Normalized();
            Vector3 f = new Vector3(1, -1, 1).Normalized();
            Vector3 g = new Vector3(-1, -1, -1).Normalized();
            Vector3 h = new Vector3(-1, -1, 1).Normalized();

            List<MeshQuad3> hexahedron = new List<MeshQuad3>()
            {
                new MeshQuad3(c, d, b, a),
                new MeshQuad3(g, h, d, c),
                new MeshQuad3(e, f, h, g),
                new MeshQuad3(a, b, f, e),
                new MeshQuad3(d, h, f, b),
                new MeshQuad3(g, c, a, e)
            };

            List<MeshQuad3> Subdivide(List<MeshQuad3> quads)
            {
                List<MeshQuad3> newQuads = new List<MeshQuad3>();

                foreach (MeshQuad3 quad in quads)
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

                    newQuads.Add(new MeshQuad3(a, h, i, e));
                    newQuads.Add(new MeshQuad3(e, i, f, d));
                    newQuads.Add(new MeshQuad3(h, b, g, i));
                    newQuads.Add(new MeshQuad3(i, g, c, f));
                }

                return newQuads;
            }

            for (int v = 0; v < iterations; v++)
            {
                hexahedron = Subdivide(hexahedron);
            }

            List<MeshTriangle3> quadSphere = new List<MeshTriangle3>();

            foreach (MeshQuad3 quad in hexahedron)
            {
                if (normalType == NormalType.Interpolated)
                {
                    quad.normalType = normalType;
                    quad.normalA = quad.a;
                    quad.normalB = quad.b;
                    quad.normalC = quad.c;
                    quad.normalD = quad.d;
                }

                quad.a *= radius;
                quad.b *= radius;
                quad.c *= radius;
                quad.d *= radius;
                quadSphere.Add(quad.GetTriangle1());
                quadSphere.Add(quad.GetTriangle2());
            }

            MeshTriangle3Collection group = new MeshTriangle3Collection(quadSphere);
            group.origin = origin;

            return new Mesh(group);
        }
    }
}
