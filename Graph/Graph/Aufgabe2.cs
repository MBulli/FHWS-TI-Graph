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
                Assert.IsFalse(result.IsEulerKreis);
                Assert.IsFalse(result.IsEulerPfad);
            }

            if (false)
            {
                var result = A(@"TestFiles\Euler2.txt");
                Assert.IsFalse(result.IsEulerKreis);
                Assert.IsTrue(result.IsEulerPfad);
            }

            if(true)
            {
                var result = A(@"TestFiles\KeinKreis.txt");
                Assert.IsTrue(result.IsKreis);
            }

        }



        private static EulerProblemSolver A(string path)
        {
            var vertices = new Dictionary<string, Vertex>();
            var edges = new List<Edge>();
            FileParser.parse(path, ref vertices, ref edges);

            var graph = new Grapher(false, vertices, edges);
            Debug.WriteLine("EulerSolver for File: " + path);

            EulerProblemSolver euler = new EulerProblemSolver(graph);

            euler.IsKreis = KreisProblemSolver.KreisSuche(graph);

            Debug.WriteLine("Eulerkreis: " + euler.IsEulerKreis);
            Debug.WriteLine("Eulerpfad: " + euler.IsEulerPfad);
            Debug.WriteLine("Kreis: " + euler.IsKreis);

            return euler;
        }
    }
}
