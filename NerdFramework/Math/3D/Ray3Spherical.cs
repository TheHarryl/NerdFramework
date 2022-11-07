namespace NerdFramework
{
    public class Ray3Spherical : Ray3Caster
    {
        protected double FOV;
        protected double vertFOV;

        protected double width;
        protected double height;
        protected double r;

        protected Vector3 bounds1;
        protected Vector3 bounds2;
        protected Vector3 bounds3;
        protected Vector3 bounds4;

        public Ray3Spherical(Ray3 direction, double width, double height, double FOVinRadians)
        {
            this.d = new Ray3(direction.p, Vector3.zAxis);
            this.width = width;
            this.height = height;
            this.FOV = FOVinRadians;
            this.vertFOV = FOVinRadians / width * height;

            /* c^2 = a^2 + b^2 - 2abcos(theta)
             * width = 2r^2 - 2rcos(FOV)
             * 0 = 2r^2 - 2rcos(FOV) - width
             * x = [-b +/- sqrt(b^2 - 4ac)] / 2a
             * r = [2cos(FOV) +/- sqrt([2cos(FOV)]^2 + 4(2)(width))] / 2(2)
             * r = [2cos(FOV) +/- sqrt([2cos(FOV)]^2 + 8*width)] / 4
             * r = 2cos(FOV) + sqrt(4cos^2(FOV) + 8*width)
             */

            double c = Math.Cos(FOV);
            this.r = 2*c + Math.Sqrt(4*c*c + 8*width);

            RotateTo(direction.v);
        }

        public override Ray3 RayAt(double wAlpha, double hAlpha)
        {
            return new Ray3(d.p, VectorAt(wAlpha, hAlpha));
        }

        public override Vector3 VectorAt(double wAlpha, double hAlpha)
        {
            return (d.v * r) + w * (wAlpha - 0.5) + h * (hAlpha - 0.5);
        }

        public override Vector2 Projection(Vector3 point)
        {
            /* c^2 = a^2 + b^2 - 2abcos(theta)
             * width = 2r^2 - 2rcos(FOV)
             * 0 = 2r^2 - 2rcos(FOV) - width
             * x = [-b +/- sqrt(b^2 - 4ac)] / 2a
             * r = [2cos(FOV) +/- sqrt([2cos(FOV)]^2 + 4(2)(width))] / 2(2)
             * r = [2cos(FOV) +/- sqrt([2cos(FOV)]^2 + 8*width)] / 4
             * r = 2cos(FOV) + sqrt(4cos^2(FOV) + 8*width)
             */

            Vector3 origin = d.p + d.v * r - w * 0.5 - h * 0.5;
            Vector3 intersection = new Plane3(origin, d.v).Intersection(new Line3(point, d.p - point));

            return Triangle3.Parameterization(origin, origin + w, origin + h, intersection);
        }

        public override bool Meets(Vector3 point)
        {
            Vector3s pointSpherical = new Vector3s(point - d.p);
            Vector3s diff = Vector3s.Difference(pointSpherical, _spherical);
            return diff.theta <= FOV && diff.phi <= vertFOV;
        }

        public override bool Meets(MeshTriangle3 triangle)
        {
            Vector3 normal = triangle.Normal();
            return (
                Vector3.Dot(bounds1, normal) < 0.0 ||
                Vector3.Dot(bounds2, normal) < 0.0 ||
                Vector3.Dot(bounds3, normal) < 0.0 ||
                Vector3.Dot(bounds4, normal) < 0.0) &&
                (Meets(triangle.a) || Meets(triangle.b) || Meets(triangle.c));
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
            double vertFOV = FOV / width * height;
            bounds1 = new Vector3(_spherical.RotatePolar( FOV / 2));
            bounds2 = new Vector3(_spherical.RotatePolar(-FOV / 2));
            bounds3 = new Vector3(_spherical.RotateZenith( vertFOV / 2));
            bounds4 = new Vector3(_spherical.RotateZenith(-vertFOV / 2));
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
