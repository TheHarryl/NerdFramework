namespace HarrylMath
{
    public class Quadratic2
    {
        readonly double? a;
        readonly double? b;
        readonly double? c;
        readonly double? d;
        readonly double? e;
        readonly double? f;
        
        public Quadratic2(double? a, double? b, double? c, double? d, double? e, double? f)
        {
            // Ax^2 + By^2 + Cxy + Dx + Ey + F

            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
            this.f = f;
        }

        public double Equals(double x, double y)
        {
            return (double)a*x*x + (double)b*y*y + (double)c*x*y + (double)d*x + (double)e*y + (double)f;
        }
    }
}
