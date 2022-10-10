using System.Collections.Generic;

namespace NerdFramework
{
    public class MaterialParser
    {
        public static Dictionary<string, Material> FromFile(string fileLocation, bool overrideNormalInterpolation = false)
        {
            Dictionary<string, Material> materials = new Dictionary<string, Material>();

            if (fileLocation.ToLower().EndsWith(".obj"))
            {
                string[] lines = System.IO.File.ReadAllLines(@fileLocation);
                string currentMaterialName = "";

                foreach (string line in lines)
                {
                    Material currentMaterial = materials[currentMaterialName];
                    string[] args = line.Split(" ");
                    switch (args[0])
                    {
                        case "newmtl":
                            materials.Add(args[1], new Material());
                            currentMaterialName = args[1];
                            break;
                        case "Ka":
                            currentMaterial.ambientColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "Kd":
                            currentMaterial.diffuseColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "Ks":
                            currentMaterial.specularColor = new Vector3(double.Parse(args[1]), double.Parse(args[2]), double.Parse(args[3]));
                            break;
                        case "illum":
                            currentMaterial.illuminationModel = (IlluminationModel)int.Parse(args[1]);
                            break;
                        case "Ns":
                            currentMaterial.shininess = double.Parse(args[1]);
                            break;
                        case "d":
                            currentMaterial.alpha = double.Parse(args[1]);
                            break;
                        case "Tr":
                            currentMaterial.alpha = 1.0 - double.Parse(args[1]);
                            break;
                        case "map_Ka":
                            currentMaterial.textureMap = ImageParser.FromFile(fileLocation);
                            break;
                    }
                }
            }

            return materials;
        }
    }
}
