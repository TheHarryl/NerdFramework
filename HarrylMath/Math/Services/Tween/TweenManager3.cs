using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TweenManager3
    {
        private Vector3 _value;
        public List<TweenInstance3> tweenStack;

        public TweenManager3(Vector3 defaultValue)
        {
            _value = defaultValue;
            tweenStack = new List<TweenInstance3>();
        }

        public Vector3 Value(double delta)
        {
            if (tweenStack.Count == 0) return _value;

            Vector3 value = tweenStack[0].Value(delta);
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
