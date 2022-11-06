using NerdFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdPanel
{
    public abstract class BaseTab
    {
        public BaseTab(params string[] args)
        {

        }

        public abstract void Update(InterfaceEngine engine, double delta);
        public abstract void Draw(InterfaceEngine renderer);
    }
}
