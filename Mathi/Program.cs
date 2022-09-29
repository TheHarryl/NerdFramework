using Math = System.Math;
using Ray3Region = NerdFramework.Ray3Region;
using Ray3Sector = NerdFramework.Ray3Sector;
using Ray3 = NerdFramework.Ray3;
using Vector3 = NerdFramework.Vector3;
using Triangle3 = NerdFramework.Triangle3;
using Triangle3Group = NerdFramework.Triangle3Group;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mathi
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();
            List<char> light = new List<char> { ',', '\"', '^', '`', '\'', '.', '-', '~', '_' };

            Ray3Region camera = new Ray3Region(new Ray3(Vector3.Zero, Vector3.zAxis), 70, 35);
            //Ray3Sector camera = new Ray3Sector(new Ray3(new Vector3(0,0,-10), Vector3.zAxis), 1.5, 1.0);

            Triangle3Group tris =
                new Triangle3(new Vector3(-10, -10, -10), new Vector3(-10, 10, -10), new Vector3(10, -10, -10)) +
                new Triangle3(new Vector3(10, 10, -10), new Vector3(10, -10, -10), new Vector3(-10, 10, -10)) +
                new Triangle3(new Vector3(-10, -10, 10), new Vector3(10, -10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, 10, 10), new Vector3(-10, 10, 10), new Vector3(10, -10, 10));

            Triangle3Group tris2 =
                new Triangle3(new Vector3(-10, -10, -10), new Vector3(-10, 10, -10), new Vector3(10, -10, -10)) +
                new Triangle3(new Vector3(10, 10, -10), new Vector3(10, -10, -10), new Vector3(-10, 10, -10)) +
                new Triangle3(new Vector3(-10, -10, 10), new Vector3(10, -10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, 10, 10), new Vector3(-10, 10, 10), new Vector3(10, -10, 10));

            Triangle3Group tris3 =
                new Triangle3(new Vector3(-10, -10, -10), new Vector3(-10, 10, -10), new Vector3(10, -10, -10)) +
                new Triangle3(new Vector3(10, 10, -10), new Vector3(10, -10, -10), new Vector3(-10, 10, -10)) +
                new Triangle3(new Vector3(-10, -10, 10), new Vector3(10, -10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, 10, 10), new Vector3(-10, 10, 10), new Vector3(10, -10, 10));

            tris2.RotateX(Math.PI / 2, new Vector3(0, 0, 0));
            tris3.RotateY(Math.PI / 2, new Vector3(0, 0, 0));

            tris = tris + tris2 + tris3;
            tris.origin = new Vector3(0, 0, 100);

            int width = 230;
            int height = 60;
            Console.SetBufferSize(800, 800);
            Console.TreatControlCAsInput = true;

            double[,] depthBuffer = new double[height, width];
            Triangle3[,] triangleBuffer = new Triangle3[height, width];

            DateTime _lastTime = DateTime.Now; // marks the beginning the measurement began
            int _framesRendered = 0; // an increasing count
            int _fps; // the FPS calculated from the last measurement

            double xRotate = 0.1;
            double yRotate = 0.0;
            double zRotate = 0.05;

            while (true)
            {
                _framesRendered++;

                while (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D:
                            xRotate -= 0.02;
                            break;
                        case ConsoleKey.A:
                            yRotate += 0.02;
                            break;
                        case ConsoleKey.S:
                            xRotate += 0.02;
                            break;
                        case ConsoleKey.H:
                            yRotate -= 0.02;
                            break;
                    }
                }

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        depthBuffer[y, x] = double.MaxValue;
                        triangleBuffer[y, x] = null;
                        Ray3 ray = camera.Ray((double)x / width, (double)y / height);
                        foreach (Triangle3 triangle in tris.triangles)
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
                Dictionary<Triangle3, double> angleCache = new Dictionary<Triangle3, double>();
                string line = "";
                for (int y = 0; y < height; y++)
                {
                    if (y > 0)
                        line += "\n";
                    for (int x = 0; x < width; x++)
                    {
                        if (triangleBuffer[y, x] == null)
                        {
                            line += ' '; // light[random.Next(light.Count)];
                            continue;
                        }
                        double angle;
                        if (angleCache.ContainsKey(triangleBuffer[y, x]))
                            angle = angleCache[triangleBuffer[y, x]];
                        else
                        {
                            angle = Vector3.Angle(triangleBuffer[y, x].Normal(), camera.d.v);
                            angleCache.Add(triangleBuffer[y, x], angle);
                        }
                        switch (angle)
                        {
                            case double theta when theta <= 0.8:
                                line += '▓';
                                break;
                            case double theta when theta <= 1.25:
                                line += '▒';
                                break;
                            case double theta when theta <= 1.5:
                                line += '░';
                                break;
                            default:
                                line += ' ';
                                break;
                        }
                    }
                }
                System.Console.Write(line);

                if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
                {
                    // one second has elapsed 

                    _fps = _framesRendered;
                    _framesRendered = 0;
                    _lastTime = DateTime.Now;

                    System.Console.Write(_fps);
                }
                Console.SetCursorPosition(0, 0);
                tris.Rotate(xRotate, yRotate, zRotate, new Vector3(0, 0, 0));
            }
        }
    }
}
