namespace HarrylMath
{
    public class Ray3Sector
    {
        public Ray3 d;
        protected Vector3 w0;
        protected Vector3 w1;
        protected Vector3 h0;
        protected Vector3 h1;

        public Ray3Sector(Ray3 direction, double wRadians, double hRadians)
        {
            this.d = new Ray3(direction.p, Vector3.zAxis);
            this.w0 = d.v.RotateY(-wRadians /2);
            this.w1 = d.v.RotateY(wRadians / 2);
            this.h0 = d.v.RotateX(-hRadians / 2);
            this.h1 = d.v.RotateX(hRadians / 2);

            RotateTo(direction.v);
        }

        public Ray3 Ray(double wAlpha, double hAlpha)
        {
            return new Ray3(d.p, Vector3.Lerp(Vector3.Lerp(w0, w1, wAlpha), Vector3.Lerp(h0, h1, wAlpha), hAlpha));
        }

        public void RotateX(double radians)
        {
            d.RotateX(radians);
            w0 = w0.RotateX(radians);
            w1 = w1.RotateX(radians);
            h0 = h0.RotateX(radians);
            h1 = h1.RotateX(radians);
        }

        public void RotateY(double radians)
        {
            d.RotateY(radians);
            w0 = w0.RotateY(radians);
            w1 = w1.RotateY(radians);
            h0 = h0.RotateY(radians);
            h1 = h1.RotateY(radians);
        }

        public void RotateZ(double radians)
        {
            d.RotateZ(radians);
            w0 = w0.RotateZ(radians);
            w1 = w1.RotateZ(radians);
            h0 = h0.RotateZ(radians);
            h1 = h1.RotateZ(radians);
        }

        public void Rotate(double r1, double r2, double r3)
        {
            d.Rotate(r1, r2, r3);
            w0 = w0.Rotate(r1, r2, r3);
            w1 = w1.Rotate(r1, r2, r3);
            h0 = h0.Rotate(r1, r2, r3);
            h1 = h1.Rotate(r1, r2, r3);
        }

        public void RotateTo(Vector3 vector)
        {
            Vector3 rotation = Vector3.Angle3(d.v, vector);
            Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
