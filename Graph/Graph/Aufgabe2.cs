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
                //Assert.IsFalse(result.IsEulerKreis);
                //Assert.IsFalse(result.IsEulerPfad);
            }

            if (false)
            {
                var result = A(@"TestFiles\Euler2.txt");
                //Assert.IsFalse(result.IsEulerKreis);
                //Assert.IsTrue(result.IsEulerPfad);
            }

            if(false)
            {
                var result = A(@"TestFiles\KeinKreis.txt");
                //Assert.IsTrue(result.IsKreis);
            }

        }



        private static EulerProblemSolver A(string path)
        {
            var graph = FileParser.Parse(path);

            Debug.WriteLine("EulerSolver for File: " + path);

            EulerProblemSolver euler = new EulerProblemSolver(graph);

            euler.IsKreis = KreisFinderAlgorithm.KreisSuche(graph);

            Debug.WriteLine("Eulerkreis: " + euler.IsEulerKreis);
            Debug.WriteLine("Eulerpfad: " + euler.IsEulerPfad);
            Debug.WriteLine("Kreis: " + euler.IsKreis);

            return euler;
        }
    }
}
