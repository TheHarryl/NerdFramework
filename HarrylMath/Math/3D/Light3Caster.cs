using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Light3Caster
    {
        public Ray3Caster rayCaster;
        public Triangle3Group origin;
        public Color3Sequence color;

        public double distance;
        public int quantization;
        public double dustCoefficient;

        public Light3Caster(Ray3Caster caster, Triangle3Group origin, Color3Sequence color, double distance, int quantization = 3, double dustCoefficient = 0.1)
        {
            this.rayCaster = caster;
            this.origin = origin;
            this.color = color;
            this.distance = distance;
            this.quantization = quantization;
            this.dustCoefficient = dustCoefficient;
        }
    }
}
