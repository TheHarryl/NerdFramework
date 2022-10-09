namespace NerdFramework
{
    public class Box3 : BoundingShape3
    {
        public Vector3 s;

        public Box3(Vector3 position, Vector3 size)
        {
            this.p = position;
            this.s = size;
        }

        public override bool Meets(Vector3 point)
        {
            return (point.x >= p.x && point.y >= p.y && point.x <= p.x + s.x && point.y <= p.y + s.y);
        }

        public override bool Meets(Box3 box)
        {
            return Meets(box.p) || Meets(box.p + box.s);
        }

        public override bool Meets(Sphere3 sphere)
        {
            throw new System.NotImplementedException();
        }
    }
}
