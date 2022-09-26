namespace NerdEngine
{
    public class Triangle3
    {
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Triangle3(Vector3 a, Vector3 b, Vector3 c)
        {
            /* Triangle:
             * a: point1
             * b: point2
             * c: point3
             */

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public void Move(Vector3 offset)
        {
            a += offset;
            b += offset;
            c += offset;
        }

        public Vector3 Normal()
        {
            /* A, B, and C are COPLANAR, all are solutions of Plane in which triangle resides
             * n ⊥ Plane
             * 
             * n ⊥ A, B, and C
             * n ⊥ AB and AC
             * 
             * c ⊥ a AND c ⊥ b <=> c = a×b
             * n = a×b
             */

            return Vector3.Cross(new Vector3(a, b), new Vector3(a, c));
        }

        public double Area()
        {
            /* |a×b| = |A||B|sin(theta) = Area of Parallelogram
             * Parallelogram is two triangles
             */

            return Vector3.Cross(b - a, c - a).Magnitude() / 2.0;
        }

        public void RotateX(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateX(radians);
            Vector3 OB = (b - origin).RotateX(radians);
            Vector3 OC = (c - origin).RotateX(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;
        }

        public void RotateY(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateY(radians);
            Vector3 OB = (b - origin).RotateY(radians);
            Vector3 OC = (c - origin).RotateY(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;
        }

        public void RotateZ(double radians, Vector3 origin)
        {
            Vector3 OA = (a - origin).RotateZ(radians);
            Vector3 OB = (b - origin).RotateZ(radians);
            Vector3 OC = (c - origin).RotateZ(radians);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;
        }

        public void Rotate(double r1, double r2, double r3, Vector3 origin)
        {
            Vector3 OA = (a - origin).Rotate(r1, r2, r3);
            Vector3 OB = (b - origin).Rotate(r1, r2, r3);
            Vector3 OC = (c - origin).Rotate(r1, r2, r3);

            a = origin + OA;
            b = origin + OB;
            c = origin + OC;
        }

        public bool Meets(Vector3 point)
        {
            /* A, B, and C are COPLANAR, all are solutions of Plane
             * 
             * p1 lies between AB and AC IF:
             * 
             * Angles AB-AP and AC-AP equal AB-AC:
             * a⋅b = |a||b|cos(theta)
             * cos(theta) = (a⋅b)/(|a||b|)
             * theta = acos((a⋅b)/(|a||b|))
             * acos((AB⋅AP)/(|AB||AP|)) + acos((AC⋅AP)/(|AC||AP|)) <= acos((AB⋅AC)/(|AB||AC|))
             * acos((AB⋅AP)/(|AB||AP|)) + acos((AC⋅AP)/(|AC||AP|)) <= acos((AB⋅AC)/(|AB||AC|))
             * 
             * Magnitude of the projection of AP onto AB is <= the magnitude of AB <= the magnitude of AC:
             * a⋅b = |a||b|cos(theta)
             * AB⋅AP = |AB||AP|cos(theta)
             * |AP|cos(theta) = AB⋅AP/|AB|
             * AB⋅AP/|AB| <= |AB|
             * AB⋅AP <= |AB||AB|
             * AB⋅AP <= AB⋅AB
             * AC⋅AP <= AC⋅AC
             */

            Vector3 AB = (b - a);
            Vector3 AC = (c - a);
            Vector3 AP = (point - a);
            double ABm = AB.Magnitude();
            double ACm = AC.Magnitude();
            double APm = AP.Magnitude();
            double ABAP = Vector3.Dot(AB, AP);
            double ACAP = Vector3.Dot(AC, AP);

            return
                Math.Abs(Math.Acos(Vector3.Dot(AB, AC) / (ABm * ACm)) - (Math.Acos(ABAP / (ABm * APm)) + Math.Acos(ACAP / (ACm * APm)))) <= 0.0001 &&
                ABAP <= ABm * ABm && ACAP <= ACm * ACm;
        }

        public bool Meets(Ray3 ray)
        {
            /* 1st Check:
             * Simplifies the question in terms of the line the ray resides in and the plane the triangle resides in
             * Do they meet?
             */
            Line3 line = new Line3(ray.p, ray.v);
            Plane3 plane = new Plane3(a, Normal());
            if (!plane.Meets(line)) return false;

            /* 2nd Check:
             * Is this point of intersection within the triangle?
             */
            Vector3 pointOfIntersection = plane.Intersection(line);
            if (!Meets(pointOfIntersection)) return false;

            /* Final Check:
             * (Definition of the Ray)
             * Is this point of intersection in the positive vector direction of the line's starting point?
             */
            return (pointOfIntersection - line.p).Normalized() == line.v.Normalized();
        }

        /*public bool Meets(Triangle3 triangle)
        {

        }*/

        public Vector3 Intersection(Ray3 ray)
        {
            // In order to maximize performance, this code assumes you will only
            //   use this function if you already know the triangle and ray intersect at a point.
            // This condition can be checked for by using the Meets(Ray) function.

            Line3 line = new Line3(ray.p, ray.v);
            Plane3 plane = new Plane3(a, Normal());

            return plane.Intersection(line);
        }

        /*public double Min(Vector3 point)
        {

        }*/

        /*public double Min(Ray3 ray)
        {

        }*/

        /*public double Min(Triangle3 triangle)
        {

        }*/

        public static Triangle3Group operator +(Triangle3 a, Triangle3 b)
        {
            return new Triangle3Group(new System.Collections.Generic.List<Triangle3>() { a, b });
        }
    }
}
