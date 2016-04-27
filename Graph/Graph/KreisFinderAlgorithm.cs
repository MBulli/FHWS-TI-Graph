using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class KreisFinderAlgorithm
    {
        public static bool KreisSuche(Grapher<VertexBase> graph)
        {
            if (graph.IsEmpty)
                return false;

            HashSet<VertexBase> discoveredVertices = new HashSet<VertexBase>();
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

        private static bool NaechsterNachbar(Grapher<VertexBase> graph, HashSet<VertexBase> discoveredVertices, VertexBase source, VertexBase target)
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

        private static bool IsVertexDiscovered(HashSet<VertexBase> discoveredVertices, VertexBase v)
        {
            return discoveredVertices.Contains(v);
        }

        private static void DiscoverVertex(HashSet<VertexBase> discoveredVertices, VertexBase v)
        {
            discoveredVertices.Add(v);
        }
    }
}

            

