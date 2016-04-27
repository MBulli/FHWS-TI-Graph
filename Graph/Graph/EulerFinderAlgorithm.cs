using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class EulerFinderAlgorithm
    {
        public static bool FindEulerCycle(Grapher<VertexBase> graph)
        {
            Debug.Write("Vertices in graph: ");
            foreach (var node in graph.Vertices)
            {
                Debug.Write("" + node.Name + ", ");
            }
            Debug.WriteLine("");
            Debug.Write("Vertices discovered: ");

            var visitedEven = new List<VertexBase>();
            var visitedUnEven = new List<VertexBase>();

            graph.DepthFirstSearch(vertex =>
            {
                if (graph.Degree(vertex) % 2 == 0)
                {
                    visitedEven.Add(vertex);
                    Debug.Write("" + vertex.Name + ", ");
                }
                else
                {
                    visitedUnEven.Add(vertex);
                    Debug.Write("" + vertex.Name + ", ");
                }
            });

            Debug.WriteLine("");

            // wenn nicht zusammenhängender Graph dann weder Weg noch Kreis.
            if (visitedEven.Count + visitedUnEven.Count != graph.Vertices.Count())
            {
                return false;
            }
            else if (visitedUnEven.Count == 0)
            {
                // Wenn kein Knoten ungeraden Grads ist, ist der Graph ein EulerKreis und ein EulerPfad.
                return true;
            }
            else if (visitedUnEven.Count == 2)
            {
                // Wenn zwei Knoten ungeraden Grads sind, ist der Graph ein Eulerpfad.
                return false;
            }

            return false;
        }


        public static bool FindEulerPath(Grapher<VertexBase> graph)
        {
            Debug.Write("Vertices in graph: ");
            foreach (var node in graph.Vertices)
            {
                Debug.Write("" + node.Name + ", ");
            }
            Debug.WriteLine("");
            Debug.Write("Vertices discovered: ");

            var visitedEven = new List<VertexBase>();
            var visitedUnEven = new List<VertexBase>();

            graph.DepthFirstSearch(vertex =>
            {
                if (graph.Degree(vertex) % 2 == 0)
                {
                    visitedEven.Add(vertex);
                    Debug.Write("" + vertex.Name + ", ");
                }
                else
                {
                    visitedUnEven.Add(vertex);
                    Debug.Write("" + vertex.Name + ", ");
                }
            });

            Debug.WriteLine("");

            // wenn nicht zusammenhängender Graph dann weder Weg noch Kreis.
            if (visitedEven.Count + visitedUnEven.Count != graph.Vertices.Count())
            {
                return false;
            }
            else if (visitedUnEven.Count == 0 || visitedUnEven.Count == 2)
            {
                // Wenn kein Knoten ungeraden Grads ist, ist der Graph ein EulerKreis und ein EulerPfad.
                return true;
            }

            return false;
        }
    }
}
