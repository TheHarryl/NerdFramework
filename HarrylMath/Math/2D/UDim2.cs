using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class UDim2
    {
        public UDim x;
        public UDim y;

        public UDim2()
        {
            this.x = new UDim();
            this.y = new UDim();
        }

        public UDim2(double xScale, double xOffset, double yScale, double yOffset)
        {
            this.x = new UDim(xScale, xOffset);
            this.y = new UDim(yScale, yOffset);
        }

        public UDim2(UDim x, UDim y)
        {
            this.x = x;
            this.y = y;
        }

        public static UDim2 FromScale(double xScale, double yScale)
        {
            return new UDim2(new UDim(xScale, 0.0), new UDim(yScale, 0.0));
        }

        public static UDim2 FromOffset(double xOffset, double yOffset)
        {
            return new UDim2(new UDim(0.0, xOffset), new UDim(0.0, yOffset));
        }

        public Vector2 Absolute(int width, int height)
        {
            return new Vector2(x.Absolute(width), y.Absolute(height));
        }
    }
}
