namespace NerdFramework
{
    public class Ray3Sector : Ray3Caster
    {
        protected double FOV;

        protected double width;
        protected double height;
        protected double r;
        protected double v;

        public Ray3Sector(Ray3 direction, double width, double height, double FOV)
        {
            this.d = new Ray3(direction.p, Vector3.zAxis);
            this.width = width;
            this.height = height;
            this.FOV = FOV;

            /* arc length = 2*PI*r*(theta/180)
             * width = 2PI*r*(FOV/180)
             * r = (width*180) / (2*PI*FOV)
             * 
             * a^2 + b^2 = c^2
             * d^2 + (w/2)^2 = r^2
             * d = sqrt(r^2 - w^2/4)
             */

            this.r = (width * 180) / (Math.TwoPI * FOV);
            this.v = Math.Sqrt(r * r - width * width / 4) / 20;

            RotateTo(direction.v);
        }

        public override Ray3 RayAt(double wAlpha, double hAlpha)
        {
            return new Ray3(d.p, VectorAt(wAlpha, hAlpha));
        }

        public override Vector3 VectorAt(double wAlpha, double hAlpha)
        {
            return (d.v * v) + w * (wAlpha - 0.5) + h * (hAlpha - 0.5);
        }

        public override Vector2 Projection(Vector3 point)
        {
            /* arc length = 2*PI*r*(theta/180)
             * width = 2PI*r*(FOV/180)
             * r = (width*180) / (2*PI*FOV)
             * 
             * a^2 + b^2 = c^2
             * d^2 + (w/2)^2 = r^2
             * d = sqrt(r^2 - w^2/4)
             */

            Vector3 origin = d.p + d.v * v - w * 0.5 - h * 0.5;
            Vector3 intersection = new Plane3(origin, d.v).Intersection(new Line3(point, d.p - point));

            return Triangle3.Parameterization(origin, origin + w, origin + h, intersection);
        }

        public override bool Meets(Vector3 point)
        {
            throw new System.NotImplementedException();
        }

        public override double Distance(Vector3 point)
        {
            Plane3 plane = new Plane3(d.p, d.v);
            return plane.Min(point);
        }

        private void SetAxes()
        {
            w = new Vector3(_spherical.RotatePolar(-Math.HalfPI)) * width;
            h = new Vector3(_spherical.RotateZenith(-Math.HalfPI)) * height;
        }

        public override void RotateX(double radians)
        {
            d.RotateX(radians);
            _spherical = new Vector3s(d.v);
            SetAxes();
        }

        public override void RotateY(double radians)
        {
            d.RotateY(radians);
            _spherical = new Vector3s(d.v);
            SetAxes();
        }

        public override void RotateZ(double radians)
        {
            d.RotateZ(radians);
            _spherical = new Vector3s(d.v);
            SetAxes();
        }

        public override void Rotate(double r1, double r2, double r3)
        {
            d.Rotate(r1, r2, r3);
            _spherical = new Vector3s(d.v);
            SetAxes();
        }

        public override void RotateAbout(Vector3 rotand, double radians)
        {
            d.RotateAbout(rotand, radians);
            _spherical = new Vector3s(d.v);
            SetAxes();
        }

        public override void RotateTo(Vector3 vector)
        {
            Vector3 rotation = Vector3.Angle3(d.v, vector);
            Rotate(rotation.x, rotation.y, rotation.z);
        }

        public override void RotateZenith(double radians)
        {
            _spherical = _spherical.RotateZenith(radians);
            d.v = new Vector3(_spherical);
            SetAxes();
        }

        public override void RotatePolar(double radians)
        {
            _spherical = _spherical.RotatePolar(radians);
            d.v = new Vector3(_spherical);
            SetAxes();
        }
    }
}
