using Math = System.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using NerdFramework;

namespace Mathi
{
    class Program
    {
        static void Main(string[] args)
        {
            //Random random = new Random();
            //List<char> light = new List<char> { ',', '\"', '^', '`', '\'', '.', '-', '~', '_' };
            Renderer3 renderer = new Renderer3(new Ray3Region(new Ray3(Vector3.Zero, Vector3.zAxis), 70, 35), 230, 60);
            Console.SetBufferSize(800, 800);
            Console.TreatControlCAsInput = true;


            //Triangle3Group tris = Triangle3Group.FromCube(Vector3.Zero, 20);
            Triangle3Group tris = Triangle3Group.FromIcophere(new Vector3(0, 0, 15), 15, 2);
            renderer.scene = tris;

            DateTime _lastTime = DateTime.Now; // marks the beginning the measurement began
            int _framesRendered = 0; // an increasing count
            int _fps; // the FPS calculated from the last measurement

            double xRotate = 0.001;
            double yRotate = 0.05;
            double zRotate = 0.0;

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

                renderer.RenderRaytraced();
                string line = "";
                for (int y = 0; y < renderer.height; y++)
                {
                    if (y > 0)
                        line += "\n";
                    for (int x = 0; x < renderer.width; x++)
                    {
                        line += ASCIIShader.FromAlpha(renderer.lightBuffer[y, x].Average() / 255.0);
                    }
                }
                System.Console.Write(line);

                if ((DateTime.Now - _lastTime).TotalSeconds >= 1)
                {
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
