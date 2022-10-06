namespace NerdFramework
{
    public class Matrix
    {
        public double[][] elements;

        public Matrix(params double[][] elements)
        {
            this.elements = elements;
        }

        public double Equals()
        {
            return 1.0;
        }
    }
}
