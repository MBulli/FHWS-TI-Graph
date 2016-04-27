using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class EulerProblemSolver
    {
        private Grapher<VertexBase> graph;
        public bool IsEulerKreis { get; private set; }
        public bool IsEulerPfad { get; private set; }
        public bool IsKreis { get; set; }


        public EulerProblemSolver(Grapher<VertexBase> graph)
        {
            this.graph = graph;
            JustDoIt();
        }

        private void JustDoIt()
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
                IsEulerKreis = false;
                IsEulerPfad = false;
            }
            else if (visitedUnEven.Count == 0)
            {
                // Wenn kein Knoten ungeraden Grads ist, ist der Graph ein EulerKreis und ein EulerPfad.
                IsEulerKreis = true;
                IsEulerPfad = true;
            }
            else if (visitedUnEven.Count == 2)
            {
                // Wenn zwei Knoten ungeraden Grads sind, ist der Graph ein Eulerpfad.
                IsEulerPfad = true;
            }
        }
    }
}
