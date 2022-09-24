using System.Collections.Generic;

namespace HarrylMath
{
    public class EquationSystem
    {
        public List<Equation> equations;

        public EquationSystem(List<Equation> equations)
        {
            this.equations = equations;
        }

        public void AddAll(Term x)
        {
            foreach (Equation equation in equations)
            {
                equation.Add(x);
            }
        }

        public void SubtractAll(Term x)
        {
            foreach (Equation equation in equations)
            {
                equation.Subtract(x);
            }
        }

        public void MultiplyAll(Term x)
        {
            foreach (Equation equation in equations)
            {
                equation.Multiply(x);
            }
        }

        public void DivideAll(Term x)
        {
            foreach (Equation equation in equations)
            {
                equation.Divide(x);
            }
        }
    }
}
