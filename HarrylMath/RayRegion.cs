namespace HarrylMath
{
    class RayRegion
    {
        public Ray d;
        protected Vector3 w;
        protected Vector3 h;

        public RayRegion(Ray direction, double width, double height)
        {
            this.d = new Ray(direction.p, Vector3.zAxis);
            this.w = Vector3.xAxis * width;
            this.h = Vector3.yAxis * height;

            RotateTo(direction.v);
        }

        public Ray Ray(double wAlpha, double hAlpha)
        {
            return new Ray(d.p + w * (wAlpha - 0.5) + h * (hAlpha - 0.5), d.v);
        }

        public void RotateX(double radians)
        {
            d.RotateX(radians);
            w = w.RotateX(radians);
            h = h.RotateX(radians);
        }

        public void RotateY(double radians)
        {
            d.RotateY(radians);
            w = w.RotateY(radians);
            h = h.RotateY(radians);
        }

        public void RotateZ(double radians)
        {
            d.RotateZ(radians);
            w = w.RotateZ(radians);
            h = h.RotateZ(radians);
        }

        public void Rotate(double r1, double r2, double r3)
        {
            d.Rotate(r1, r2, r3);
            w = w.Rotate(r1, r2, r3);
            h = h.Rotate(r1, r2, r3);
        }

        public void RotateTo(Vector3 vector)
        {
            Vector3 rotation = Vector3.Angle3(d.v, vector);
            Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
