﻿using System;
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

        public Mesh(MeshTriangle3Collection polygons)
        {
            this.polygons = polygons;
        }
    }
}