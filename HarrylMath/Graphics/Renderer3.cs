using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Renderer3
    {
        public Ray3Caster camera;

        public Triangle3Group scene;
        public List<Light3Caster> lightSources;

        public Color3 fog;
        private int _width;
        private int _height;

        public double[,] depthBuffer;
        public Triangle3[,] triangleBuffer;
        public Color3[,] lightBuffer;

        public Renderer3(Ray3Caster camera, int width, int height)
        {
            this.camera = camera;

            _width = width;
            _height = height;

            depthBuffer = new double[height, width];
            triangleBuffer = new Triangle3[height, width];
            lightBuffer = new Color3[height, width];
        }

        public void Render()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    depthBuffer[y, x] = double.MaxValue;
                    triangleBuffer[y, x] = null;
                    Ray3 ray = camera.Ray((double)x / _width, (double)y / _height);
                    foreach (Triangle3 triangle in scene.triangles)
                    {
                        if (triangle.Meets(ray))
                        {
                            double distance = (triangle.Intersection(ray) - ray.p).Magnitude();
                            if (distance >= depthBuffer[y, x]) continue;
                            depthBuffer[y, x] = distance;
                            triangleBuffer[y, x] = triangle;
                        }
                    }
                }
            }
        }
    }
}
