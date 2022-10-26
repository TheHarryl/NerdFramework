using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdFramework
{
    public enum CPUMode
    {
        SingleThreaded,
        MultiThreaded
    }

    public class Renderer3 : Renderer2
    {
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public Ray3Caster camera;
        public Light3Caster cameraLight;

        public MeshTriangle3Collection scene = new MeshTriangle3Collection(new List<MeshTriangle3>());
        public List<Light3Caster> lightSources = new List<Light3Caster>();

        public Color3 fog = new Color3(0, 0, 0, 0.001);//0.05);
        public new int width
        {
            get => _width;
            set
            {
                _width = value;
                depthBuffer = new double[_height, _width];
                lightBuffer = new Color3[_height, _width];
                _cachedSkybox = new Color3[_height, _width];
                normalBuffer = new Vector3[_height, _width];
            }
        }
        public new int height
        {
            get => _height;
            set
            {
                _height = value;
                depthBuffer = new double[_height, _width];
                lightBuffer = new Color3[_height, _width];
                _cachedSkybox = new Color3[_height, _width];
                normalBuffer = new Vector3[_height, _width];
            }
        }

        public double[,] depthBuffer;
        public Vector3[,] normalBuffer;

        public CPUMode CPUMode;

        public Texture2 skyboxFront = Texture2.Black;
        public Texture2 skyboxLeft = Texture2.Black;
        public Texture2 skyboxBack = Texture2.Black;
        public Texture2 skyboxRight = Texture2.Black;
        public Texture2 skyboxTop = Texture2.Black;
        public Texture2 skyboxBottom = Texture2.Black;

        ParallelOptions parallelOptions = new ParallelOptions();

        public ulong frameNum { get; private set; }

        private Vector3 _cachedCameraDirection = Vector3.Zero;
        private Color3[,] _cachedSkybox;

        public Renderer3(Ray3Caster camera, int width = 200, int height = 100) : base(width, height)
        {
            this.camera = camera;
            this.cameraLight = new Light3Caster(new Ray3Radial(new Ray3(camera.d.p - new Vector3(0.0, 0.0, 100), camera.d.v), Math.TwoPI), scene, new Color3Sequence(Color3.White), 10000);

            this.width = width;
            this.height = height;

            Material defaultMaterial = new Material();
            this.materials.Add("None", defaultMaterial);

            this.CPUMode = CPUMode.MultiThreaded;

            this.parallelOptions.MaxDegreeOfParallelism = 10;
        }

        public void AddMaterial(string name, Material material)
        {
            materials[name] = material;
        }
        public void AddMaterials(Dictionary<string, Material> materials)
        {
            materials.ToList().ForEach(x => this.materials[x.Key] = x.Value);
        }
        public Material GetMaterial(string name)
        {
            if (!materials.ContainsKey(name))
                return materials["None"];
            return materials[name];
        }

        public void FillLine(Color3 color, Vector2 begin, Vector2 end)
        {
            bool steep = Math.Abs(end.y - begin.y) > Math.Abs(end.x - begin.x);
            if (steep)
            {
                begin = new Vector2(begin.y, begin.x);
                end = new Vector2(end.y, end.x);
            }
            if (begin.x > end.x)
            {
                double t;
                t = begin.x; // swap begin.x and end.x
                begin.x = end.x;
                end.x = t;

                t = begin.y; // swap begin.y and end.y
                begin.y = end.y;
                end.y = t;
            }
            double dx = (end - begin).x;
            double dy = Math.Abs(end.y - begin.y);
            double error = (dx / 2f);
            int ystep = (begin.y < end.y) ? 1 : -1;
            double y = begin.y;
            for (int x = (int)Math.Max(begin.x, 0.0); x <= Math.Min(end.x, _width-1); x++)
            {
                if (x - 1 >= 0 && x < _width && y - 1 >= 0 && y < _height)
                    lightBuffer[(int)Math.Min(Math.Max((steep ? x : y), 0.0), _height-1), (int)(steep ? y : x)] = color;
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        public void FillTriangle(Color3 color, Vector2 a, Vector2 b, Vector2 c)
        {
            /* Triangle:
             * OP = OA + ABt + ACs
             * 
             * AP = ABt + ACs
             * AP.x = AB.x*t + AC.x*s
             * t >= 0
             * s >= 0
             * t + s <= 1
             * 
             * t = [P.x - P.y*(C.x/C.y)] / [B.x - B.y*(C.x/C.y)]
             * s = [P.x - P.y*(B.x/B.y)] / [C.x - C.y*(B.x/B.y)]
             */

            double minX = Math.Min(a.x, b.x, c.x);
            double maxX = Math.Max(a.x, b.x, c.x);
            double minY = Math.Min(a.y, b.y, c.y);
            double maxY = Math.Max(a.y, b.y, c.y);

            Triangle2 triangle = new Triangle2(a, b, c);
            for (int y = (int)Math.Max(minY, 0.0); y <= (int)Math.Min(maxY, _height - 1); y++)
            {
                for (int x = (int)Math.Max(minX, 0.0); x <= (int)Math.Min((int)maxX, _width - 1); x++)
                {
                    Vector2 pos = triangle.Parameterization(new Vector2(x, y));
                    if (pos.x >= 0.0 && pos.y >= 0.0 && pos.x + pos.y <= 1.0)
                    {
                        lightBuffer[y, x] = color;
                    }
                }
            }
            /*for (int y = (int)minY; y <= (int)maxY; y++)
            {
                Vector2 AB = b - a;
                Vector2 AC = c - a;
                double ABx;
                if (Math.Abs(AB.y) <= 0.001)
                    ABx = AB.x;
                else
                    ABx = b.x * (y - a.y) / AB.y;
                double ACx;
                if (Math.Abs(AC.y) <= 0.001)
                    ACx = AC.x;
                else
                    ACx = c.x * (y - a.y) / AC.y;

                for (int x = (int)Math.Min(ABx, ACx); x <= (int)Math.Max(ABx, ACx); x++)
                {
                    System.Diagnostics.Trace.WriteLine(x + " " + y);
                    lightBuffer[y < 0 ? 0 : (y > 200 ? 200 : y), x < 0 ? 0 : (x > 100 ? 100 : x)] = color;
                }
            }*/
        }

        public void FillCircle(Color3 color, Vector2 pos, int radius)
        {
            /* Circle:
             * (x - p.x)^2 + (y - p.y)^2 = r^2
             * (x - p.x) = [r^2 - (y - p.y)^2]^0.5
             * x = +/-[r^2 - (y - p.y)^2]^0.5 + p.x
             * 
             * MinX: -[r^2 - (y - p.y)^2]^0.5 + p.x
             * MaxX: [r^2 - (y - p.y)^2]^0.5 + p.x
             * MinY: p.y - r
             * MaxY: p.y + r
             */

            for (int y = (int)pos.y - radius; y <= (int)pos.y + radius; y++)
            {
                double yOffset = y - pos.y;
                for (int x = (int)(-Math.Sqrt(radius * radius - yOffset * yOffset) + pos.x); x <= (int)(Math.Sqrt(radius * radius - yOffset * yOffset) + pos.x); x++)
                {
                    lightBuffer[y, x] = color;
                }
            }
        }

        public void FillCircle(Color3Sequence color, Vector2 pos, int radius)
        {
            /* Circle:
             * (x - p.x)^2 + (y - p.y)^2 = r^2
             * (x - p.x) = [r^2 - (y - p.y)^2]^0.5
             * x = +/-[r^2 - (y - p.y)^2]^0.5 + p.x
             * 
             * MinX: -[r^2 - (y - p.y)^2]^0.5 + p.x
             * MaxX: [r^2 - (y - p.y)^2]^0.5 + p.x
             * MinY: p.y - r
             * MaxY: p.y + r
             */

            for (int y = (int)pos.y - radius; y <= (int)pos.y + radius; y++)
            {
                double yOffset = y - pos.y;
                for (int x = (int)(-Math.Sqrt(radius * radius - yOffset * yOffset) + pos.x); x <= (int)(Math.Sqrt(radius * radius - yOffset * yOffset) + pos.x); x++)
                {
                    double xOffset = x - pos.x;
                    double interpolant = (xOffset * xOffset + yOffset * yOffset) / (radius * radius);
                    lightBuffer[y, x] = color.ColorAt(interpolant);
                }
            }
        }

        public Color3 CalculateLighting(Vector3 point, Vector3 normal)
        {
            Vector3 displacement = cameraLight.rayCaster.d.p - point;
            double angle = Vector3.Angle(normal, displacement);
            double distance = displacement.Magnitude();
            return cameraLight.LightAt(distance, angle);
        }

        public void RenderSampled()
        {
            RenderShader((x, y) =>
            {
                int size = 1;
                int y0 = (y > _height - 1 - size ? _height - 1 - size : (y < size ? size : y));
                int x0 = (x > _width - 1 - size ? _width - 1 - size : (x < size ? size : x));
                double max = Math.Max(depthBuffer[y0 - size, x0], depthBuffer[y0 + size, x0], depthBuffer[y0, x0 - size], depthBuffer[y0, x0 + size]);
                return Math.Abs(max - depthBuffer[y, x]) > 1;
            }, (x, y, color) =>
            {
                return Color3.Lerp(Color3.Black, color, 0.5);
            });
        }

        public void RenderDynamicShader(Func<int, int, bool> filter, Func<int, int, Color3, Color3> modification)
        {
            Color3[,] newLightBuffer = new Color3[_height, _width];

            Parallel.For(0, _height, parallelOptions, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    if (filter(x, y))
                        newLightBuffer[y, x] = modification(x, y, lightBuffer[y, x]);
                }
            });

            lightBuffer = newLightBuffer;
        }

        public void RenderShader(Func<int, int, bool> filter, Func<int, int, Color3, Color3> modification)
        {
            Parallel.For(0, _height, parallelOptions, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    if (filter(x, y))
                        lightBuffer[y, x] = modification(x, y, lightBuffer[y, x]);
                }
            });
        }

        public void RenderDirectShader(Action<int, int> modification)
        {
            Parallel.For(0, _height, parallelOptions, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    modification(x, y);
                }
            });
        }

        public void RenderRasterized(double minDist = 0.0, double maxDist = double.MaxValue)
        {
            Parallel.For(0, _height, parallelOptions, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    depthBuffer[y, x] = double.MaxValue;
                    normalBuffer[y, x] = Vector3.Zero;
                }
            });

            void RenderMeshTriangle3(MeshTriangle3 triangle)
            {
                Vector2 a = camera.Projection(triangle.a);
                Vector2 b = camera.Projection(triangle.b);
                Vector2 c = camera.Projection(triangle.c);

                if ((a.x < 0.0 && b.x < 0.0 && c.x < 0.0) ||
                    (a.y < 0.0 && b.y < 0.0 && c.y < 0.0) ||
                    (a.x > 1.0 && b.x > 1.0 && c.x > 1.0) ||
                    (a.y > 1.0 && b.y > 1.0 && c.y > 1.0)) return;
                //if (!Rectangle2.One.Overlaps(a) && !Rectangle2.One.Overlaps(b) && !Rectangle2.One.Overlaps(c)) return;

                a *= new Vector2(_width, _height);
                b *= new Vector2(_width, _height);
                c *= new Vector2(_width, _height);

                double distance1 = camera.Distance(triangle.a);
                double distance2 = camera.Distance(triangle.b);
                double distance3 = camera.Distance(triangle.c);

                double avgDist = Math.Average(distance1, distance2, distance3);
                if (avgDist < minDist || avgDist > maxDist) return;

                Color3 colorA;
                Color3 colorB;
                Color3 colorC;
                Vector3 normal1;
                Vector3 normal2;
                Vector3 normal3;

                Material material = GetMaterial(triangle.material);

                if (triangle.normalType == NormalType.Interpolated)
                {
                    normal1 = triangle.normalA;
                    normal2 = triangle.normalB;
                    normal3 = triangle.normalC;
                }
                else
                {
                    Vector3 normal = triangle.Normal();
                    normal1 = normal;
                    normal2 = normal;
                    normal3 = normal;
                }
                colorA = RenderFog(CalculateLighting(triangle.a, normal1), distance1);
                colorB = RenderFog(CalculateLighting(triangle.b, normal2), distance2);
                colorC = RenderFog(CalculateLighting(triangle.c, normal3), distance3);

                /* Triangle:
                 * OP = OA + ABt + ACs
                 * 
                 * AP = ABt + ACs
                 * AP.x = AB.x*t + AC.x*s
                 */

                Color3 TotalColorAt(double t, double s)
                {
                    Vector2 textureCoords = Vector2.FromParameterization3(t, s, triangle.textureU, triangle.textureV, triangle.textureW);

                    Color3 textureLight = material.texture.ColorAt(textureCoords.x, textureCoords.y);
                    Color3 diffuseLight = Color3.FromVector3(material.diffuseColor);
                    double lightValue = Color3.ValueFromParameterization3(t, s, colorA, colorB, colorC) / 255.0;

                    Color3 flattened = Color3.Flatten(diffuseLight, textureLight);
                    //Color3.Lerp(Color3.Black, Color3.Flatten(diffuseLight, textureLight), lightValue).WithAlpha(material.alpha);
                    return new Color3(
                        (int)(flattened.r * lightValue),
                        (int)(flattened.g * lightValue),
                        (int)(flattened.b * lightValue),
                        ((1.0 - lightValue) + (flattened.alpha * lightValue)) * material.alpha
                    );
                }

                double[] boundsX = Math.Bounds(a.x, b.x, c.x);
                double[] boundsY = Math.Bounds(a.y, b.y, c.y);

                for (int y = (int)Math.Max(boundsY[0], 0.0); y <= Math.Min(boundsY[1], _height - 1); y++)
                {
                    for (int x = (int)Math.Max(boundsX[0], 0.0); x <= Math.Min(boundsX[1], _width - 1); x++)
                    {
                        Vector2 pos = Triangle2.Parameterization(a, b, c, new Vector2(x, y));
                        double dist = Math.FromParameterization3(pos.x, pos.y, distance1, distance2, distance3);
                        if (pos.x >= 0.0 && pos.y >= 0.0 && pos.x + pos.y <= 1.0)
                        {
                            if (dist < depthBuffer[y, x])
                            {
                                Color3 color = TotalColorAt(pos.x, pos.y);
                                lightBuffer[y, x] = Color3.Flatten(lightBuffer[y, x], color);
                                depthBuffer[y, x] = dist;
                                normalBuffer[y, x] = Vector3.FromParameterization3(pos.x, pos.y, normal1, normal2, normal3);
                            }
                            else if (lightBuffer[y, x].alpha < 1.0)
                            {
                                Color3 color = TotalColorAt(pos.x, pos.y);
                                lightBuffer[y, x] = Color3.Flatten(color, lightBuffer[y, x]);
                            }
                        }
                    }
                }

                /*Vector2[] vertices = new Vector2[3] { a, b, c };

                int[] boundsY = Math.BoundsIndex(a.y, b.y, c.y);
                int midVertice = 3 - boundsY[0] - boundsY[1];

                int minY = (int)Math.Max(vertices[boundsY[0]].y, 0.0);
                int maxY = (int)Math.Min(vertices[boundsY[1]].y, _height - 1);

                for (int y = minY; y <= maxY; y++)
                {
                    bool uncertain = false;
                
                    double[] boundsX;
                    if (y == minY || y == maxY) {
                        boundsX = Math.Bounds(a.x, b.x, c.x);
                        uncertain = true;
                    } else
                    {
                        Vector2 majorDiff = vertices[boundsY[0]] - vertices[boundsY[1]];
                        double majorSlope = majorDiff.y / majorDiff.x;
                        double majorX = (y - vertices[boundsY[0]].y) / majorSlope + vertices[boundsY[0]].x;

                        Vector2 diff0 = vertices[midVertice] - vertices[boundsY[0]];
                        double diffSlope0 = diff0.y / diff0.x;
                        double x0 = (y - vertices[midVertice].y) / diffSlope0 + vertices[midVertice].x;

                        Vector2 diff1 = vertices[midVertice] - vertices[boundsY[1]];
                        double diffSlope1 = diff1.y / diff1.x;
                        double x1 = (y - vertices[midVertice].y) / diffSlope1 + vertices[midVertice].x;

                        double minorX = Math.Abs(majorX - x0) < Math.Abs(majorX - x1) ? x0 : x1;
                        boundsX = Math.Bounds(minorX, majorX);
                    }

                    for (int x = (int)Math.Max(boundsX[0], 0.0); x <= Math.Min(boundsX[1], _width - 1); x++)
                    {
                        Vector2 pos = Triangle2.Parameterization(a, b, c, new Vector2(x, y));
                        double dist = Math.FromParameterization3(pos.x, pos.y, distance1, distance2, distance3);
                        if (uncertain && (pos.x < 0 || pos.y < 0 || pos.x + pos.y > 1)) continue;
                        if (dist < depthBuffer[y, x])
                        {
                            Color3 color = TotalColorAt(pos.x, pos.y);
                            lightBuffer[y, x] = Color3.Flatten(lightBuffer[y, x], color);
                            depthBuffer[y, x] = dist;
                            normalBuffer[y, x] = Vector3.FromParameterization3(pos.x, pos.y, normal1, normal2, normal3);
                        }
                        else if (lightBuffer[y, x].alpha < 1.0)
                        {
                            Color3 color = TotalColorAt(pos.x, pos.y);
                            lightBuffer[y, x] = Color3.Flatten(color, lightBuffer[y, x]);
                        }
                    }
                }
                 */

                //FillLine(Color3.Red, a, b);
                //FillLine(Color3.Green, a, c);
                //FillLine(Color3.Blue, c, b);
            }

            OrderedParallelQuery<MeshTriangle3> processed = scene.triangles.AsParallel()
                .Where(t => Vector3.Dot(camera.d.v, t.Normal()) < 0.2 && Vector3.Dot((t.a + t.b + t.c) / 3.0 - camera.d.p, camera.d.v) > 0.0)
                .OrderBy(t => -((t.a + t.b + t.c) / 3.0 - camera.d.p).Magnitude());

            Parallel.ForEach(processed, parallelOptions, RenderMeshTriangle3);

            bool recache = _cachedCameraDirection != camera.d.v;

            Parallel.For(0, _height, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    Color3 SkyboxFromVector(Vector3 vector)
                    {
                        Vector3 projected = vector.NormalizedCubic();

                        if (projected.x == -1)
                            return skyboxLeft.ColorAt(1.0 - (projected.z / 2.0 - 0.5), projected.y / 2.0 + 0.5);
                        else if (projected.x == 1)
                            return skyboxRight.ColorAt(projected.z / 2.0 - 0.5, projected.y / 2.0 + 0.5);
                        else if (projected.y == -1)
                            return skyboxBottom.ColorAt(projected.z / 2.0 + 0.5, projected.x / 2.0 + 0.5);
                        else if (projected.y == 1)
                            return skyboxTop.ColorAt(projected.z / 2.0 + 0.5, 1.0 - (projected.x / 2.0 + 0.5));
                        else if (projected.z == -1)
                            return skyboxBack.ColorAt(projected.x / 2.0 + 0.5, projected.y / 2.0 + 0.5);
                        else if (projected.z == 1)
                            return skyboxFront.ColorAt(1.0 - (projected.x / 2.0 + 0.5), projected.y / 2.0 + 0.5);
                        return Color3.None;
                    }

                    if (recache)
                        _cachedSkybox[y, x] = SkyboxFromVector(camera.VectorAt((double)x / _width, (double)y / _height));

                    if (depthBuffer[y, x] != double.MaxValue)
                    {
                        lightBuffer[y, x] = RenderFog(lightBuffer[y, x], depthBuffer[y, x]);
                        if (lightBuffer[y, x].alpha < 1.0)
                            lightBuffer[y, x] = Color3.Flatten(_cachedSkybox[y, x], lightBuffer[y, x]);
                    }
                    else
                    {
                        lightBuffer[y, x] = _cachedSkybox[y, x];
                    }
                }
            });

            if (recache)
                _cachedCameraDirection = camera.d.v;

            frameNum++;
        }

        public void RenderRaytraced(double minDist = 0.0, double maxDist = double.MaxValue)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    depthBuffer[y, x] = double.MaxValue;
                    lightBuffer[y, x] = Color3.Black;

                    Ray3 ray = camera.RayAt((double)x / _width, (double)y / _height);
                    foreach (Triangle3 triangle in scene.triangles)
                    {
                        if (triangle.Meets(ray))
                        {
                            Vector3 intersection = triangle.Intersection(ray);
                            double distance = (intersection - ray.p).Magnitude();
                            if (distance >= depthBuffer[y, x] || distance < minDist || distance > maxDist) continue;

                            double distance2 = (intersection - cameraLight.rayCaster.d.p).Magnitude();
                            depthBuffer[y, x] = distance;

                            double angle = Vector3.Angle(triangle.Normal(), (cameraLight.rayCaster.d.p - intersection));
                            //double interpolant = angle / Math.HalfPI;
                            //if (interpolant > 1.0)
                            //    interpolant = 1.0;
                            lightBuffer[y, x] = cameraLight.LightAt(distance2, angle);
                        }
                    }

                    lightBuffer[y, x] = RenderFog(lightBuffer[y, x], depthBuffer[y, x]);
                }
            }

            frameNum++;
        }

        private Color3 RenderFog(Color3 original, double distance)
        {
            /* Uses alpha as intensity of fog per unit distance
             */

            return Color3.Lerp(fog.Opaque(), original, Math.Pow(1.0 - fog.alpha, distance));
        }
    }
}
