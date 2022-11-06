using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdFramework
{
    public class Frame : UIObject
    {
        public List<UIObject> children = new List<UIObject>();
        public UDim scrollBarSize = new UDim(0.0, 5.0);

        public Frame(UDim2 position, UDim2 size) : base(position, size)
        {

        }

        public override void Update(double delta)
        {
            base.Update(delta);

            foreach (UIObject obj in children)
                obj.Update(delta);
        }

        public override void Draw(Color3[,] screen)
        {
            base.Draw(screen);

            children = children.OrderByDescending(obj => obj.zindex).ToList();
            foreach (UIObject obj in children)
                obj.Draw(screen);
        }
    }
}
