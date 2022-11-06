using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TweenManager2
    {
        private Vector2 _value;
        public List<TweenInstance2> tweenStack;

        public TweenManager2(Vector2 defaultValue)
        {
            _value = defaultValue;
            tweenStack = new List<TweenInstance2>();
        }

        public Vector2 Value(double delta)
        {
            if (tweenStack.Count == 0) return _value;

            Vector2 value = tweenStack[0].Value(delta);
            while (tweenStack.Count > 0 && tweenStack[0].lifespan <= 0)
            {
                if (tweenStack.Count > 1)
                    value = tweenStack[1].Value(-tweenStack[0].lifespan);
                tweenStack.RemoveAt(0);
            }

            return value;
        }
    }
}
