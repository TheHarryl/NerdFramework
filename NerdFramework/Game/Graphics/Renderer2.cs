using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdFramework
{
    public enum CPUMode
    {
        SingleThreaded,
        MultiThreaded
    }

    public class Renderer2
    {
        protected int _width = 1;
        protected int _height = 1;
        public int width
        {
            get => _width;
            set
            {
                _width = value;
                lightBuffer = new Color3[_height, _width];
            }
        }
        public int height
        {
            get => _height;
            set
            {
                _height = value;
                lightBuffer = new Color3[_height, _width];
            }
        }
        public Color3[,] lightBuffer;

        public CPUMode CPUMode;
        public ParallelOptions parallelOptions = new ParallelOptions();

        public ulong frameNum { get; private set; }

        public Renderer2(int width, int height)
        {
            this.width = width;
            this.height = height;

            this.lightBuffer = new Color3[height, width];

            this.CPUMode = CPUMode.MultiThreaded;
            this.parallelOptions.MaxDegreeOfParallelism = 10;
        }

        public void Clear()
        {
            Parallel.For(0, _height, parallelOptions, y =>
            {
                for (int x = 0; x < _width; x++)
                {
                    lightBuffer[y, x] = Color3.None;
                }
            });
        }

        public void Render()
        {
            frameNum++;
        }
    }
}
