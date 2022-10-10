namespace NerdFramework
{
    public class RasterizedTriangle2 : Triangle2
    {
        public Color3 colorA;
        public Color3 colorB;
        public Color3 colorC;

        public double distA;
        public double distB;
        public double distC;

        public Material material;

        public RasterizedTriangle2(Vector2 a, Vector2 b, Vector2 c, Color3 colorA, Color3 colorB, Color3 colorC, double distA, double distB, double distC, Material material) : base(a, b, c)
        {
            /* Triangle:
             * a: point1
             * b: point2
             * c: point3
             */

            this.colorA = colorA;
            this.colorB = colorB;
            this.colorC = colorC;

            this.distA = distA;
            this.distB = distB;
            this.distC = distC;

            this.material = material;
        }

        public Color3 ColorAt(double t, double s)
        {
            return colorA * (1.0 - t - s) + colorB * t + colorC * s;
        }
        public double DistanceAt(double t, double s)
        {
            return distA * (1.0 - t - s) + distB * t + distC * s;
        }
    }
}
