using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public struct TweenInstance3
    {
        public double lifespan;

        private double _duration;
        private Vector3 _value0;
        private Vector3 _value1;

        private Func<double, double, double, double> _func;

        public TweenInstance3(double duration, Vector3 x0, Vector3 x1, Func<double, double, double, double> func)
        {
            lifespan = duration;

            _duration = duration;
            _value0 = x0;
            _value1 = x1;

            _func = func;
        }

        public Vector3 Value(double delta)
        {
            lifespan -= delta;

            if (lifespan <= 0) return _value1;

            double interpolant = (1.0 - lifespan) / _duration;

            return new Vector3(_func(_value0.x, _value1.x, interpolant), _func(_value0.y, _value1.y, interpolant), _func(_value0.z, _value1.z, interpolant));
        }
    }
}
