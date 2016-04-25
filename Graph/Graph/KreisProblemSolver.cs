using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class KreisProblemSolver
    {
        public static bool KreisSuche(Grapher graph)
        {
            if (graph.IsEmpty)
                return false;

            HashSet<Vertex> discoveredVertices = new HashSet<Vertex>();
            var root = graph.Vertices.First();
            DiscoverVertex(discoveredVertices, root);

            foreach (var v in graph.Neighbours(root))
            {
                if (NaechsterNachbar(graph, discoveredVertices, root, v))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool NaechsterNachbar(Grapher graph, HashSet<Vertex> discoveredVertices, Vertex source, Vertex target)
        {   
            if (IsVertexDiscovered(discoveredVertices, target))
                return true;

            DiscoverVertex(discoveredVertices, target);

            foreach (var v in graph.Neighbours(target))
            {
                if (v.Name == source.Name)
                    continue;

                if (NaechsterNachbar(graph, discoveredVertices, target, v))
                {
                    return true;
                }
            }
            
            return false;
        }

        private static bool IsVertexDiscovered(HashSet<Vertex> discoveredVertices, Vertex v)
        {
            return discoveredVertices.Contains(v);
        }

        private static void DiscoverVertex(HashSet<Vertex> discoveredVertices, Vertex v)
        {
            discoveredVertices.Add(v);
        }
    }
}

            

