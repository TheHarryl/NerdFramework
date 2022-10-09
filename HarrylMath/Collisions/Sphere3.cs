namespace NerdFramework
{
    public class Sphere3 : BoundingShape3
    {
        public double r;

        public Sphere3(Vector3 position, double radius)
        {
            this.p = position;
            this.r = radius;
        }

        public override bool Meets(Vector3 point)
        {
            return (point - p).Magnitude() <= r;
        }

        public override bool Meets(Box3 box)
        {
            throw new System.NotImplementedException();
        }

        public override bool Meets(Sphere3 sphere)
        {
            return (sphere.p - p).Magnitude() <= Math.Max(r, sphere.r);
        }
    }
}
