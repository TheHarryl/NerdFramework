using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public abstract class Ray3Caster
    {
        public Ray3 d;
        public Vector3 w;
        public Vector3 h;
        protected Vector3s _spherical;

        public abstract Ray3 RayAt(double wAlpha, double hAlpha);
        public abstract Vector3 VectorAt(double wAlpha, double hAlpha);
        public abstract Vector2 Projection(Vector3 point);
        public abstract bool Meets(Vector3 point);
        public abstract bool Meets(MeshTriangle3 triangle);
        public abstract double Distance(Vector3 point);

        public abstract void RotateX(double radians);
        public abstract void RotateY(double radians);
        public abstract void RotateZ(double radians);
        public abstract void Rotate(double r1, double r2, double r3);
        public abstract void RotateTo(Vector3 vector);

        public abstract void RotateZenith(double radians);
        public abstract void RotatePolar(double radians);

        public abstract void RotateAbout(Vector3 rotand, double radians);
    }
}
