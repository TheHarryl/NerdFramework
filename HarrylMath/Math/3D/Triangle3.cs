﻿namespace NerdFramework
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

        public void Move(Vector3 offset)
        {
            a += offset;
            b += offset;
            c += offset;
        }

        public void Scale(Vector3 scale, Vector3 origin)
        {
            a = (a - origin) * scale + origin;
            b = (b - origin) * scale + origin;
            c = (c - origin) * scale + origin;
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
            /* A, B, and C are COPLANAR, and vertices of the triangle
             * P is any point that lies within the triangle
             * 
             * Parameterization of triangle:
             * A triangle is created by two vectors AB and AC
             * A solution of the triangle consists of initial point A with added components AB*t and AC*s
             * In other words, picture a triangle as a defined coordinate plane of two (non?)orthogonal axes, AB and AC
             * 
             * OP = OA + (OB-OA)t + (OC-OA)s
             * (OP-OA) = (OB-OA)t + (OC-OA)s <=> AP = ABt + ACs
             * The point is within the triangle if t and s fall within defined boundaries
             * 
             * Solving for t and s:
             * P = (OP-OA)
             * B = (OB-OA)
             * C = (OC-OA)
             * 
             * P = Bt + Cs
             * Since P being coplanar is a given, we can eliminate one dimension through rotation (rigid transformation)
             * Thus, we can derive t and s from a system of two equations:
             * P.x = B.x*t + C.x*s
             * P.y = B.y*t + C.y*s
             * 
             * s = (P.y - B.y*t)/C.y
             * t = (P.y - C.y*s)/B.y
             * 
             * P.x = B.x*t + C.x*[(P.y - B.y*t)/C.y]
             * B.x*t - B.y*t*(C.x/C.y) = P.x - P.y*(C.x/C.y)
             * t = [P.x - P.y*(C.x/C.y)] / [B.x - B.y*(C.x/C.y)]
             * 
             * P.x = B.x*(P.y - C.y*s)/B.y + C.x*s
             * C.x*s - C.y*s*(B.x/B.y) = P.x - P.y*(B.x/B.y)
             * s = [P.x - P.y*(B.x/B.y)] / [C.x - C.y*(B.x/B.y)]
             * 
             * t and s are interpolant values between 0 and 1 that describe P length relative to axis lengths
             * IF t defines how far the point is along AB and s the same for AC:
             * The hypotenuse BC can be seen as a linear inverse relationship between s and t
             * 
             * THUS the additional condition applies:
             * t <= 1 - s
             * t + s <= 1
             * 
             * [Conclusion]
             * After calculating t and s using the following formulas:
             * t = [P.x - P.y*(C.x/C.y)] / [B.x - B.y*(C.x/C.y)]
             * s = [P.x - P.y*(B.x/B.y)] / [C.x - C.y*(B.x/B.y)]
             * 
             * The point is a solution of the triangle if:
             * t >= 0
             * s >= 0
             * t + s <= 1
             */

            Vector3 AB = b - a;
            Vector3 AC = c - a;
            Vector3 AP = point - a;
            double ABdiff = AB.x / AB.y;
            double ACdiff = AC.x / AC.y;
            double t = (AP.x - AP.y * ACdiff) / (AB.x - AB.y * ACdiff);
            double s = (AP.x - AP.y * ABdiff) / (AC.x - AC.y * ABdiff);

            return t >= 0 && s >= 0 && t + s <= 1;


            /* METHOD 2 */

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
             * Magnitude of the projection of AP onto AB and AC is <= the magnitude of AB and AC:
             * a⋅b = |a||b|cos(theta)
             * AB⋅AP = |AB||AP|cos(theta)
             * |AP|cos(theta) = AB⋅AP/|AB|
             * AB⋅AP/|AB| <= |AB|
             * AB⋅AP <= |AB||AB|
             * AB⋅AP <= AB⋅AB
             * AC⋅AP <= AC⋅AC
             */

            /*Vector3 AB = (b - a);
            Vector3 AC = (c - a);
            Vector3 AP = (point - a);
            double ABm = AB.Magnitude();
            double ACm = AC.Magnitude();
            double APm = AP.Magnitude();
            double ABAP = Vector3.Dot(AB, AP);
            double ACAP = Vector3.Dot(AC, AP);

            return
                Math.Abs(Math.Acos(Vector3.Dot(AB, AC) / (ABm * ACm)) - (Math.Acos(ABAP / (ABm * APm)) + Math.Acos(ACAP / (ACm * APm)))) <= 0.0001 &&
                ABAP <= ABm * ABm && ACAP <= ACm * ACm;*/
        }

        public bool Meets(Ray3 ray)
        {
            /* 1st Check:
             * Would the ray collide on the triangle's front face?
             */
            if (Vector3.Dot(ray.v, Normal()) < 0) return false;

            /* 2nd Check:
             * Simplifies the question in terms of the line the ray resides in and the plane the triangle resides in
             * Do they meet?
             */

            Vector3 n = Normal();
            Line3 line = new Line3(ray.p, ray.v);
            Plane3 plane = new Plane3(a, n);
            if (!plane.Meets(line)) return false;

            /* Final Check:
             * Is this point of intersection within the triangle?
             */

            /* Plane:
             * n.x(x - q.x) + n.y(y - q.y) + n.z(z - q.z) = 0
             * 
             * Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * Intersection:
             * n.x([p.x + v.xt] - q.x) + n.y([p.y + v.yt] - q.y) + n.z([p.z + v.zt] - q.z) = 0
             * n⋅p + n⋅vt - n⋅q = 0
             * (n⋅v)t = n⋅(q - p)
             * t = [n⋅(q - p)]/(n⋅v)
             */

            Vector3 pointOfIntersection = plane.Intersection(line);

            //if (Vector3.Dot((pointOfIntersection - line.p), line.v) < 0) return false;
            //if ((pointOfIntersection - line.p).Normalized() != line.v.Normalized()) return false;

            return Meets(pointOfIntersection);
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