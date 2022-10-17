using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdFramework
{
    public class InterfaceEngine
    {
        public List<UIObject> scene;

        private int _width;
        private int _height;
        public int width
        {
            get => _width;
            set
            {
                _width = value;
                pixelBuffer = new Color3[_height, _width];
            }
        }
        public int height
        {
            get => _height;
            set
            {
                _height = value;
                pixelBuffer = new Color3[_height, _width];
            }
        }

        public Color3[,] pixelBuffer;

        public InterfaceEngine(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Update(double delta)
        {
            scene = scene.OrderByDescending(obj => obj.zindex).ToList();

            foreach (UIObject obj in scene)
            {
                obj.Update(delta);
            }
        }

        public void Draw()
        {
            scene = scene.OrderByDescending(obj => obj.zindex).ToList();

            foreach (UIObject obj in scene)
            {
                obj.Draw(pixelBuffer);
            }
        }
    }
}
