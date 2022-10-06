using System.Linq;

namespace NerdFramework
{
    public class Light3Caster
    {
        public Ray3Caster rayCaster;

        public Triangle3Group reference;

        public Color3Sequence color;

        public double distance;
        public double volumentricCoefficient;

        public Light3Caster(Ray3Caster caster, Triangle3Group reference, Color3Sequence color, double distance, double volumentricCoefficient = 0.1)
        {
            this.rayCaster = caster;
            this.reference = reference;
            this.color = color;
            this.distance = distance;
            this.volumentricCoefficient = volumentricCoefficient;
        }

        public void Quantize(int quantization = 5)
        {
            Color3 first = color.steps.Values.First();
            Color3 last = color.steps.Values.Last();

            for (int i = 1; i < quantization; i++)
            {
                double stepInterval0 = Tween.QuartIn(0, 1, (i-1) / quantization);
                double stepInterval1 = Tween.QuartIn(0, 1, i / quantization);
                color.steps.Add(stepInterval1, Color3.Lerp(first, last, stepInterval0));
                color.steps.Add(stepInterval1 + 0.01, Color3.Lerp(first, last, stepInterval1));
            }
        }

        public Color3 LightAt(double distance, double angle)
        {
            double interpolant = angle / Math.QuarterPI;

            Color3 light = color.ColorAt(distance / this.distance);
            light = Color3.Lerp(light, Color3.Black, interpolant > 1.0 ? 1.0 : interpolant);
            return light;
        }

        public bool InRange(Vector3 point)
        {
            return (point - this.reference.origin).Magnitude() <= distance;
        }
    }
}
