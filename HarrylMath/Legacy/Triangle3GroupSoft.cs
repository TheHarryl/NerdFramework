using System.Collections.Generic;
using System.Linq;

namespace NerdFramework
{
    /*public class Triangle3GroupSoft : Triangle3Collection
    {
        public Dictionary<Vector3, List<KeyValuePair<Vector3, double>>> connections = new Dictionary<Vector3, List<KeyValuePair<Vector3, double>>>();

        public Triangle3GroupSoft(List<Triangle3> triangles) : base(triangles)
        {
            foreach (Triangle3 triangle in triangles)
            {
                List<Vector3> points = new List<Vector3>() { triangle.a, triangle.b, triangle.c };
                for (int i = 0; i < points.Count; i++)
                {
                    if (!connections.ContainsKey(points[i]))
                        connections.Add(points[i], new List<KeyValuePair<Vector3, double>>());
                    Vector3 point2 = points[(i + 1) % 3];
                    Vector3 point3 = points[(i + 2) % 3];
                    connections[points[i]].Add(new KeyValuePair<Vector3, double>(point2, (point2 - points[i]).Magnitude()));
                    connections[points[i]].Add(new KeyValuePair<Vector3, double>(point3, (point3 - points[i]).Magnitude()));
                }
            }
        }

        public void Update(double delta)
        {
            for (int i = 0; i < connections.Count; i++)
            {
                Vector3 point = connections.Keys.ToList()[i];
                List<KeyValuePair<Vector3, double>> connection = connections.Values.ToList()[i];
            }
        }
    }*/
}
