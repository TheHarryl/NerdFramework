namespace NerdFramework
{
    public enum NormalType
    {
        Default,
        Interpolated
    }

    public class MeshTriangle3 : Triangle3
    {
        public Vector2 textureU;
        public Vector2 textureV;
        public Vector2 textureW;

        public Vector3 normalA;
        public Vector3 normalB;
        public Vector3 normalC;

        public NormalType normalType;
        public string material;

        public MeshTriangle3(Vector3 a, Vector3 b, Vector3 c) : base(a, b, c)
        {
            this.textureU = new Vector2(0.0, 0.0);
            this.textureV = new Vector2(0.0, 1.0);
            this.textureW = new Vector2(1.0, 1.0);

            this.normalA = Vector3.Zero;
            this.normalB = Vector3.Zero;
            this.normalC = Vector3.Zero;

            normalType = NormalType.Default;
            this.material = "None";
        }

        public MeshTriangle3(Vector3 a, Vector3 b, Vector3 c, Vector2 textureU, Vector2 textureV, Vector2 textureW, Vector3 normalA, Vector3 normalB, Vector3 normalC, NormalType normalType = NormalType.Interpolated, string material = "None") : base(a, b, c)
        {
            this.textureU = textureU;
            this.textureV = textureV;
            this.textureW = textureW;

            this.normalA = normalA;
            this.normalB = normalB;
            this.normalC = normalC;

            this.normalType = normalType;
            this.material = material;
        }

        public override void RotateX(double radians, Vector3 origin)
        {
            base.RotateX(radians, origin);

            normalA = normalA.RotateX(radians);
            normalB = normalB.RotateX(radians);
            normalC = normalC.RotateX(radians);
        }

        public override void RotateY(double radians, Vector3 origin)
        {
            base.RotateY(radians, origin);

            normalA = normalA.RotateY(radians);
            normalB = normalB.RotateY(radians);
            normalC = normalC.RotateY(radians);
        }

        public override void RotateZ(double radians, Vector3 origin)
        {
            base.RotateZ(radians, origin);

            normalA = normalA.RotateZ(radians);
            normalB = normalB.RotateZ(radians);
            normalC = normalC.RotateZ(radians);
        }

        public override void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            base.Rotate(r1, r2, r3, origin);

            normalA = normalA.Rotate(r1, r2, r3);
            normalB = normalB.Rotate(r1, r2, r3);
            normalC = normalC.Rotate(r1, r2, r3);
        }

        public Vector2 TextureCoordsAt(double t, double s)
        {
            return textureU * (1.0 - t - s) + textureV * t + textureW * s;
        }

        public Vector3 NormalAt(double t, double s)
        {
            return normalA * (1.0 - t - s) + normalB * t + normalC * s;
        }

        public static MeshTriangle3Collection operator +(MeshTriangle3 a, MeshTriangle3 b)
        {
            return new MeshTriangle3Collection(new System.Collections.Generic.List<MeshTriangle3>() { a, b });
        }
    }
}
