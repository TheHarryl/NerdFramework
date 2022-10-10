namespace NerdFramework
{
    public class MeshQuad3 : Quad3
    {
        public Vector2 textureU;
        public Vector2 textureV;
        public Vector2 textureW;
        public Vector2 textureX;

        public Vector3 normalA;
        public Vector3 normalB;
        public Vector3 normalC;
        public Vector3 normalD;

        public NormalType normalType;

        public MeshQuad3(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector2 textureU, Vector2 textureV, Vector2 textureW, Vector2 textureX, Vector3 normalA, Vector3 normalB, Vector3 normalC, Vector3 normalD) : base(a, b, c, d)
        {
           /* A ----- D
            * |       |
            * |       |
            * |       |
            * B ----- C
            * 
            * Normal points out of the page
            */

            this.textureU = textureU;
            this.textureV = textureV;
            this.textureW = textureW;
            this.textureX = textureX;

            this.normalA = normalA;
            this.normalB = normalB;
            this.normalC = normalC;
            this.normalD = normalD;

            this.normalType = NormalType.Interpolated;
        }

        public MeshQuad3(Vector3 a, Vector3 b, Vector3 c, Vector3 d) : base(a, b, c, d)
        {
            this.textureU = new Vector2(0.0, 0.0);
            this.textureV = new Vector2(0.0, 1.0);
            this.textureW = new Vector2(1.0, 1.0);
            this.textureX = new Vector2(1.0, 0.0);

            this.normalA = Vector3.Zero;
            this.normalB = Vector3.Zero;
            this.normalC = Vector3.Zero;
            this.normalD = Vector3.Zero;

            this.normalType = NormalType.Default;
        }

        public new MeshTriangle3 GetTriangle1()
        {
            return new MeshTriangle3(a, b, c, textureU, textureV, textureW, normalA, normalB, normalC, normalType);
        }

        public new MeshTriangle3 GetTriangle2()
        {
            return new MeshTriangle3(a, c, d, textureU, textureW, textureX, normalA, normalC, normalD, normalType);
        }
    }
}
