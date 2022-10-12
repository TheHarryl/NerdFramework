using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdFramework
{
    public class Renderer3
    {
        public Dictionary<string, Material> materials = new Dictionary<string, Material>();

        public Ray3Caster camera;
        public Light3Caster cameraLight;

        public MeshTriangle3Collection scene = new MeshTriangle3Collection(new List<MeshTriangle3>());
        public List<Light3Caster> lightSources = new List<Light3Caster>();

        public Color3 fog = new Color3(0, 0, 0, 0.0);//0.05);
        private int _width = 1;
        private int _height = 1;
        public int width
        {
            get => _width;
            set
            {
                _width = value;
                depthBuffer = new double[_height, _width];
                lightBuffer = new Color3[_height, _width];
            }
        }
        public int height
        {
            get => _height;
            set
            {
                _height = value;
                depthBuffer = new double[_height, _width];
                lightBuffer = new Color3[_height, _width];
            }
        }

        public double[,] depthBuffer;
        public Color3[,] lightBuffer;

        public Renderer3(Ray3Caster camera, int width = 200, int height = 100)
        {
            this.camera = camera;
            this.cameraLight = new Light3Caster(new Ray3Radial(new Ray3(camera.d.p - new Vector3(0.0, 0.0, 100), camera.d.v), Math.TwoPI), scene, new Color3Sequence(Color3.White), 10000);

            this.width = width;
            this.height = height;

            Material defaultMaterial = new Material();
            this.materials.Add("None", defaultMaterial);
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
            for (int x = (int)Math.Max(begin.x, 0.0); x <= Math.Min(end.x, width-1); x++)
            {
                if (x - 1 >= 0 && x < width && y - 1 >= 0 && y < height)
                    lightBuffer[(int)Math.Min(Math.Max((steep ? x : y), 0.0), height-1), (int)(steep ? y : x)] = color;
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

        public void FillTriangle(RasterizedTriangle2 colorTriangle)
        {
            /* Triangle:
             * OP = OA + ABt + ACs
             * 
             * AP = ABt + ACs
             * AP.x = AB.x*t + AC.x*s
             */

            double minX = Math.Min(colorTriangle.a.x, colorTriangle.b.x, colorTriangle.c.x);
            double maxX = Math.Max(colorTriangle.a.x, colorTriangle.b.x, colorTriangle.c.x);
            double minY = Math.Min(colorTriangle.a.y, colorTriangle.b.y, colorTriangle.c.y);
            double maxY = Math.Max(colorTriangle.a.y, colorTriangle.b.y, colorTriangle.c.y);

            for (int y = (int)Math.Max(minY, 0.0); y <= (int)Math.Min(maxY, _height - 1); y++)
            {
                for (int x = (int)Math.Max(minX, 0.0); x <= (int)Math.Min((int)maxX, _width - 1) ; x++)
                {
                    Vector2 pos = colorTriangle.Parameterization(new Vector2(x, y));
                    double dist = colorTriangle.DistanceAt(pos.x, pos.y);
                    if (pos.x >= 0.0 && pos.y >= 0.0 && pos.x + pos.y <= 1.0)
                    {
                        if (dist < depthBuffer[y, x])
                        {
                            Color3 color = colorTriangle.TotalColorAt(pos.x, pos.y);
                            lightBuffer[y, x] = lightBuffer[y, x].WithOverlayed(color);
                            depthBuffer[y, x] = dist;
                        } else if (lightBuffer[y, x].alpha < 0.9999)
                        {
                            Color3 color = colorTriangle.TotalColorAt(pos.x, pos.y);
                            lightBuffer[y, x] = color.WithOverlayed(lightBuffer[y, x]);
                        }
                    }
                }
            }
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
            double angle = Vector3.Angle(normal, (cameraLight.rayCaster.d.p - point));
            double distance = (point - cameraLight.rayCaster.d.p).Magnitude();
            return cameraLight.LightAt(distance, angle);
        }

        public void RenderSampled()
        {
            Color3[,] newLightBuffer = new Color3[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int minY = (int)Math.Max(y - 1, 0.0);
                    int maxY = (int)Math.Min(y + 1, _height - 1);
                    int minX = (int)Math.Max(x - 1, 0.0);
                    int maxX = (int)Math.Min(x + 1, _width - 1);
                    Color3[] colors = new Color3[(maxX - minX + 1) * (maxY - minY + 1)];
                    int i = 0;
                    for (int y1 = minY; y1 <= maxY; y1++)
                    {
                        for (int x1 = minX; x1 <= maxX; x1++)
                        {
                            colors[i] = lightBuffer[y1, x1];
                            i++;
                        }
                    }
                    newLightBuffer[y, x] = Color3.Average(colors);
                }
            }
            lightBuffer = newLightBuffer;
        }

        public void RenderShader(Func<int, int, bool> filter, Func<Color3, Color3> modification)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (filter(y, x))
                        lightBuffer[y, x] = modification(lightBuffer[y, x]);
                }
            }
        }

        public void RenderRasterized()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    depthBuffer[y, x] = double.MaxValue;
                    lightBuffer[y, x] = Color3.None;
                }
            }
            scene.triangles = scene.triangles.OrderBy(t => ((t.a + t.b + t.c) / 3.0 - camera.d.p).Magnitude()).ToList();
            List<RasterizedTriangle2> projectedTriangles = new List<RasterizedTriangle2>();
            foreach (MeshTriangle3 triangle in scene.triangles)
            {
                if (Vector3.Dot(camera.d.v, triangle.Normal()) >= 0.0) continue;

                Vector2 a = camera.Projection(triangle.a); 
                Vector2 b = camera.Projection(triangle.b);
                Vector2 c = camera.Projection(triangle.c);

                Rectangle2 visible = new Rectangle2(Vector2.Zero, Vector2.One);

                if (!visible.Overlaps(a) && !visible.Overlaps(b) && !visible.Overlaps(c)) continue;

                a *= new Vector2(width, height);
                b *= new Vector2(width, height);
                c *= new Vector2(width, height);

                double distance1 = camera.Distance(triangle.a);
                double distance2 = camera.Distance(triangle.b);
                double distance3 = camera.Distance(triangle.c);

                Color3 colorA;
                Color3 colorB;
                Color3 colorC;

                Material material = GetMaterial(triangle.material);

                if (triangle.normalType == NormalType.Interpolated)
                {
                    colorA = RenderFog(CalculateLighting(triangle.a, triangle.normalA), distance1);
                    colorB = RenderFog(CalculateLighting(triangle.b, triangle.normalB), distance2);
                    colorC = RenderFog(CalculateLighting(triangle.c, triangle.normalC), distance3);
                } else
                {
                    Vector3 normal = triangle.Normal();
                    colorA = RenderFog(CalculateLighting(triangle.a, normal), distance1);
                    colorB = RenderFog(CalculateLighting(triangle.b, normal), distance2);
                    colorC = RenderFog(CalculateLighting(triangle.c, normal), distance3);
                }
                FillTriangle(new RasterizedTriangle2(a, b, c, colorA, colorB, colorC, distance1, distance2, distance3, triangle.textureU, triangle.textureV, triangle.textureW, material));

                //FillLine(Color3.Red, a, b);
                //FillLine(Color3.Green, a, c);
                //FillLine(Color3.Blue, c, b);
            }
        }

        public void RenderRaytraced()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    depthBuffer[y, x] = double.MaxValue;
                    lightBuffer[y, x] = Color3.Black;

                    Ray3 ray = camera.RayAt((double)x / width, (double)y / height);
                    foreach (Triangle3 triangle in scene.triangles)
                    {
                        if (triangle.Meets(ray))
                        {
                            Vector3 intersection = triangle.Intersection(ray);
                            double distance = (intersection - ray.p).Magnitude();
                            double distance2 = (intersection - cameraLight.rayCaster.d.p).Magnitude();
                            if (distance >= depthBuffer[y, x]) continue;
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
        }

        private Color3 RenderFog(Color3 original, double distance)
        {
            /* Uses alpha as intensity of fog per unit distance
             */

            return Color3.Lerp(fog.Opaque(), original, Math.Pow(1.0 - fog.alpha, distance));
        }
    }
}
