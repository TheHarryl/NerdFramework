using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class GameTriangle3 : Triangle3
    {
        public TrianglePhysicsInfo physics;
        public TriangleRenderInfo graphics;

        public GameTriangle3(Vector3 a, Vector3 b, Vector3 c) : base(a, b, c)
        {
        }

        public GameTriangle3(Vector3 a, Vector3 b, Vector3 c, TrianglePhysicsInfo physics, TriangleRenderInfo graphics) : base(a, b, c)
        {
            this.physics = physics;
            this.graphics = graphics;
        }
    }
}
