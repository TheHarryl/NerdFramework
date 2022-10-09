namespace NerdFramework
{
    public class MeshQuad3 : Quad3
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;
        public Vector3 d;

        public Vector3 textureU;
        public Vector3 textureV;
        public Vector3 textureW;
        public Vector3 textureX;

        public Vector3 normalA;
        public Vector3 normalB;
        public Vector3 normalC;
        public Vector3 normalD;

        public MeshQuad3(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 textureU, Vector3 textureV, Vector3 textureW, Vector3 textureX, Vector3 normalA, Vector3 normalB, Vector3 normalC, Vector3 normalD) : base(a, b, c, d)
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
        }

        public MeshQuad3(Vector3 a, Vector3 b, Vector3 c, Vector3 d) : base(a, b, c, d)
        {
            this.textureU = Vector3.Zero;
            this.textureV = Vector3.Zero;
            this.textureW = Vector3.Zero;
            this.textureX = Vector3.Zero;

            this.normalA = Vector3.Zero;
            this.normalB = Vector3.Zero;
            this.normalC = Vector3.Zero;
            this.normalD = Vector3.Zero;
        }

        public new MeshTriangle3 GetTriangle1()
        {
            return new MeshTriangle3(a, b, c, textureU, textureV, textureW, normalA, normalB, normalC);
        }

        public new MeshTriangle3 GetTriangle2()
        {
            return new MeshTriangle3(a, c, d, textureU, textureW, textureX, normalA, normalC, normalD);
        }
    }
}
