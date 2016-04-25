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
        public static void parse(string path, ref Dictionary<string, Vertex> vertices, ref List<Edge> edges)
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
                    string data = null;

                    if (vertices.ContainsKey(name))
                        throw new InvalidOperationException("a vertex with the name " + name + " already exists");

                    if (subs.Length == 3 && subs[2].Length > 0)
                    {
                        data = subs[2];
                    }

                    vertices[name] = new Vertex(name, data);
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

                    Edge edge = new Edge(vertices[from], vertices[to], gewicht);
                    edges.Add(edge);
                }
            }
        }
    }
}
