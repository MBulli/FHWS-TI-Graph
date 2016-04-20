using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph {
    class EulerProblemSolver : DepthFirstSearchVisitorBase {
        private DataGraph graph;
        public bool IsEulerKreis { get; private set; }
        public bool IsEulerPfad { get; private set; }


        public EulerProblemSolver(DataGraph graph) : base(graph) {
            this.graph = graph;
            JustDoIt();
        }

        List<DataVertex> visitedEven = new List<DataVertex>();
        List<DataVertex> visitedUnEven = new List<DataVertex>();
        protected override void DiscoverVertex(DataVertex vertex) {
            
            base.DiscoverVertex(vertex);
            if (graph.Degree(vertex) % 2 == 0) {
                visitedEven.Add(vertex);
                Debug.Write("" + vertex.Name + ", ");
            } else {
                visitedUnEven.Add(vertex);
                Debug.Write("" + vertex.Name + ", ");
            }
        }
        private void JustDoIt() {
            Debug.Write("Vertices in graph: ");
            foreach (var node in graph.Vertices)
            {
                Debug.Write("" + node.Name + ", ");
            }
            Debug.WriteLine("");
            Debug.Write("Vertices discovered: ");
            this.Start();
            Debug.WriteLine("");
            // wenn nicht zusammenhängender Graph dann weder Weg noch Kreis.
            if (visitedEven.Count + visitedUnEven.Count != graph.Vertices.Count()) {
                IsEulerKreis = false;
                IsEulerPfad = false;
            } else {
                if (visitedUnEven.Count == 0) {
                    // Wenn kein Knoten ungeraden Grads ist, ist der Graph ein EulerKreis und ein EulerPfad.
                    IsEulerKreis = true;
                    IsEulerPfad = true;
                } else if (visitedUnEven.Count == 2) {
                    // Wenn zwei Knoten ungeraden Grads sind, ist der Graph ein Eulerpfad.
                    IsEulerPfad = true;
                }
            }
        }
    }
}
