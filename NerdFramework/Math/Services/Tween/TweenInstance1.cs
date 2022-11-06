using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct TweenInstance1
    {
        public double lifespan;

        private double _duration;
        private double _value0;
        private double _value1;

        private Func<double, double, double, double> _func;

        public TweenInstance1(double duration, double x0, double x1, Func<double, double, double, double> func)
        {
            lifespan = duration;

            _duration = duration;
            _value0 = x0;
            _value1 = x1;

            _func = func;
        }

        public double Value(double delta)
        {
            lifespan -= delta;

            if (lifespan <= 0) return _value1;

            double interpolant = (1.0 - lifespan) / _duration;

            return _func(_value0, _value1, interpolant);
        }
    }
}
