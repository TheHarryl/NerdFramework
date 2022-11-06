using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NerdFramework
{
    public class InterfaceEngine : Renderer2
    {
        public Frame scene;

        public InterfaceEngine(int width, int height) : base(width, height)
        {
            this.scene = new Frame(UDim2.Zero, UDim2.One);
            this.scene.borderWidth = 0;
            this.scene.color = Color3.None;
        }

        public void Update(double delta)
        {
            scene.Update(delta);
        }

        public void RenderUI()
        {
            base.Render();

            scene.Draw(this.lightBuffer);
        }
    }
}
