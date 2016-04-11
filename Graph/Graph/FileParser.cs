using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class FileParser
    {
        public static void parse(string path, ref Dictionary<string, DataVertex> vertices, ref List<DataEdge> edges)
        {
            string[] content = File.ReadAllLines(path);
            foreach(string line in content)
            {
                string[] subs = line.Split(' ');
                if(subs.Length <= 1)
                {
                    continue;
                }
                if(subs[0] == "knoten")
                {
                    string name = subs[1];
                    DataVertex vertex = new DataVertex(name);
                    if(subs.Length == 3 && subs[2].Length > 0)
                    {
                        vertex.Data = subs[2];
                    }
                    if (vertices.ContainsKey(name))
                    {
                        throw new InvalidOperationException("a vertex with the name " + name + " already exists");
                    }
                    else
                    {
                        vertices[name] = vertex;
                    }
                }
                else if(subs[0] == "kante")
                {
                    string from = subs[1];
                    string to = subs[2];
                    double gewicht = 1.0;
                    if (subs.Length == 4 && subs[3].Length > 0)
                    {
                        gewicht = double.Parse(subs[3]);
                    }
                    DataEdge edge = new DataEdge(vertices[from], vertices[to], gewicht);
                    edges.Add(edge);
                }
            }
        }
    }
}
