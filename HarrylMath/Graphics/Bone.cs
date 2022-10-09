using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct Bone
    {
        Dictionary<Vector3, double> verticeWeights;
        Vector3 p;
        Vector3 v;

        public Bone(Dictionary<Vector3, double> verticeWeights, Vector3 position, Vector3 vector)
        {
            this.verticeWeights = verticeWeights;
            this.p = position;
            this.v = vector;
        }
    }
}
