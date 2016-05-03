using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    /// <summary>
    /// Takes a weighted graph and returns a minimal spanning tree.
    /// </summary>
    class JarnikAlgorithm
    {
        public static void Compute(Grapher<VertexBase> graph)
        {
            var T = new Grapher<VertexBase>(graph.IsDirected);
            var A = new WeightedAdjacencyMatrix<VertexBase>(graph);

            var start = graph.AnyVertex();
            T.AddVertex(start);

            while (T.Vertices.Count() != graph.Vertices.Count())
            {
                double globalMinWeight = double.PositiveInfinity;
                string gloablaMinVertex0 = null;
                string gloablaMinVertex1 = null;

                // Find min edge of all vertices in T
                foreach (var v in T.Vertices)
                {
                    string localMinVertex = A.MinimumEdge(v.Name);
                    if (localMinVertex != null)
                    {
                        double? weight = A.Weight(v.Name, localMinVertex);

                        if (weight.Value < globalMinWeight)
                        {
                            globalMinWeight = weight.Value;
                            gloablaMinVertex0 = v.Name;
                            gloablaMinVertex1 = localMinVertex;
                        }
                    }
                }

                // Add min edge and vertex
                T.AddVertex(graph.VertexForName(gloablaMinVertex1));
                T.AddEdge(graph.GetEdge(gloablaMinVertex0, gloablaMinVertex1).First());
                // 
                A.RemoveAllEdges(gloablaMinVertex0);
            }
        }
    }
}
