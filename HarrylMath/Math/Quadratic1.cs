namespace NerdFramework
{
    public class Quadratic1
    {
        double a;
        double b;
        double c;

        public Quadratic1(double a, double b, double c)
        {
            // Ax^2 + Bx + C

            this.a = a;
            this.b = b;
            this.c = c;
        }

        public double Equals(double x)
        {
            return a * x * x + b * x + c;
        }

        public void Rotate(double radians)
        {
            /* x' = xcos(theta) - ysin(theta)
             * y' = xsin(theta) + ycos(theta)
             * 
             * y = Ax^2 + Bx + C
             * 
             * s = sin(theta)
             * c = cos(theta)
             * 
             * y' = Ax'^2 + Bx' + C
             * (xs + yc) = A(xc - ys)^2 + B(xc - ys) + C
             * xs + (Ax^2 + Bx + C)c = Accx^2 - 2Acsx(Ax^2 + Bx + C) + ssA(Ax^2 + Bx + C)^2 + Bcx - Bs(Ax^2 + Bx + C) + C
             * xs + Acx^2 + Bcx + Cc = Accx^2 - 2AAcsx^3 - 2ABcsx^2 - 2ACcsx + AAAssx^4 + BBAssx^2 + 2AABssx^3 + 2AACssx^2 + 2ABCssx + ACCss + Bcx - BAsx^2 - BBsx - BCs + C
             * A' = (Acc+Dcs+Bss)
             * B' = (Ass+Bcc-Dsc)
             * C' = (C)
             * D' = (2Bsc+Dcc-2Acs-Dss)
             * E' = (Ec+Fs)
             * F' = (Fc-Es)
             * G' = (Gc+Hs)
             * H' = (Hc-Gs)
             * I' = (I)
             * J' = (J)
             */
        }
    }
}
