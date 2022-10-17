using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TextLabel : UIObject
    {
        public Font font;
        public string text;

        public TextLabel(UDim2 position, UDim2 size) : base(position, size)
        {

        }
    }
}
