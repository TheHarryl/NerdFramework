using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class Vector3s
    {
        // Radius
        public readonly double rho;

        // Zenith angle
        public readonly double phi;
        // Polar angle
        public readonly double theta;

        public Vector3s(double rho = 1.0, double phi = 0.0, double theta = 0.0)
        {
            /* Spherical Coordinates:
             * ρ = |OP|
             * ϕ = Angle between e3 and OP
             * θ = Polar angle
             * 
             * ρ = sqrt(x^2 + y^2 + z^2)
             * cot(ϕ) = y/r
             * ϕ = arccot(y / sqrt(x^2 + z^2))
             * θ = arctan(x/z)
             * 
             * Natural Range = NR = [0, ∞) x [0, π] x [0, 2π]
             */

            /* Spherical coordinates to Rectangular:
             * x = ρsin(ϕ)cos(θ)
             * y = ρsin(ϕ)sin(θ)
             * z = ρcos(ϕ)
             * 
             * HOWEVER: This engine uses x and z as horizontal plane
             * We will shift the values with cyclic permutation
             * 
             * z = ρsin(ϕ)cos(θ)
             * x = ρsin(ϕ)sin(θ)
             * y = ρcos(ϕ)
             */

            this.rho = rho;
            this.phi = phi;
            this.theta = theta;
        }

        public Vector3s(Vector3 rectangular)
        {
            /* Rectangular coordinates to Spherical:
             * ρ = |OP|
             * ϕ = Angle between e3 and OP
             * θ = Polar angle
             * 
             * ρ = sqrt(x^2 + y^2 + z^2)
             * cot(ϕ) = y/r
             * ϕ = arccot(y / sqrt(x^2 + z^2))
             * θ = arctan(x/z)
             */

            this.rho = rectangular.Magnitude();
            this.phi = Math.Acot(rectangular.y / Math.Sqrt(rectangular.x*rectangular.x + rectangular.z*rectangular.z));
            this.theta = Math.Atan(rectangular.x / rectangular.z);
        }

        public Vector3s RotateZenith(double radians)
        {
            return new Vector3s(rho, phi + radians, theta);
        }

        public Vector3s RotatePolar(double radians)
        {
            return new Vector3s(rho, phi, theta + radians);
        }
    }
}
