using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TriangleRenderInfo
    {
        public Vector2 textureU;
        public Vector2 textureV;
        public Vector2 textureW;

        public Vector3 normalA;
        public Vector3 normalB;
        public Vector3 normalC;

        public NormalType normalType;
        public string material;

        public TriangleRenderInfo(Vector2 textureU, Vector2 textureV, Vector2 textureW, Vector3 normalA, Vector3 normalB, Vector3 normalC, NormalType normalType = NormalType.Interpolated, string material = "None")
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
    }
}
