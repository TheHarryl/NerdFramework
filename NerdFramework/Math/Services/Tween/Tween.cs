namespace NerdFramework
{
    public static class Tween
    {
        // https://www.desmos.com/calculator/m8myals511

        public static double Linear(double x1, double x2, double interpolant)
        {
            if (interpolant < 0) interpolant = 0;
            else if (interpolant > 1) interpolant = 1;

            return x1 + (x2 - x1) * interpolant;
        }

        public static double SineIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Sin(Math.PI * interpolant / 2 - Math.PI / 2) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double SineOut(double x1, double x2, double interpolant)
        {
            interpolant = Math.Sin(Math.PI * interpolant / 2);
            return Linear(x1, x2, interpolant);
        }
        public static double SineInOut(double x1, double x2, double interpolant)
        {
            interpolant = Math.Sin(Math.PI * interpolant - Math.PI / 2) / 2 + 0.5;
            return Linear(x1, x2, interpolant);
        }
        public static double SineOutIn(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? Math.Sin(Math.PI * interpolant) / 2 : Math.Sin(Math.PI * interpolant - Math.PI) / 2 + 1;
            return Linear(x1, x2, interpolant);
        }

        public static double BackIn(double x1, double x2, double interpolant)
        {
            double s = 1.70158;
            interpolant = interpolant * interpolant * (interpolant * (s + 1) - s);
            return Linear(x1, x2, interpolant);
        }
        public static double BackOut(double x1, double x2, double interpolant)
        {
            double s = 1.70158;
            interpolant = Math.Pow(interpolant - 1, 2.0) * ((interpolant - 1) * (s + 1) + s) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double BackInOut(double x1, double x2, double interpolant)
        {
            double s1 = 2.5949095;
            interpolant = interpolant <= 0.5 ? 2 * interpolant * interpolant * (2 * interpolant * (s1 + 1) - s1) : 0.5 * Math.Pow(2 * interpolant - 2, 2.0) * ((2 * interpolant - 2) * (s1 + 1) + s1) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double BackOutIn(double x1, double x2, double interpolant)
        {
            double s1 = 2.5949095;
            interpolant = 0.5 * Math.Pow(2 * interpolant - 1, 2.0) * ((2 * interpolant - 1) * (s1 + 1) + s1 * (interpolant <= 0.5 ? 1 : -1)) + 0.5;
            return Linear(x1, x2, interpolant);
        }

        public static double QuadIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant, 2.0);
            return Linear(x1, x2, interpolant);
        }
        public static double QuadOut(double x1, double x2, double interpolant)
        {
            interpolant = -Math.Pow(interpolant - 1, 2.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuadInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 2 * Math.Pow(interpolant, 2.0) : -2 * Math.Pow(interpolant - 1, 2.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuadOutIn(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? -2 * Math.Pow(interpolant - 0.5, 2.0) + 0.5 : 2 * Math.Pow(interpolant - 0.5, 2.0) + 0.5;
            return Linear(x1, x2, interpolant);
        }

        public static double QuartIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant, 4.0);
            return Linear(x1, x2, interpolant);
        }
        public static double QuartOut(double x1, double x2, double interpolant)
        {
            interpolant = -Math.Pow(interpolant - 1, 4.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuartInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 8 * Math.Pow(interpolant, 4.0) : -8 * Math.Pow(interpolant - 1, 4.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuartOutIn(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? -8 * Math.Pow(interpolant - 0.5, 4.0) + 0.5 : 8 * Math.Pow(interpolant - 0.5, 4.0) + 0.5;
            return Linear(x1, x2, interpolant);
        }

        public static double QuintIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant, 5.0);
            return Linear(x1, x2, interpolant);
        }
        public static double QuintOut(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant - 1, 5.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuintInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 16 * Math.Pow(interpolant, 5.0) : 16 * Math.Pow(interpolant - 1, 5.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double QuintOutIn(double x1, double x2, double interpolant)
        {
            interpolant = 16 * Math.Pow(interpolant - 0.5, 5.0) + 0.5;
            return Linear(x1, x2, interpolant);
        }

        public static double BounceIn(double x1, double x2, double interpolant)
        {
            if (interpolant <= 0.25 / 2.75)
                interpolant = -7.5625 * Math.Pow(1.0 - interpolant - 2.625 / 2.75, 2.0) + 0.015625;
            else if (interpolant <= 0.75 / 2.75)
                interpolant = -7.5625 * Math.Pow(1.0 - interpolant - 2.25 / 2.75, 2.0) + 0.0625;
            else if (interpolant <= 1.75 / 2.75)
                interpolant = -7.5625 * Math.Pow(1.0 - interpolant - 1.5 / 2.75, 2.0) + 0.25;
            else
                interpolant = 1 - 7.5625 * Math.Pow(1.0 - interpolant, 2.0);
            return Linear(x1, x2, interpolant);
        }
        public static double BounceOut(double x1, double x2, double interpolant)
        {
            if (interpolant <= 1.0 / 2.75)
                interpolant = 7.5625 * interpolant * interpolant;
            else if (interpolant <= 2.0 / 2.75)
                interpolant = 7.5625 * Math.Pow(interpolant - 1.5 / 2.75, 2.0) + 0.75;
            else if (interpolant <= 2.5 / 2.75)
                interpolant = 7.5625 * Math.Pow(interpolant - 2.25 / 2.75, 2.0) + 0.9375;
            else
                interpolant = 7.5625 * Math.Pow(interpolant - 2.625 / 2.75, 2.0) + 0.984375;
            return Linear(x1, x2, interpolant);
        }
        public static double BounceInOut(double x1, double x2, double interpolant)
        {
            return Linear(x1, x2, interpolant);
        }
        public static double BounceOutIn(double x1, double x2, double interpolant)
        {
            return Linear(x1, x2, interpolant);
        }

        public static double ElasticIn(double x1, double x2, double interpolant)
        {
            double p = 0.3;
            interpolant = -Math.Pow(2.0, 10.0 * (interpolant - 1.0)) * Math.Sin(2.0 * Math.PI * (interpolant - 1.0 - p / 4.0) / p);
            return Linear(x1, x2, interpolant);
        }
        public static double ElasticOut(double x1, double x2, double interpolant)
        {
            double p = 0.3;
            interpolant = Math.Pow(2.0, -10.0 * interpolant) * Math.Sin(2.0 * Math.PI * (interpolant - p / 4.0) / p) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double ElasticInOut(double x1, double x2, double interpolant)
        {
            double p1 = 0.45;
            interpolant = interpolant <= 0.5 ? -0.5 * Math.Pow(2.0, 20.0 * interpolant - 10.0) * Math.Sin(2.0 * Math.PI * (2.0 * interpolant - 1.1125) / p1) : 0.5 * Math.Pow(2.0, -20.0 * interpolant + 10.0) * Math.Sin(2.0 * Math.PI * (2.0 * interpolant - 1.1125) / p1) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double ElasticOutIn(double x1, double x2, double interpolant)
        {
            double p1 = 0.45;
            interpolant = interpolant <= 0.5 ? 0.5 * Math.Pow(2.0, -20.0 * interpolant) * Math.Sin(2.0 * Math.PI * (2.0 * interpolant - p1 / 4.0) / p1) + 0.5 : -0.5 * Math.Pow(2.0, 10.0 * (2.0 * interpolant - 2.0)) * Math.Sin(2.0 * Math.PI * (2.0 * interpolant - 2.0 - p1 / 4.0) / p1) + 0.5;
            return Linear(x1, x2, interpolant);
        }

        public static double ExponentialIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(2.0, 10.0 * interpolant - 10.0) - 0.001;
            return Linear(x1, x2, interpolant);
        }
        public static double ExponentialOut(double x1, double x2, double interpolant)
        {
            interpolant = -1.001 * Math.Pow(2.0, -10.0 * interpolant) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double ExponentialInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 0.5 * Math.Pow(2.0, 20.0 * interpolant - 10.0) - 0.0005 : 0.50025 * -Math.Pow(2.0, -20.0 * interpolant + 10.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double ExponentialOutIn(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 0.5005 * -Math.Pow(2.0, -20.0 * interpolant) + 0.5005 : 0.5 * Math.Pow(2.0, 10.0 * (2.0 * interpolant - 2.0)) + 0.4995;
            return Linear(x1, x2, interpolant);
        }

        public static double CircularIn(double x1, double x2, double interpolant)
        {
            interpolant = -Math.Pow(1.0 - Math.Pow(interpolant, 2.0), 0.5) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double CircularOut(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(-Math.Pow(interpolant - 1.0, 2.0) + 1.0, 0.5);
            return Linear(x1, x2, interpolant);
        }
        public static double CircularInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? -Math.Pow(-Math.Pow(interpolant, 2.0) + 0.25, 0.5) + 0.5 : Math.Pow(-Math.Pow(interpolant - 1.0, 2.0) + 0.25, 0.5) + 0.5;
            return Linear(x1, x2, interpolant);
        }
        public static double CircularOutIn(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? Math.Pow(-Math.Pow(interpolant - 0.5, 2.0) + 0.25, 0.5) : -Math.Pow(-Math.Pow(interpolant - 0.5, 2.0) + 0.25, 0.5) + 1;
            return Linear(x1, x2, interpolant);
        }

        public static double CubicIn(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant, 3.0);
            return Linear(x1, x2, interpolant);
        }
        public static double CubicOut(double x1, double x2, double interpolant)
        {
            interpolant = Math.Pow(interpolant - 1, 3.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double CubicInOut(double x1, double x2, double interpolant)
        {
            interpolant = interpolant <= 0.5 ? 4 * Math.Pow(interpolant, 3.0) : 4 * Math.Pow(interpolant - 1, 3.0) + 1;
            return Linear(x1, x2, interpolant);
        }
        public static double CubicOutIn(double x1, double x2, double interpolant)
        {
            interpolant = 4 * Math.Pow(interpolant - 0.5, 3.0) + 0.5;
            return Linear(x1, x2, interpolant);
        }
    }
}
