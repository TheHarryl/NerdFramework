using System.Linq;

namespace NerdFramework
{
    public class Light3Caster
    {
        public Ray3Caster rayCaster;

        public Triangle3Collection reference;

        public Color3Sequence color;

        public double distance;
        public double volumentricCoefficient;

        public Light3Caster(Ray3Caster caster, Triangle3Collection reference, Color3Sequence color, double distance, double volumentricCoefficient = 0.1)
        {
            this.rayCaster = caster;
            this.reference = reference;
            this.color = color;
            this.distance = distance;
            this.volumentricCoefficient = volumentricCoefficient;
        }

        public Color3 LightAt(double distance, double angle)
        {
            double interpolant = angle / Math.HalfPI;

            Color3 light = color.ColorAt(distance / this.distance);
            light = Color3.Lerp(light, Color3.None, interpolant > 1.0 ? 1.0 : interpolant);
            return light;
        }

        public bool InRange(Vector3 point)
        {
            return (point - this.reference.origin).Magnitude() <= distance;
        }
    }
}
