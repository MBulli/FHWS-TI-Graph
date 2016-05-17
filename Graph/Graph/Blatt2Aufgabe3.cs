using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Blatt2Aufgabe3
    {
        public static Grapher<VertexBase> A()
        {
            var graph = FileParser.Parse(@"TestFiles\Dijkstra.txt");

            graph = GreedyEdgeColAlgorithm.ColorEdges(graph);
            return graph;
        }
    }
}
