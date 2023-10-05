using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace machinevision
{


    class ObjLoader
    {
        public List<Vector3> Vertices { get; } = new List<Vector3>();
        public List<int> Indices { get; } = new List<int>();

        public void LoadObjFile(string filePath)
        {
            Vertices.Clear();
            Indices.Clear();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts.Length > 0)
                    {
                        if (parts[0] == "v")
                        {
                            // 정점 좌표를 파싱하여 Vertices 리스트에 추가
                            float x = float.Parse(parts[1]);
                            float y = float.Parse(parts[2]);
                            float z = float.Parse(parts[3]);
                            Vertices.Add(new Vector3(x, y, z));
                        }
                        else if (parts[0] == "f")
                        {
                            // 면 정보를 파싱하여 Indices 리스트에 추가
                            int v1 = int.Parse(parts[1].Split('/')[0]) - 1;
                            int v2 = int.Parse(parts[2].Split('/')[0]) - 1;
                            int v3 = int.Parse(parts[3].Split('/')[0]) - 1;
                            Indices.Add(v1);
                            Indices.Add(v2);
                            Indices.Add(v3);
                        }
                    }
                }
            }
        }


    }
}
