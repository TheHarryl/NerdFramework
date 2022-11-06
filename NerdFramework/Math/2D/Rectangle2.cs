namespace NerdFramework
{
    public struct Rectangle2
    {
        public UDim2 p;
        public UDim2 s;

        public static Rectangle2 One = new Rectangle2(Vector2.Zero, Vector2.One);

        public Rectangle2(UDim2 position, UDim2 size)
        {
            this.p = position;
            this.s = size;
        }

        public Rectangle2(Vector2 position, Vector2 size)
        {
            this.p = new UDim2(new UDim(0.0, position.x), new UDim(0.0, position.y));
            this.s = new UDim2(new UDim(0.0, size.x), new UDim(0.0, size.y));
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
