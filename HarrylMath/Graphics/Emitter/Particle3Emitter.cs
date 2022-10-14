using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Particle3Emitter
    {
        public Ray3 face;

        public Texture2 texture;
        public Color3Sequence color;

        public float duration;
        public float emissionRate;
        public float spread;

        public NumberRange velocity;
        public Vector3 acceleration;
    }
}
