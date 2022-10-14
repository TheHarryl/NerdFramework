using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Texture2
    {
        public int width { get; private set; }
        public int height { get; private set; }

        private Color3[,] _data;
        public Color3[,] data
        {
            get => _data;
            set
            {
                _data = value;
                width = data.GetLength(1);
                height = data.GetLength(0);
            }
        }

        public static Texture2 None = new Texture2(new Color3[1, 1] { { Color3.None } });
        public static Texture2 White = new Texture2(new Color3[1, 1] { { Color3.White } });
        public static Texture2 Black = new Texture2(new Color3[1, 1] { { Color3.Black } });

        public Texture2(Color3[,] data)
        {
            this.data = data;
        }

        public Color3 ColorAt(double t, double s)
        {
            t -= Math.Floor(t);
            s -= Math.Floor(s);

            int x = (int)(t * width);
            int y = (int)(s * height);

            return data[y, x];
        }
        public Color3 ColorAt(Vector2 parameters) { return ColorAt(parameters.x, parameters.y); }
    }
}
