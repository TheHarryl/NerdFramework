using System;
using System.Collections.Generic;
using System.Text;

namespace NerdFramework
{
    public class ImageParser
    {
        public static Texture2 FromFile(string fileLocation)
        {
            /* Portable Network Graphics (PNG)
             * Specifications
             * https://www.w3.org/TR/PNG/
             */
            if (fileLocation.ToLower().EndsWith(".png"))
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);

                foreach (string line in lines)
                {
                    
                }
            }

            return Texture2.None;
        }
    }
}
