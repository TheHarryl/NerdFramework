using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    public class Color3Sequence
    {
        public Dictionary<double, Color3> steps = new Dictionary<double, Color3>();

        public Color3Sequence()
        {
            steps.Add(0.0, Color3.White);
        }

        public Color3Sequence(Dictionary<double, Color3> steps)
        {
            this.steps = steps;
        }

        public Color3Sequence(Color3 color)
        {
            steps.Add(0.0, color);
            steps.Add(1.0, Color3.Black);
        }

        public Color3Sequence(params Color3[] colors)
        {
            double stepInterval = 1.0 / (colors.Length - 1);
            for (int i = 0; i < colors.Length; i++)
            {
                steps.Add(stepInterval * i, colors[i]);
            }
        }

        public Color3Sequence Quantized(int quantization = 5)
        {
            Dictionary<double, Color3> quantizedSteps = new Dictionary<double, Color3>();
            Color3 first = steps.Values.First();
            Color3 last = steps.Values.Last();

            for (int i = 1; i < quantization; i++)
            {
                double stepInterval0 = Tween.QuartIn(0, 1, (i - 1) / quantization);
                double stepInterval1 = Tween.QuartIn(0, 1, i / quantization);
                quantizedSteps.Add(stepInterval1, Color3.Lerp(first, last, stepInterval0));
                quantizedSteps.Add(stepInterval1 + 0.01, Color3.Lerp(first, last, stepInterval1));
            }

            return new Color3Sequence(quantizedSteps);
        }

        public Color3 ColorAt(double interpolant)
        {
            KeyValuePair<double, Color3>? step0 = null;
            KeyValuePair<double, Color3>? step1 = null;
            foreach (KeyValuePair<double, Color3> step in steps)
            {
                if (step.Key >= interpolant)
                {
                    step1 = step;
                    break;
                }
            }
            if (step1 == null)
                return steps.Values.Last();
            foreach (KeyValuePair<double, Color3> step in steps.Reverse())
            {
                if (step.Key < step1.Value.Key)
                {
                    step0 = step;
                    break;
                }
            }
            if (step0 == null)
                return steps.Values.First();

            return Color3.Lerp(step0.Value.Value, step1.Value.Value, (interpolant - step0.Value.Key) / (step1.Value.Key - step0.Value.Key));
        }
    }
}
