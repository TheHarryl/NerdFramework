using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    public class MaterialParser
    {
        public static Dictionary<string, Material> FromFile(string fileLocation, Dictionary<string, Texture2> textures)
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material>();

            /* Material (MTL) Specifications
             * http://paulbourke.net/dataformats/mtl/
             */
            if (fileLocation.ToLower().EndsWith(".mtl"))
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);
                string currentMaterialName = "";

                foreach (string line in lines)
                {
                    string[] args = line.Split(" ");
                    switch (args[0])
                    {
                        case "newmtl":
                            materials.Add(args[1], new Material());
                            currentMaterialName = args[1];
                            break;
                        case "Ka":
                            materials[currentMaterialName].ambientColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "Kd":
                            materials[currentMaterialName].diffuseColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "Ks":
                            materials[currentMaterialName].specularColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "illum":
                            materials[currentMaterialName].illuminationModel = (IlluminationModel)int.Parse(args[1]);
                            break;
                        case "Ns":
                            materials[currentMaterialName].shininess = double.Parse(args[1]);
                            break;
                        case "d":
                            materials[currentMaterialName].alpha = double.Parse(args[1]);
                            break;
                        case "Tr":
                            materials[currentMaterialName].alpha = 1.0 - double.Parse(args[1]);
                            break;
                        case "Tf":
                            materials[currentMaterialName].alpha = (double.Parse(args[1]) + double.Parse(args[2]) + double.Parse(args[3])) / 3.0;
                            break;
                        case "map_Kd":
                            if (args[1].StartsWith("-"))
                                materials[currentMaterialName].textureMap = textures[string.Join(" ", args.Skip(5).ToArray())];
                            else
                                materials[currentMaterialName].textureMap = textures[string.Join(" ", args.Skip(1).ToArray())];
                            break;
                    }
                }
            }

            return materials;
        }
    }
}
