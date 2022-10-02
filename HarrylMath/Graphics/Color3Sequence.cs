using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    public class Color3Sequence
    {
        Dictionary<double, Color3> steps = new Dictionary<double, Color3>();

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
        }

        public Color3Sequence(params Color3[] colors)
        {
            double stepInterval = 1.0 / (colors.Length - 1);
            for (int i = 0; i < colors.Length; i++)
            {
                steps.Add(stepInterval * i, colors[i]);
            }
        }

        public Color3 ColorAt(double interpolant)
        {
            KeyValuePair<double, Color3>? step1 = steps.First(x => x.Key >= interpolant);
            if (step1 == null)
                return steps.Values.Last();
            KeyValuePair<double, Color3>? step0 = steps.First(x => x.Key < step1.Value.Key);
            if (step0 == null)
                return steps.Values.First();

            return Color3.Lerp(step0.Value.Value, step1.Value.Value, (interpolant - step0.Value.Key) / (step1.Value.Key - step0.Value.Key));
        }
    }
}
