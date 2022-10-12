using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Texture2
    {
        public Color3[,] data;

        public static Texture2 None = new Texture2(new Color3[1, 1] { { Color3.None } });
        public static Texture2 One = new Texture2(new Color3[1, 1] { { Color3.White } });

        public Texture2(Color3[,] data)
        {
            this.data = data;
        }

        public Color3 ColorAt(double t, double s)
        {
            t %= 1;
            s %= 1;
            if (t < 0) t++;
            if (s < 0) s++;

            int x = (int)(t * data.GetLength(1));
            int y = (int)(s * data.GetLength(0));

            return data[y, x];
            /*int tMin = Math.Floor(t);
            int tMax = Math.Ceil(t);
            int sMin = Math.Floor(s);
            int sMax = Math.Ceil(s);

            double tMinWeight = t - tMin; 
            double tMaxWeight = tMax - t;
            double sMinWeight = s - sMin;
            double sMaxWeight = sMax - s;

            //System.Diagnostics.Trace.WriteLine(data.Length);
            return Color3.White;//data[sMin, tMin] * (2 - (sMinWeight + tMinWeight)) + data[sMax, tMax] * (2 - (sMaxWeight + tMaxWeight));*/
        }
        public Color3 ColorAt(Vector2 parameters) { return ColorAt(parameters.x, parameters.y); }
    }
}
