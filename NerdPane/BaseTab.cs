using NerdFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdPanel
{
    public abstract class BaseTab
    {
        public BaseTab(string args)
        {

        }

        public abstract void Update(double delta);
        public abstract void Draw(Color3[,] screen);
    }
}
