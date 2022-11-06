using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdFramework
{
    public enum ImageScaleType
    {
        Crop,
        Fit,
        Slice,
        Stretch,
        Tile
    }

    public class ImageLabel : UIObject
    {
        public Texture2 texture;
        public Color3 imageColor = Color3.White;
        public ImageScaleType scaleType = ImageScaleType.Stretch;

        public ImageLabel(UDim2 position, UDim2 size) : base(position, size)
        {

        }

        public override void Update(double delta)
        {
            base.Update(delta);
        }

        public override void Draw(Color3[,] screen)
        {
            if (visible == false) return;
            base.Draw(screen);

            // Object bounds; Doesn't use AbsolutePosition() and AbsoluteSize() for optimization purposes
            int screenWidth = screen.GetLength(1);
            int screenHeight = screen.GetLength(0);
            double xMin = position.x.Absolute(screenWidth);
            double yMin = position.y.Absolute(screenHeight);
            double xMax = xMin + size.x.Absolute(screenWidth);
            double yMax = yMin + size.y.Absolute(screenHeight);

            // Render the texture on top of pre-existing
            if (imageColor.alpha > 0.0)
            {
                Parallel.For((int)yMin, (int)yMax + 1, y =>
                {
                    double s = (double)(y - yMin) / (yMax - yMin);
                    for (int x = (int)xMin; x <= (int)xMax; x++)
                    {
                        double t = (double)(x - xMin) / (xMax - xMin);
                        screen[y, x] = Color3.Flatten(screen[y, x], texture.ColorAt(t, s) * imageColor);
                    }
                });
            }
        }
    }
}
