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
        public Dictionary<string, MeshTriangle3Collection> texturedPolygons;

        private Vector3 _x = Vector3.xAxis;
        private Vector3 _y = Vector3.yAxis;
        private Vector3 _z = Vector3.zAxis;

        public Mesh(MeshTriangle3Collection polygons)
        {
            this.polygons = polygons;
        }
    }
}
