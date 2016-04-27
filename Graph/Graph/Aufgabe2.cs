using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class Aufgabe2
    {
        public static void A()
        {
            if (false)
            {
                var result = A(@"TestFiles\Euler1.txt");
                Assert.IsFalse(result.HasEulerCycle);
                Assert.IsFalse(result.HasEulerPath);
            }

            if (false)
            {
                var result = A(@"TestFiles\Euler2.txt");
                Assert.IsFalse(result.HasEulerCycle);
                Assert.IsTrue(result.HasEulerPath);
            }

            if(false)
            {
                var result = A(@"TestFiles\keinKreis.txt");
                Assert.IsFalse(result.HasCycle);
            }

            if(true)
            {
                var result = A(@"TestFiles\Dijkstra.txt");
            }

        }

        private static Aufgabe2Results A(string path)
        {
            var graph = FileParser.Parse(path);

            Debug.WriteLine("EulerSolver for File: " + path);

            var result = new Aufgabe2Results();
            result.HasEulerCycle = EulerFinderAlgorithm.FindEulerCycle(graph);
            result.HasEulerPath = EulerFinderAlgorithm.FindEulerPath(graph);
            result.HasCycle = CycleFinderAlgorithm.FindCycle(graph);

            var startVertice = graph.Vertices.ElementAt(0);
            var endVertice = graph.Vertices.ElementAt(4);

            result.shortestPath = DijkstraAlgorithm.FindShortestPath(graph, graph.Vertices.ElementAt(0), graph.Vertices.ElementAt(4));

            Debug.WriteLine("Eulerkreis: " + result.HasEulerCycle);
            Debug.WriteLine("Eulerpfad: " + result.HasEulerPath);
            Debug.WriteLine("Kreis: " + result.HasCycle);

            Debug.WriteLine("Shortest Dijkstra Path From " + startVertice.Name + "to " + endVertice.Name);

            foreach (var v in result.shortestPath)
            {
                Debug.Write(v.Name + " ");
            }

            return result;
        }
    }

    public class Aufgabe2Results
    {
        public bool HasEulerCycle;
        public bool HasEulerPath;
        public bool HasCycle;
        public List<VertexBase> shortestPath;
    }
}
