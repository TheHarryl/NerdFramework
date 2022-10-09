namespace NerdFramework
{
    public class MeshTriangle3 : Triangle3
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 textureU;
        public Vector3 textureV;
        public Vector3 textureW;

        public Vector3 normalA;
        public Vector3 normalB;
        public Vector3 normalC;

        public MeshTriangle3(Vector3 a, Vector3 b, Vector3 c) : base(a, b, c)
        {
            this.a = a;
            this.b = b;
            this.c = c;

            this.textureU = Vector3.Zero;
            this.textureV = Vector3.Zero;
            this.textureW = Vector3.Zero;

            this.normalA = Vector3.Zero;
            this.normalB = Vector3.Zero;
            this.normalC = Vector3.Zero;
        }

        public MeshTriangle3(Vector3 a, Vector3 b, Vector3 c, Vector3 textureU, Vector3 textureV, Vector3 textureW, Vector3 normalA, Vector3 normalB, Vector3 normalC) : base(a, b, c)
        {
            this.a = a;
            this.b = b;
            this.c = c;

            this.textureU = textureU;
            this.textureV = textureV;
            this.textureW = textureW;

            this.normalA = normalA;
            this.normalB = normalB;
            this.normalC = normalC;
        }

        public override void RotateX(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateX(radians);
            Vector3 OB = (b - origin).RotateX(radians);
            Vector3 OC = (c - origin).RotateX(radians);

            Vector3 OAn = (normalA - origin).RotateX(radians);
            Vector3 OBn = (normalB - origin).RotateX(radians);
            Vector3 OCn = (normalC - origin).RotateX(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;

            normalA = origin + OAn;
            normalB = origin + OBn;
            normalC = origin + OCn;
        }

        public override void RotateY(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateY(radians);
            Vector3 OB = (b - origin).RotateY(radians);
            Vector3 OC = (c - origin).RotateY(radians);

            Vector3 OAn = (normalA - origin).RotateY(radians);
            Vector3 OBn = (normalB - origin).RotateY(radians);
            Vector3 OCn = (normalC - origin).RotateY(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;

            normalA = origin + OAn;
            normalB = origin + OBn;
            normalC = origin + OCn;
        }

        public override void RotateZ(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateZ(radians);
            Vector3 OB = (b - origin).RotateZ(radians);
            Vector3 OC = (c - origin).RotateZ(radians);

            Vector3 OAn = (normalA - origin).RotateZ(radians);
            Vector3 OBn = (normalB - origin).RotateZ(radians);
            Vector3 OCn = (normalC - origin).RotateZ(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;

            normalA = origin + OAn;
            normalB = origin + OBn;
            normalC = origin + OCn;
        }

        public override void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            Vector3 OA = (a - origin).Rotate(r1, r2, r3);
            Vector3 OB = (b - origin).Rotate(r1, r2, r3);
            Vector3 OC = (c - origin).Rotate(r1, r2, r3);

            Vector3 OAn = (normalA - origin).Rotate(r1, r2, r3);
            Vector3 OBn = (normalB - origin).Rotate(r1, r2, r3);
            Vector3 OCn = (normalC - origin).Rotate(r1, r2, r3);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;

            normalA = origin + OAn;
            normalB = origin + OBn;
            normalC = origin + OCn;
        }

        public Vector3 TextureCoordsAt(double t, double s)
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
