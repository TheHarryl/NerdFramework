using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public static class ASCIIShader
    {
        private static string ascii = "  .,:ilwW@@";

        public static char FromAlpha(double alpha)
        {
            //return ascii[(int)(alpha * (ascii.Length - 1))];
            switch (alpha)
            {
                case double a when a >= 0.4907041821:
                    return '▓';
                case double a when a >= 0.20422528454:
                    return '▒';
                case double a when a >= 0.04507034144:
                    return '░';
                default:
                    return ' ';
            }
        }
    }
}
