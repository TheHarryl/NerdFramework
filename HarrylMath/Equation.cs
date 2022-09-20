using System.Collections.Generic;

namespace HarrylMath
{
    class Term
    {
        public static Term One = new Term(1);
        public static Term Zero = new Term(0);

        public double coefficient;
        public List<string> variables = new List<string>();

        public Term(double coefficient, List<string>? variables = null)
        {
            this.coefficient = coefficient;

            if (variables != null)
                this.variables = variables;
        }

        public static bool SameVariables(Term a, Term b)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Term term &&
                   coefficient == term.coefficient &&
                   EqualityComparer<List<string>>.Default.Equals(variables, term.variables);
        }

        public override int GetHashCode()
        {
            return System.HashCode.Combine(coefficient, variables);
        }

        public static Term operator +(Term a, Term b)
        {
            if (!SameVariables(a, b)) return Zero;
            return new Term(a.coefficient + b.coefficient, a.variables);
        }
        public static Term operator -(Term a, Term b)
        {
            if (!SameVariables(a, b)) return Zero;
            return new Term(a.coefficient - b.coefficient, a.variables);
        }
        public static Term operator *(Term a, Term b)
        {
            if (!SameVariables(a, b)) return Zero;
            return new Term(a.coefficient * b.coefficient, a.variables);
        }
        public static Term operator /(Term a, Term b)
        {
            if (!SameVariables(a, b)) return Zero;
            return new Term(a.coefficient / b.coefficient, a.variables);
        }
        public static bool operator ==(Term a, Term b)
        {
            return a.coefficient == b.coefficient && SameVariables(a, b);
        }
        public static bool operator !=(Term a, Term b)
        {
            return a.coefficient != b.coefficient || !SameVariables(a, b);
        }
    }

    class Equation
    {
        public List<Term> terms;

        public Equation(List<Term> terms)
        {
            this.terms = terms;
        }

        public double Equals(Dictionary<string, double> variables)
        {
            double x1 = 0.0;

            foreach (Term term in terms)
            {
                double t = term.coefficient;
                foreach (string variable in term.variables)
                {
                    if (variables.ContainsKey(variable))
                        t *= variables[variable];
                }

                x1 += t;
            }

            return x1;
        }

        public void Add(Term x)
        {
            foreach (Term term in terms)
            {
                if (Term.SameVariables(term, x))
                {
                    term.coefficient += x.coefficient;
                    return;
                }
            }

            terms.Add(x);
        }

        public void Subtract(Term x)
        {
            Add(Term.Zero - x);
        }

        public void Multiply(Term x)
        {
            foreach (Term term in terms)
            {
                term.coefficient *= x.coefficient;
                foreach (string variable in x.variables)
                {
                    term.variables.Add(variable);
                }
            }
        }

        public void Divide(Term x)
        {
            Multiply(Term.One / x);
        }
    }
}
