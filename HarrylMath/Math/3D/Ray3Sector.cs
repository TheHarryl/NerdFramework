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
            this.w = Vector3.xAxis * width;
            this.h = Vector3.yAxis * height;
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
            /*Vector3 v1 = (d.p + d.v * v) + w * (wAlpha - 0.5) + h * (hAlpha - 0.5);
            Vector3 v0 = d.p;
            return new Ray3(d.p, v1 - v0);*/
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

            return Triangle3.Parameterization(origin, origin + w, origin + h, intersection);// new Vector2(intersection.x / w.x + 0.5, intersection.y / h.y + 0.5);
            
            /*double distX = new Plane3(d.p - w/2, w).Min(intersection);
            double distY = new Plane3(d.p - h/2, h).Min(intersection);

            return new Vector2(distX / w.x, distY / h.y);*/
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

        public override void RotateAbout(Vector3 rotand, double radians)
        {
            d.RotateAbout(rotand, radians);
            w = w.RotateAbout(rotand, radians);
            h = h.RotateAbout(rotand, radians);
        }

        public override void RotateTo(Vector3 vector)
        {
            Vector3 rotation = Vector3.Angle3(d.v, vector);
            Rotate(rotation.x, rotation.y, rotation.z);
        }
    }
}
