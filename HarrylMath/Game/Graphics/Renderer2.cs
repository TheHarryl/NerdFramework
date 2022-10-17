using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
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

        public Renderer2(int width, int height)
        {
            _width = width;
            _height = height;

            lightBuffer = new Color3[height, width];
        }

        public void Render()
        {

        }
    }
}
