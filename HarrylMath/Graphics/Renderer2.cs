using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Renderer2
    {
        private int _width;
        private int _height;
        public Color3[,] pixelBuffer;
        public Color3[,] lightBuffer;
        public double[,] luminanceBuffer;
        public double[,] opacityBuffer;

        public Renderer2(int width, int height)
        {
            _width = width;
            _height = height;

            pixelBuffer = new Color3[height, width];
        }

        public void Render()
        {

        }
    }
}
