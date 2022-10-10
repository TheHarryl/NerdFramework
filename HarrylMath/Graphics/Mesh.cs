using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Mesh
    {
        public MeshTriangle3Collection polygons;
        public List<BoundingShape3> colliders;
        public List<Bone> bones;
        public Color3[,] texture;

        public Mesh(MeshTriangle3Collection polygons)
        {
            this.polygons = polygons;
        }
    }
}
