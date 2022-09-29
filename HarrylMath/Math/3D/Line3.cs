﻿namespace NerdFramework
{
    public class Line3
    {
        public Vector3 p;
        public Vector3 v;

        public Line3(Vector3 position, Vector3 vector)
        {
            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * v ∥ Line
             * p is a solution of the line
             */

            p = position;
            v = vector;
        }

        public bool Meets(Vector3 point)
        {
            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * 
             * <x, y, z> = p + vt
             * <x, y, z> - p = vt
             * 
             * vt / |v| = ±v
             * (<x, y, z> - p) / |v| = ±v
             * 
             * IF the vector between the specified point and ANY point on the Line
             *   is ∥ to the Line's vector, the specified point exists on the Line
             */

            Vector3 v0 = v.Normalized();
            Vector3 v1 = (point - p).Normalized();

            return v0 == v1 || v0 == -v1;
        }

        public bool Meets(Line3 line)
        {
            return Min(line) == 0.0;
        }

        public Vector3 Intersection(Line3 line)
        {
            // In order to maximize performance, this code assumes you will only
            //   use this function if you already know the two lines intersect at a single point.
            // These conditions can be checked for by checking if the two lines meet and are nonparallel.

            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * v0 ∥ Line0
             * v1 ∥ Line1
             * 
             * L0 = p + v0t
             * L1 = q + v1s
             * 
             * Augmented Matrix:
             *   t    s
             * | v0.x -v1.x | q.x-p.x |   | a b | c |
             * | v0.y -v1.y | q.y-p.y | = | d e | f |
             * | v0.z -v1.z | q.z-p.z |   | g h | i |
             * 
             * Given lines only intersect at a single point:
             * R2 -> R2 - (d/a)R1
             * R2 -> R2 * 1/[e-(d/a)b]
             * R3 -> 0
             *   t    s
             * | a b | c          |
             * | 0 1 | f - (d/a)c |
             * 
             * s = [q.y-p.y - (v0.y/v0.x)*(q.x-p.x)]/[-v1.y+(v0.y/v0.x)*v1.x]
             *   
             * Intersection:
             * p = q + v1*[q.y-p.y - (v0.y/v0.x)*(q.x-p.x)]/[-v1.y+(v0.y/v0.x)*v1.x]
             */

            return line.p + line.v * (line.p.y - p.y - (v.y / v.x) * (line.p.x - p.x)) / (-line.v.y + (v.y / v.x) * line.v.x);


            /* METHOD 2 */

            /* Given: Lines are coplanar and nonparallel
             * 
             * The point of intersection is found by finding the line of intersection
             * of two planes that both are perpendicular to the plane in which the two lines reside in
             * and contain the solutions of each line.
             * 
             * The point at which this line of intersection meets the plane of the lines
             * is the point of intersection of the two lines.
             * 
             * Implementation:
             * n = (v0×v1) = <(v0.y*v1.z) - (v0.z*v1.y), -[(v0.x*v1.z) - (v0.z*v1.x)], (v0.x*v1.y) - (v0.y*v1.x)>
             * P = plane of lines
             * P0 = plane of line0 projection
             * P1 = plane of line1 projection
             * L = line of intersection
             * Plane:
             * n.x(x - p.x) + n.y(y - p.y) + n.z(z - p.z) = 0
             * P0: (n×v0)⋅(<x,y,z> - p) = 0
             * P1: (n×v1)⋅(<x,y,z> - q) = 0
             * L: <x, y, z> = ((n×v0)*p - (n×v1)*q) / ((n×v0) - (n×v1)) + [(n×v0)×(n×v1)]t
             * t = [n⋅(p - [((n×v0)*p - (n×v1)*q) / ((n×v0) - (n×v1))])]/(n⋅[(n×v0)×(n×v1)])
             * point: ((n×v0)*p - (n×v1)*q) / ((n×v0) - (n×v1)) + [(n×v0)×(n×v1)]*[[n⋅(p - [((n×v0)*p - (n×v1)*q) / ((n×v0) - (n×v1))])]/(n⋅[(n×v0)×(n×v1)])]
             */

            /*Vector3 n = Vector3.Cross(v, line.v);
            Plane3 planeOfLines = new Plane3(p, n);
            Plane3 plane0 = new Plane3(p, Vector3.Cross(n, v));
            Plane3 plane1 = new Plane3(line.p, Vector3.Cross(n, line.v));
            Line3 lineOfIntersection = plane0.Intersection(plane1);

            return planeOfLines.Intersection(lineOfIntersection);*/
        }

        public double Min(Vector3 point)
        {
            /* d = p0 - p = Path between any point of Plane and specified point
             * v ∥ Line
             * 
             * Minimum path between point and Line is ⊥ to the Line
             * n = Minimum path
             * d, v, and n form a right triangle
             * 
             * IF theta = angle between d and v:
             * |d|sin(theta) = Projection of d onto n
             * 
             * Turns it ⊥ to the Plane, shortest path between point and Plane
             * 
             * |a×b| = |A||B|sin(theta)
             * |d×v| = |d||v|sin(theta)
             * (|d×v|)/|v| = |d|sin(theta)
             */

            return Vector3.Cross(point - p, v).Magnitude() / v.Magnitude();
        }

        public double Min(Line3 line)
        {
            /* Different types of Line solutions:
             * Parallel intersecting        (All solutions of Line0 are all solutions of Line1 and vice versa)
             * Parallel non-intersecting    (Line0 and Line1 have no solutions)
             * Skew                         (Line0 and Line1 have no solutions)
             * Traditional                  (Line0 and Line1 intersect at one point)
             */

            /* Lines are parallel intersecting (any arbitrary point of one line would be a solution of the other, as they are the same line)
             * or coincidentally intersect at a predefined arbitrary point of both lines
             */
            if (Meets(line.p))
                return 0.0;

            Vector3 v0 = v.Normalized();
            Vector3 v1 = line.v.Normalized();

            /* Lines are parallel non-intersecting
             * THEREFORE any point within both Lines yields the minimum distance
             */
            if (v0 == v1 || v0 == -v1)
                return Min(line.p);

            /* Line:
             * x = p.x + v.xt
             * y = p.y + v.yt
             * z = p.z + v.zt
             * v0 ∥ Line0
             * v1 ∥ Line1
             * 
             * Two Lines intersect IF they lie in the same plane
             * n ⊥ v0, v1 <=> n = v0×v1
             * 
             * Plane:
             * n.x(x - p.x) + n.y(y - p.y) + n.z(z - p.z) = 0
             * n = v0×v1
             * p0 = Any point in Line0
             * p1 = Any point in Line1
             * 
             * Two planes:
             * Plane0: n⋅(<x,y,z> - p0) = 0
             * Plane1: n⋅(<x,y,z> - p1) = 0
             * 
             * Distance between planes yields minimum distance
             * d > 0: Lines are skew
             * d = 0: Lines intersect traditionally
             */

            Vector3 n = Vector3.Cross(v, line.v);
            return (new Plane3(p, n)).Min(line.p);
        }
    }
}
