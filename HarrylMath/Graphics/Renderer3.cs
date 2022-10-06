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
            this.cameraLight = new Light3Caster(new Ray3Radial(new Ray3(camera.d.p, camera.d.v), Math.TwoPI), scene, new Color3Sequence(Color3.White), 10000);

            this.width = width;
            this.height = height;

            depthBuffer = new double[height, width];
            lightBuffer = new Color3[height, width];
        }

        public void FillLine(Color3 color, int x1, int y1, int x2, int y2)
        {
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int t = x1;
                y1 = x1;
                x1 = t;

                t = x2;
                y2 = x2;
                x2 = t;
            }
            if (x1 > x2)
            {
                int t = x1;
                x1 = x2;
                x2 = t;

                t = y1;
                y1 = y2;
                y2 = t;
            }
            double dx = x2 - x1;
            double dy = Math.Abs(y2 - y1);
            double error = (dx / 2f);
            int ystep = (y1 < y2) ? 1 : -1;
            double y = y1;
            for (int x = (int)x1; x <= x2; x++)
            {
                if (x - 1 >= 0 && x < width && y - 1 >= 0 && y < height)
                    lightBuffer[(int)((steep ? x : y) - 1), (int)(steep ? y : x) - 1] = color;
                error -= dy;
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

        public void RenderRasterized()
        {
            scene.triangles = scene.triangles.OrderBy(t => ((t.a + t.b + t.c) / 3.0 - camera.d.p).Magnitude()).ToList();
            foreach (Triangle3 triangle in scene.triangles)
            {
                Vector2 a = camera.Projection(triangle.a) * new Vector2(width, height);
                Vector2 b = camera.Projection(triangle.b) * new Vector2(width, height);
                Vector2 c = camera.Projection(triangle.c) * new Vector2(width, height);
                FillLine(Color3.White, (int)triangle.a.x, (int)triangle.a.y, (int)triangle.b.x, (int)triangle.b.y);
                FillLine(Color3.White, (int)triangle.b.x, (int)triangle.b.y, (int)triangle.c.x, (int)triangle.c.y);
                FillLine(Color3.White, (int)triangle.c.x, (int)triangle.c.y, (int)triangle.a.x, (int)triangle.a.y);
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
                            if (distance >= depthBuffer[y, x]) continue;
                            depthBuffer[y, x] = distance;

                            double angle = Vector3.Angle(triangle.Normal(), (intersection - cameraLight.rayCaster.d.p));
                            //double interpolant = angle / Math.HalfPI;
                            //if (interpolant > 1.0)
                            //    interpolant = 1.0;
                            lightBuffer[y, x] = cameraLight.LightAt(distance, angle);
                        }
                    }

                    lightBuffer[y, x] = RenderFog(lightBuffer[y, x], depthBuffer[y, x]);
                }
            }
        }

        public Color3 RenderFog(Color3 original, double distance)
        {
            /* Uses alpha as intensity of fog per unit distance
             */

            return Color3.Lerp(fog.WithoutAlpha(), original, Math.Pow(1.0 - fog.alpha, distance));
        }
    }
}
