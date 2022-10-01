using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public static class ASCIIShader
    {
        private static string ascii = " ░▒▒▓▓████";

        public static char FromAlpha(double alpha)
        {
            return ascii[(int)(alpha * (ascii.Length - 1))];
        }
    }
}
