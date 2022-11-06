using NerdFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Math = NerdFramework.Math;

namespace NerdPanel
{
    public class Grapher : BaseTab
    {
        public Grapher(params string[] args) : base(args)
        {

        }

        public Vector2i position = new Vector2i(60, 120);
        public int gridSize = 35;

        public override void Update(InterfaceEngine engine, double delta)
        {

        }

        public override void Draw(InterfaceEngine renderer)
        {
            Parallel.For(0, renderer.height, renderer.parallelOptions, y =>
            {
                for (int x = 0; x < renderer.width; x++)
                {
                    Vector2i gridPos = new Vector2i(renderer.width / 2, renderer.height / 2) - new Vector2i(x, y) + position;

                    if (gridPos.x == 0 || gridPos.x == 1 || gridPos.y == 0 || gridPos.y == 1)
                        renderer.lightBuffer[y, x] = new Color3(180, 180, 180);
                    else if (gridPos.x % (gridSize * 4) == 0 || gridPos.y % (gridSize * 4) == 0)
                        renderer.lightBuffer[y, x] = new Color3(140, 140, 140);
                    else if (gridPos.x % gridSize == 0 || gridPos.y % gridSize == 0)
                        renderer.lightBuffer[y, x] = new Color3(80, 80, 80);
                    else
                        renderer.lightBuffer[y, x] = new Color3(30, 30, 30);
                }
            });

            Parallel.For(1, renderer.width-1, renderer.parallelOptions, x =>
            {
                double virtualX = (renderer.width / 2) - x + position.x;
                virtualX *= 0.5 / gridSize;

                double targetY = virtualX * virtualX;

                double targetYAbsolute = -(targetY / (0.5 / gridSize) - position.y - (renderer.height / 2));

                for (int y = 0; y < renderer.height; y++)
                {
                    double dist = Math.Abs(y - targetYAbsolute);
                    if (dist <= 1) {
                        renderer.lightBuffer[y, x] = Color3.LightRed;
                        renderer.lightBuffer[y, x+1] = Color3.LightRed;
                        renderer.lightBuffer[y, x-1] = Color3.LightRed;
                    } else if (dist <= 2.5)
                    {
                        renderer.lightBuffer[y, x] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y, x], (dist - 1.0) / 1.5);
                        renderer.lightBuffer[y, x + 1] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y, x + 1], (dist - 1.0) / 1.5);
                        renderer.lightBuffer[y, x - 1] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y, x - 1], (dist - 1.0) / 1.5);
                    }
                }
            });

            Parallel.For(1, renderer.height-1, renderer.parallelOptions, y =>
            {
                double virtualY = (renderer.height / 2) - y + position.y;
                virtualY *= 0.5 / gridSize;

                double targetX = Math.Sqrt(virtualY);

                double targetXAbsolute1 = -(targetX / (0.5 / gridSize) - position.x - (renderer.width / 2));
                double targetXAbsolute2 = -(-targetX / (0.5 / gridSize) - position.x - (renderer.width / 2));

                for (int x = 0; x < renderer.width; x++)
                {
                    double dist1 = Math.Abs(x - targetXAbsolute1);
                    double dist2 = Math.Abs(x - targetXAbsolute2);
                    if (dist1 <= 1 || dist2 <= 1)
                    {
                        renderer.lightBuffer[y, x] = Color3.LightRed;
                        renderer.lightBuffer[y + 1, x] = Color3.LightRed;
                        renderer.lightBuffer[y - 1, x] = Color3.LightRed;
                    } else if (dist1 <= 2.5 || dist2 <= 2.5)
                    {
                        double dist = Math.Min(dist1, dist2);
                        renderer.lightBuffer[y, x] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y, x], (dist - 1.0) / 1.5);
                        renderer.lightBuffer[y + 1, x] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y + 1, x], (dist - 1.0) / 1.5);
                        renderer.lightBuffer[y - 1, x] = Color3.Lerp(Color3.LightRed, renderer.lightBuffer[y - 1, x], (dist - 1.0) / 1.5);
                    }
                }
            });
        }
    }
}
