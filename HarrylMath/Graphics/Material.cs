namespace NerdFramework
{
    public enum IlluminationModel
    {
        Flat = 1,
        Specular = 2
    }

    public class Material
    {
        public Vector3 ambientColor;
        public Vector3 diffuseColor;
        public Vector3 specularColor;

        public IlluminationModel illuminationModel;

        public double shininess;
        public double alpha;

        public Texture2 textureMap;

        public Material()
        {
            ambientColor = Vector3.One;
            diffuseColor = Vector3.One;
            specularColor = Vector3.One;

            illuminationModel = IlluminationModel.Flat;

            shininess = 0.0;
            textureMap = Texture2.None;
        }
    }
}
