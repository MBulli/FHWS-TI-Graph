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
        public static Grapher<VertexBase> Parse(string path, bool directedGraph = false)
        {
            Grapher<VertexBase> result = new Grapher<VertexBase>(directed: directedGraph);
            string[] content = File.ReadAllLines(path);

            foreach(string line in content)
            {
                string[] subs = line.Split(' ');

                if(subs.Length <= 1)
                    continue;
                
                if(subs[0] == "knoten")
                {
                    string name = subs[1];
                    string data = null;

                    if (result.ContainsVertex(name))
                        throw new InvalidOperationException($"A vertex with the name {name} already exists");

                    if (subs.Length == 3 && subs[2].Length > 0)
                    {
                        data = subs[2];
                    }

                    result.AddVertex(new VertexBase(name, data));
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

                    result.AddEdge(from, to, gewicht);
                }
            }

            return result;
        }
    }
}
