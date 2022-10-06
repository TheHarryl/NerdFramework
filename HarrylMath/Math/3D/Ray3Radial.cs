namespace NerdFramework
{
    public class Ray3Radial : Ray3Caster
    {
        protected Vector3 w;
        protected Vector3 h;
        protected double angle;

        public Ray3Radial(Ray3 direction, double angle)
        {
            this.d = new Ray3(direction.p, Vector3.zAxis);
            this.w = Vector3.xAxis;
            this.h = Vector3.yAxis;
            this.angle = angle;

            RotateTo(direction.v);
        }

        public override Ray3 RayAt(double wAlpha, double hAlpha)
        {
            throw new System.NotImplementedException();
        }

        public override Vector2 Projection(Vector3 point)
        {
            throw new System.NotImplementedException();
        }

        public override bool Meets(Vector3 point)
        {
            return Vector3.Angle(d.v, point - d.p) <= angle / 2.0;
        }

        public override void RotateX(double radians)
        {
            d.RotateX(radians);
            w = w.RotateX(radians);
            h = h.RotateX(radians);
        }

        public override void RotateY(double radians)
        {
            d.RotateY(radians);
            w = w.RotateY(radians);
            h = h.RotateY(radians);
        }

        public override void RotateZ(double radians)
        {
            d.RotateZ(radians);
            w = w.RotateZ(radians);
            h = h.RotateZ(radians);
        }

        public override void Rotate(double r1, double r2, double r3)
        {
            d.Rotate(r1, r2, r3);
            w = w.Rotate(r1, r2, r3);
            h = h.Rotate(r1, r2, r3);
        }

        public override void RotateTo(Vector3 vector)
        {
            Vector3 rotation = Vector3.Angle3(d.v, vector);
            Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
