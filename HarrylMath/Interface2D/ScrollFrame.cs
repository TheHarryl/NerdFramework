using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class ScrollFrame : Frame
    {
        public UDim2 sizeVirtual = UDim2.One;

        public ScrollFrame(UDim2 position, UDim2 size) : base(position, size)
        {

        }
    }
}
