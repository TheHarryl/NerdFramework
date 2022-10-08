using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdFramework
{
    public class Renderer3
    {
        public Ray3Caster camera;
        public Light3Caster cameraLight;

        public Triangle3Group scene = new Triangle3Group(new List<Triangle3>());
        public List<Light3Caster> lightSources = new List<Light3Caster>();

        public Color3 fog = new Color3(0, 0, 0, 0.05);
        public readonly int width;
        public readonly int height;

        public double[,] depthBuffer;
        public Color3[,] lightBuffer;

        public Renderer3(Ray3Caster camera, int width, int height)
        {
            this.camera = camera;
            this.cameraLight = new Light3Caster(new Ray3Radial(new Ray3(camera.d.p - new Vector3(0.0, 0.0, 100), camera.d.v), Math.TwoPI), scene, new Color3Sequence(Color3.White), 10000);

            this.width = width;
            this.height = height;

            depthBuffer = new double[height, width];
            lightBuffer = new Color3[height, width];
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
            for (int x = (int)begin.x; x <= end.x; x++)
            {
                if (x - 1 >= 0 && x < width && y - 1 >= 0 && y < height)
                    lightBuffer[height - (int)(steep ? x : y), (int)(steep ? y : x)] = color;
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        public void FillTriangle(Color3 color, int x1, int y1, int x2, int y2)
        {
            for (int y = y1; y <= y2; y++)
            {

            }
        }

        public Color3 CalculateLighting(Vector3 point, Vector3 normal)
        {
            double angle = Vector3.Angle(normal, (cameraLight.rayCaster.d.p - point));
            double distance = (point - cameraLight.rayCaster.d.p).Magnitude();
            return cameraLight.LightAt(distance, angle);
        }

        public void RenderRasterized()
        {
            //scene.triangles = scene.triangles.OrderBy(t => ((t.a + t.b + t.c) / 3.0 - camera.d.p).Magnitude()).ToList();
            List<ColorTriangle2> projectedTriangles = new List<ColorTriangle2>();
            foreach (Triangle3 triangle in scene.triangles)
            {
                if (Vector3.Dot(camera.d.v, triangle.Normal()) >= 0.0) continue;

                Vector2 a = camera.Projection(triangle.a) * new Vector2(width, height);
                Vector2 b = camera.Projection(triangle.b) * new Vector2(width, height);
                Vector2 c = camera.Projection(triangle.c) * new Vector2(width, height);
                Vector3 normal = triangle.Normal();
                double distance1 = (triangle.a - camera.d.p).Magnitude();
                double distance2 = (triangle.b - camera.d.p).Magnitude();
                double distance3 = (triangle.c - camera.d.p).Magnitude();
                Color3 colorA = RenderFog(CalculateLighting(triangle.a, normal), distance1);
                Color3 colorB = RenderFog(CalculateLighting(triangle.b, normal), distance2);
                Color3 colorC = RenderFog(CalculateLighting(triangle.c, normal), distance3);

                projectedTriangles.Add(new ColorTriangle2(a, b, c, colorA, colorB, colorC));
            }
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //depthBuffer[y, x] = double.MaxValue;
                    lightBuffer[y, x] = Color3.Black;

                    Vector2 point = new Vector2(x, y);
                    foreach (ColorTriangle2 projectedTriangle in projectedTriangles)
                    {
                        Vector2 pos = projectedTriangle.Parameterization(point);
                        if (pos.x >= 0.0 && pos.y >= 0.0 && pos.x + pos.y <= 1.0)
                        {
                            lightBuffer[y, x] = projectedTriangle.ColorAt(pos.x, pos.y);
                            break;
                        }
                    }
                }
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

            return Color3.Lerp(fog.WithoutAlpha(), original, Math.Pow(1.0 - fog.alpha, distance));
        }
    }
}
