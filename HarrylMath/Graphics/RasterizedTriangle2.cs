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

        public Vector2 textureU;
        public Vector2 textureV;
        public Vector2 textureW;

        public Material material;

        public RasterizedTriangle2(Vector2 a, Vector2 b, Vector2 c, Color3 colorA, Color3 colorB, Color3 colorC, double distA, double distB, double distC, Vector2 textureU, Vector2 textureV, Vector2 textureW, Material material) : base(a, b, c)
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

            this.textureU = textureU;
            this.textureV = textureV;
            this.textureW = textureW;

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
        public Vector2 TextureCoordsAt(double t, double s)
        {
            return textureU * (1.0 - t - s) + textureV * t + textureW * s;
        }
    }
}
