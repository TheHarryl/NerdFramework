namespace NerdFramework
{
    public class Rectangle2
    {
        public Vector2 p;
        public Vector2 s;

        public Rectangle2(Vector2 position, Vector2 size)
        {
            this.p = position;
            this.s = size;
        }

        public bool Overlaps(Vector2 point)
        {
            return (point.x >= p.x && point.y >= p.y && point.x <= p.x + s.x && point.y <= p.y + s.y);
        }

        public bool Overlaps(Rectangle2 rectangle)
        {
            return Overlaps(rectangle.p) || Overlaps(rectangle.p + rectangle.s);
        }
    }
}
