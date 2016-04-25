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
            if (true)
            {
                var result = A(@"TestFiles\Euler1.txt");
                AssertIsFalse(result.IsEulerKreis);
                AssertIsFalse(result.IsEulerPfad);
            }

            if (false)
            {
                var result = A(@"TestFiles\Euler2.txt");
                AssertIsFalse(result.IsEulerKreis);
                AssertIsTrue(result.IsEulerPfad);
            }

        }

        private static void AssertIsTrue(bool value)
        {
            if (!value)
                throw new Exception();
        }

        private static void AssertIsFalse(bool value)
        {
            if (value)
                throw new Exception();
        }

        private static EulerProblemSolver A(string path)
        {
            var vertices = new Dictionary<string, Vertex>();
            var edges = new List<Edge>();
            FileParser.parse(path, ref vertices, ref edges);

            var graph = new Grapher(false, vertices, edges);
            Debug.WriteLine("EulerSolver for File: " + path);

            EulerProblemSolver euler = new EulerProblemSolver(graph);

            Debug.WriteLine("Eulerkreis: " + euler.IsEulerKreis);
            Debug.WriteLine("Eulerpfad: " + euler.IsEulerPfad);

            return euler;
        }
    }
}
