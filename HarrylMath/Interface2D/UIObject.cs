using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdFramework
{
    public abstract class UIObject
    {
        public Color3 color = Color3.White;

        public int borderWidth = 1;
        public Color3 borderColor = Color3.Black;

        public UDim2 position = UDim2.Zero;
        public UDim2 size = new UDim2(new UDim(0.0, 200.0), new UDim(0.0, 100.0));
        public int zindex = 1;

        public Action<UIObject, double> onUpdate = (obj, delta) => { };
        public Action<UIObject, Color3[,]> onDraw = (obj, screen) => { };

        protected UIObject(UDim2 position, UDim2 size)
        {
            this.position = position;
            this.size = size;
        }

        public Vector2i AbsolutePosition(int width, int height)
        {
            return new Vector2i((int)position.x.Absolute(width), (int)position.y.Absolute(height));
        }
        public Vector2i AbsoluteSize(int width, int height)
        {
            return new Vector2i((int)size.x.Absolute(width), (int)size.y.Absolute(height));
        }

        public virtual void Update(double delta)
        {
            onUpdate(this, delta);
        }

        public virtual void Draw(Color3[,] screen)
        {
            // Object bounds; Doesn't use AbsolutePosition() and AbsoluteSize() for optimization purposes
            double xMin = position.x.Absolute(screen.GetLength(1));
            double yMin = position.y.Absolute(screen.GetLength(0));
            double xMax = xMin + size.x.Absolute(screen.GetLength(1));
            double yMax = yMin + size.y.Absolute(screen.GetLength(0));

            // Render object border color on top of pre-existing
            for (int y = (int)yMin - borderWidth; y < (int)yMin; y++)
                for (int x = (int)xMin - borderWidth; x <= (int)xMax + borderWidth; x++)
                    screen[y, x] = Color3.Flatten(screen[y, x], borderColor);
            for (int y = (int)yMax + 1; y <= (int)yMax + borderWidth; y++)
                for (int x = (int)xMin - borderWidth; x <= (int)xMax + borderWidth; x++)
                    screen[y, x] = Color3.Flatten(screen[y, x], borderColor);
            for (int y = (int)yMin; y <= (int)yMax; y++)
                for (int x = (int)xMin - borderWidth; x < (int)xMin; x++)
                    screen[y, x] = Color3.Flatten(screen[y, x], borderColor);
            for (int y = (int)yMin; y <= (int)yMax; y++)
                for (int x = (int)xMax + 1; x <= (int)xMax + borderWidth; x++)
                    screen[y, x] = Color3.Flatten(screen[y, x], borderColor);

            // Render object fill color on top of pre-existing
            Parallel.For((int)yMin, (int)yMax + 1, y => {
                for (int x = (int)xMin; x <= (int)xMax; x++)
                    screen[y, x] = Color3.Flatten(screen[y, x], color);
            });

            onDraw(this, screen);
        }
    }
}
