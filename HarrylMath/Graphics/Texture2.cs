using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Texture2
    {
        public Color3[,] data;

        public static Texture2 None = new Texture2(new Color3[1, 1] { { Color3.None } });

        public Texture2(Color3[,] data)
        {
            this.data = data;
        }

        public Color3 ColorAt(double t, double s)
        {
            int tMin = Math.Floor(t);
            int tMax = Math.Ceil(t);
            int sMin = Math.Floor(s);
            int sMax = Math.Ceil(s);

            double tMinWeight = t - tMin; 
            double tMaxWeight = tMax - t;
            double sMinWeight = s - sMin;
            double sMaxWeight = sMax - s;

            return data[sMin, tMin] * (2 - (sMinWeight + tMinWeight)) + data[sMax, tMax] * (2 - (sMaxWeight + tMaxWeight));
        }
        public Color3 ColorAt(Vector2 parameters) { return ColorAt(parameters.x, parameters.y); }
    }
}
