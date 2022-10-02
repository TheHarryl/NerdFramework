namespace NerdFramework
{
    public class Ray3Region : Ray3Caster
    {
        protected Vector3 w;
        protected Vector3 h;

        public Ray3Region(Ray3 direction, double width, double height)
        {
            this.d = new Ray3(direction.p, Vector3.zAxis);
            this.w = Vector3.xAxis * width;
            this.h = Vector3.yAxis * height;

            RotateTo(direction.v);
        }

        public override Ray3 RayAt(double wAlpha, double hAlpha)
        {
            return new Ray3(d.p + w * (wAlpha - 0.5) + h * (hAlpha - 0.5), d.v);
        }

        public override Vector2 Projection(Vector3 point)
        {
            /* Get projection of point onto camera as intersection of shortest path from point to camera's plane
             * 
             * Plane:
             * n.x(x - q.x) + n.y(y - q.y) + n.z(z - q.z) = 0
             * 
             * Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * Shortest path is ⊥ to Plane
             * n ⊥ Plane
             * 
             * Shortest path ∥ n
             * v = n
             * p = point
             * 
             * P0 = Origin of camera
             * w = Horizontal axis vector of camera
             * h = Vertical axis vector of camera
             * P1 = Intersection(plane, line)
             * 
             * P1 = P0 + wt + hs
             * We can eliminate one dimension through rotation (rigid transformation)
             * Thus, we can derive t and s from a system of two equations:
             * P1.x = P0.x + w.x*t + h.x*s
             * P1.y = P0.y + w.y*t + h.y*s
             * 
             * t = (P1.y - P0.y - h.y*s) / w.y
             * s = (P1.y - P0.y - w.y*t) / h.y
             * 
             * P1.x = P0.x + w.x*t + h.x*[(P1.y - P0.y - w.y*t) / h.y]
             * w.x*t - w.y*t*(h.x/h.y) = P1.x + P0.y*(h.x/h.y) - P0.x - P1.y*(h.x/h.y)
             * t = [P1.x + P0.y*(h.x/h.y) - P0.x - P1.y*(h.x/h.y)] / [w.x - w.y*(h.x/h.y)]
             * 
             * P1.x = P0.x + w.x*[(P1.y - P0.y - h.y*s) / w.y] + h.x*s
             * h.x*s - h.y*s*(w.x/w.y) = P1.x + P0.y*(w.x/w.y) - P0.x - P1.y*(w.x/w.y)
             * s = [P1.x + P0.y*(w.x/w.y) - P0.x - P1.y*(w.x/w.y)] / [h.x - h.y*(w.x/w.y)]
             * 
             * Since the origin is in the center of the screen:
             * t += 0.5
             * s += 0.5
             */

            Plane3 plane = new Plane3(d.p, d.v);
            Line3 line = new Line3(point, d.v);

            Vector3 intersection = plane.Intersection(line);

            double hSlope = h.x / h.y;
            double wSlope = w.x / w.y;

            //System.Diagnostics.Debug.WriteLine((intersection.x + d.p.y * wSlope - d.p.x - intersection.y * wSlope) + " " + (h.x - h.y * wSlope) + 0.5);

            return new Vector2(
                (intersection.x + d.p.y * hSlope - d.p.x - intersection.y * hSlope) / (w.x - w.y * hSlope) + 0.5,
                (intersection.x + d.p.y * wSlope - d.p.x - intersection.y * wSlope) / (h.x - h.y * wSlope) + 0.5
            );
        }

        public override bool Meets(Vector3 point)
        {
            throw new System.NotImplementedException();
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
