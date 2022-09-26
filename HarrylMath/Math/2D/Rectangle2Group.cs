namespace NerdEngine
{
    public class Rectangle2Group
    {
        protected System.Collections.Generic.List<Rectangle2> rectangles;
        private Vector2 _origin;
        public Vector2 origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;

                foreach (Rectangle2 rectangle in rectangles)
                {
                    rectangle.p += value - _origin;
                }
                _origin = value;
            }
        }

        public Rectangle2Group(System.Collections.Generic.List<Rectangle2> rectangles)
        {
            this.rectangles = rectangles;
        }

        public static void Move(System.Collections.Generic.List<Rectangle2> rectangles, Vector2 offset)
        {
            foreach (Rectangle2 rectangle in rectangles)
            {
                rectangle.p += offset;
            }
        }

        public bool Overlaps(Vector2 point)
        {
            foreach (Rectangle2 rect in rectangles)
            {
                if (rect.Overlaps(point))
                    return true;
            }
            return false;
        }

        public bool Overlaps(Rectangle2 rectangle)
        {
            foreach (Rectangle2 rect in rectangles)
            {
                if (rect.Overlaps(rectangle))
                    return true;
            }
            return false;
        }

        public bool Overlaps(Rectangle2Group rectangleGroup)
        {
            foreach (Rectangle2 rect in rectangles)
            {
                if (rectangleGroup.Overlaps(rect))
                    return true;
            }
            return false;
        }
    }
}
