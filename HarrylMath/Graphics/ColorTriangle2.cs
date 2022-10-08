namespace NerdFramework
{
    public class ColorTriangle2 : Triangle2
    {
        public Color3 colorA;
        public Color3 colorB;
        public Color3 colorC;

        public ColorTriangle2(Vector2 a, Vector2 b, Vector2 c, Color3 colorA, Color3 colorB, Color3 colorC) : base(a, b, c)
        {
            /* Triangle:
             * a: point1
             * b: point2
             * c: point3
             */

            this.colorA = colorA;
            this.colorB = colorB;
            this.colorC = colorC;
        }

        public Color3 ColorAt(double t, double s)
        {
            return colorA * (1 - t - s) + colorB * t + colorC * s;
        }
    }
}
