using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph {
    class Aufgabe2 {
        public static void A() {
            string euler1 = @"TestFiles\Euler1.txt";
            string euler2 = @"TestFiles\Euler2.txt";

            //A(euler1);
            A(euler2);

        }
        private static void A(string path) {
            Dictionary<string, DataVertex> vertices = new Dictionary<string, DataVertex>();
            List<DataEdge> edges = new List<DataEdge>();
            FileParser.parse(path, ref vertices, ref edges);

            var graph = new DataGraph(vertices.Values, edges);
            EulerProblemSolver euler = new EulerProblemSolver(graph);
            Debug.WriteLine("EulerSolver for File: " + path);
            Debug.WriteLine("Kreis: " + euler.IsEulerKreis);
            Debug.WriteLine("Pfad: " + euler.IsEulerPfad);
        }
    }
}
