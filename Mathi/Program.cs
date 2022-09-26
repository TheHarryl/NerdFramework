using Math = System.Math;
using Ray3Region = NerdEngine.Ray3Region;
using Ray3Sector = NerdEngine.Ray3Sector;
using Ray3 = NerdEngine.Ray3;
using Vector3 = NerdEngine.Vector3;
using Triangle3 = NerdEngine.Triangle3;
using Triangle3Group = NerdEngine.Triangle3Group;
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
            List<string> light = new List<string> { ",", "\"", "^", "`", "'", ".", "-", "~", "_" };
            List<string> dark = new List<string> { "▓" };

            Ray3Region camera = new Ray3Region(new Ray3(Vector3.Zero, Vector3.zAxis), 20, 35);
            //Ray3Sector camera = new Ray3Sector(new Ray3(new Vector3(0,0,-10), Vector3.zAxis), 2, (1080/1920)*2.0);

            Triangle3Group tris = (
                new Triangle3(new Vector3(-10, 10, 10), new Vector3(-10, -10, 10), new Vector3(10, -10, 10)) +
                new Triangle3(new Vector3(10, -10, 10), new Vector3(10, 10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, -10, 30), new Vector3(-10, -10, 30), new Vector3(-10, 10, 30)) +
                new Triangle3(new Vector3(-10, 10, 30), new Vector3(10, 10, 30), new Vector3(10, -10, 30))
            );

            Triangle3Group tris2 = new Triangle3(new Vector3(-10, 10, 10), new Vector3(-10, -10, 10), new Vector3(10, -10, 10)) +
                new Triangle3(new Vector3(10, -10, 10), new Vector3(10, 10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, -10, 30), new Vector3(-10, -10, 30), new Vector3(-10, 10, 30)) +
                new Triangle3(new Vector3(-10, 10, 30), new Vector3(10, 10, 30), new Vector3(10, -10, 30));

            Triangle3Group tris3 = new Triangle3(new Vector3(-10, 10, 10), new Vector3(-10, -10, 10), new Vector3(10, -10, 10)) +
                new Triangle3(new Vector3(10, -10, 10), new Vector3(10, 10, 10), new Vector3(-10, 10, 10)) +
                new Triangle3(new Vector3(10, -10, 30), new Vector3(-10, -10, 30), new Vector3(-10, 10, 30)) +
                new Triangle3(new Vector3(-10, 10, 30), new Vector3(10, 10, 30), new Vector3(10, -10, 30));

            tris2.RotateX(Math.PI/2, new Vector3(0, 0, 20));
            tris3.RotateY(Math.PI/2, new Vector3(0, 0, 20));

            tris = tris + tris2 + tris3;

            tris.origin += new Vector3(30, 0, 0);

            double width = 230;
            double height = 60;
            Console.SetBufferSize(800, 800);

            while (true)
            {
                string line = "";
                for (int y = 0; y < height; y++)
                {
                    if (y > 0)
                        line += "\n";
                    for (int x = 0; x < width; x++)
                    {
                        Ray3 ray = camera.Ray(x / height, y / height);
                        tris.triangles = tris.triangles.OrderBy(t => Vector3.Angle(t.Normal(), ray.v)).ToList();
                        if (tris.Meets(ray)) {
                            if (tris.Angle(ray) <= 0.8)
                                line += dark[random.Next(dark.Count)];
                            else if (tris.Angle(ray) <= 1.25)
                                line += "▒";
                            else if (tris.Angle(ray) <= 1.5)
                                line += "░";
                            else
                                line += " ";
                        } else
                            line += light[random.Next(light.Count)];
                    }
                }
                System.Console.Write(line);
                Console.SetCursorPosition(0, 0);
                System.Threading.Thread.Sleep(0);
                tris.Rotate(0.1, 0.0, 0.05, new Vector3(30, 0, 20));
            }
        }
    }
}
