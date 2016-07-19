using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uebung6
{
    class Program
    {
        static void Main(string[] args)
        {
            var x1 = new Literal() { Name = "x1" };
            var x2 = new Literal() { Name = "x2" };

            // (!x1 v x2) ^ (x1 v x2) ^ (x1 v !x2) ^ (!x1 v !x2)
            var sat = new SAT()
            {
                Literale = new List<Literal>() {
                    x1, x2
                },
                Klauseln = new List<Klausel>() {
                    new Klausel () {
                        Literale = new Dictionary<Literal, bool> (){
                            { x1, true },
                            { x2, false }
                        }
                    },
                    new Klausel () {
                        Literale = new Dictionary<Literal, bool> (){
                            { x1, false },
                            { x2, false }
                        }
                    },
                    new Klausel () {
                        Literale = new Dictionary<Literal, bool> (){
                            { x1, false },
                            { x2, true }
                        }
                    },
                    new Klausel () {
                        Literale = new Dictionary<Literal, bool> (){
                            { x1, true },
                            { x2, true }
                        }
                    }
                }
            };
            MaxSatAlgo algo = new MaxSatAlgo(sat);
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(algo.ComputeProbabilistic());
            }
            Console.ReadKey();
        }
    }
}
