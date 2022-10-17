using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TrianglePhysicsInfo
    {
        public bool anchored = true;
        public bool collisions = true;

        public Vector3 acceleration = Vector3.Zero;
        public Vector3 velocity = Vector3.Zero;
        public Vector3 origin = Vector3.Zero;


    }
}
