namespace HarrylMath
{
    public class Quadratic1
    {
        readonly double? a;
        readonly double? b;
        readonly double? c;

        public Quadratic1(double? a, double? b, double? c)
        {
            // Ax^2 + Bx + C

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public double Equals(double x)
        {
            return (double)a*x*x + (double)b*x + (double)c;
        }
    }
}
