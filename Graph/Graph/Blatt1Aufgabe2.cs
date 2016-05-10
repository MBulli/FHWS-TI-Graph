using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph {
    class Blatt1Aufgabe2 {
        public static Grapher<VertexBase> A() {
            Aufgabe2Results result;
            if (false) {
                result = A(@"TestFiles\Euler1.txt");
                Assert.IsFalse(result.HasEulerCycle);
                Assert.IsFalse(result.HasEulerPath);
            }

            if (false) {
                result = A(@"TestFiles\Euler2.txt");
                Assert.IsFalse(result.HasEulerCycle);
                Assert.IsTrue(result.HasEulerPath);
            }

            if (false) {
                result = A(@"TestFiles\keinKreis.txt");
                Assert.IsFalse(result.HasCycle);
            }

            if (true) {
                result = A(@"TestFiles\VielGewicht.txt");
            }
            return result.Graph;
        }

        private static Aufgabe2Results A(string path) {
            var graph = FileParser.Parse(path);

            Debug.WriteLine("EulerSolver for File: " + path);

            var result = new Aufgabe2Results();
            result.HasEulerCycle = EulerFinderAlgorithm.FindEulerCycle(graph);
            result.HasEulerPath = EulerFinderAlgorithm.FindEulerPath(graph);
            result.HasCycle = CycleFinderAlgorithm.FindCycle(graph);

            var startVertice = graph["I"];
            var endVertice = graph["F"];

            result.shortestPath = DijkstraAlgorithm.FindShortestPath(graph, startVertice, endVertice);

            Debug.WriteLine("Eulerkreis: " + result.HasEulerCycle);
            Debug.WriteLine("Eulerpfad: " + result.HasEulerPath);
            Debug.WriteLine("Kreis: " + result.HasCycle);

            Debug.WriteLine("Shortest Dijkstra Path From " + startVertice.Name + " to " + endVertice.Name);

            result.shortestPath.Reverse();
            if (result.shortestPath.Count == 0) {
                Debug.WriteLine("Error!");
            } else {
                foreach (var v in result.shortestPath) {
                    Debug.Write(v.Name + " ");
                }
            }

            result.Graph = graph;

            return result;
        }
    }

    public class Aufgabe2Results {
        public bool HasEulerCycle;
        public bool HasEulerPath;
        public bool HasCycle;
        public List<VertexBase> shortestPath;
        public Grapher<VertexBase> Graph;
    }
}
