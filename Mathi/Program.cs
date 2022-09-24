using Math = System.Math;
using Ray3Sector = HarrylMath.Ray3Sector;
using Ray3 = HarrylMath.Ray3;
using Vector3 = HarrylMath.Vector3;
using Triangle3 = HarrylMath.Triangle3;

namespace Mathi
{
    class Program
    {
        static void Main(string[] args)
        {
            Ray3Sector camera = new Ray3Sector(new Ray3(Vector3.Zero, Vector3.zAxis), 2.0, (1080 / 1920) * 2.0);
            Triangle3 triangle = new Triangle3(new Vector3(10, 0, 10), new Vector3(0, 10, 10), new Vector3(-10, 0, 10));

            int width = 120;
            int height = 30;
            for (int i = 0; i < 500; i++)
            {
                System.Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
            }
            while (true)
            {
                for (int y = 0; y < height; y++)
                {
                    string line = "";
                    for (int x = 0; x < width; x++)
                    {
                        if (triangle.Meets(camera.Ray(x / height, y / height)))
                            line += "█";
                        else
                            line += "░";
                    }
                    System.Console.WriteLine(line);
                }
                System.Threading.Thread.Sleep(1000 / 30);
            }
        }
    }
}
