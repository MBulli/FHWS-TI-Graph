using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Blatt2Aufgabe1
    {
        public static void B()
        {
            var graph = FileParser.Parse(@"TestFiles\Dijkstra.txt");

            graph = GreedyColAlgorithm.ColorGraph(graph);
        }
    }
}
