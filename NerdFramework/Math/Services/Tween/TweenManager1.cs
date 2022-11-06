using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class TweenManager1
    {
        private double _value;
        public List<TweenInstance1> tweenStack;

        public TweenManager1(double defaultValue)
        {
            _value = defaultValue;
            tweenStack = new List<TweenInstance1>();
        }

        public double Value(double delta)
        {
            if (tweenStack.Count == 0) return _value;

            double value = tweenStack[0].Value(delta);
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
