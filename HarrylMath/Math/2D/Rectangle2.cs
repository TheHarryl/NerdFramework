namespace NerdFramework
{
    public class Rectangle2
    {
        public UDim2 p;
        public UDim2 s;

        public Rectangle2(UDim2 position, UDim2 size)
        {
            this.p = position;
            this.s = size;
        }

        public bool Overlaps(Vector2 point, int windowWidth = 0, int windowHeight = 0)
        {
            Vector2 pAbsolute = p.Absolute(windowWidth, windowHeight);
            Vector2 sAbsolute = s.Absolute(windowWidth, windowHeight);
            return (point.x >= pAbsolute.x && point.y >= pAbsolute.y && point.x <= pAbsolute.x + sAbsolute.x && point.y <= pAbsolute.y + sAbsolute.y);
        }

        public bool Overlaps(Rectangle2 rectangle, int windowWidth = 0, int windowHeight = 0)
        {
            Vector2 pAbsolute = rectangle.p.Absolute(windowWidth, windowHeight);
            Vector2 sAbsolute = rectangle.s.Absolute(windowWidth, windowHeight);
            return Overlaps(pAbsolute) || Overlaps(pAbsolute + sAbsolute);
        }
    }
}
