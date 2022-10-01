using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Light3Caster
    {
        public Ray3Caster rayCaster;
        public Triangle3Group origin;
        public Vector3 offset;

        public Color3 color;
        public double distance;

        public Light3Caster(Ray3Caster caster, Triangle3Group origin, Vector3 offset, Color3 color, double distance)
        {
            this.rayCaster = caster;
            this.origin = origin;
            this.offset = offset;
            this.color = color;
            this.distance = distance;
        }
    }
}
