namespace NerdFramework
{
    public abstract class BoundingShape3
    {
        public Vector3 p;

        public abstract bool Meets(Vector3 point);
        public abstract bool Meets(Box3 box);
        public abstract bool Meets(Sphere3 sphere);
    }
}
