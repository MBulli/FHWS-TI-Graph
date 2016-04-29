using C5;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class DijkstraAlgorithm
    {
        public static List<VertexBase> FindShortestPath(Grapher<VertexBase> graph, VertexBase startVertex, VertexBase endVertex)
        {
            Grapher<DijkstraVertex> G = ConvertToDijkstraGraph(graph);
            DijkstraVertex s = G.VertexForName(startVertex.Name);
            DijkstraVertex n = G.VertexForName(endVertex.Name);

            // Active set
            var A = new List<DijkstraVertex>();

            // Non-active set
            var N = new List<DijkstraVertex>();

            var predecessorList = new Dictionary<DijkstraVertex, DijkstraVertex>();
            var lastVertex = s;

            foreach(var p in G.Vertices)
            {
                predecessorList.Add(p,null);
            }

            // Init
            s.Distance = 0;
            A.AddRange(G.Vertices);

            //var handleMap = A.AddRange(G.Vertices);
            for (int i = 0; i < A.Count;)
            {
                var v = A.ElementAt(i);
                if (v.Distance != double.PositiveInfinity)
                {
                    double min = GetMinElement(A).Distance;

                    for(int j = 0; j < A.Count; j++)
                    {
                        if(A.ElementAt(j).Distance == min)
                        {
                            N.Add(A.ElementAt(j));
                            i++;
                        }
                    }

                    foreach (var d in N) // N needs to be cleared after each iteration
                    {
                        Debug.WriteLine("Node finished: " + d.Name + ", Distance:" + d.Distance);
                        A.Remove(d);
                        i--;
                    }

                    foreach (var e in G.Edges)
                    {
                        if (N.Contains(e.V1) && A.Contains(e.V0))
                        {
                            e.V0.Distance = Math.Min(e.V0.Distance, e.V1.Distance + e.Weight);
                            predecessorList[e.V0] = e.V1; //Add actual predecessor to e.v0
                            Debug.WriteLine("New Distance for " + e.V0.Name + ": " + e.V0.Distance);

                        }
                        else if (N.Contains(e.V0) && A.Contains(e.V1))
                        {
                            e.V1.Distance = Math.Min(e.V1.Distance, e.V0.Distance + e.Weight);
                            predecessorList[e.V1] = e.V0; //Add actual predecessor to e.v1
                            Debug.WriteLine("New Distance for " + e.V1.Name + ": " + e.V1.Distance);
                        }
                    }
                    N.Clear();
                }
            }

            List<VertexBase> shortestPath = new List<VertexBase>();
            var actualPredecessor = predecessorList[n];

            if (predecessorList[n].Name == s.Name) // Path only has EndVertex and StartVertex -> finished!
            {
                shortestPath.Add(n);
                shortestPath.Add(s);

                return shortestPath;
            }
            else
            {
                shortestPath.Add(n);

                // Find shortest path to endVertex
                while (actualPredecessor.Name != s.Name)
                {
                    if (predecessorList[actualPredecessor].Name == s.Name) // Predecessor of actual predecessor is s -> finished!
                    {
                        shortestPath.Add(actualPredecessor);
                        shortestPath.Add(s);

                        return shortestPath;
                    }
                    else
                    {
                        shortestPath.Add(actualPredecessor);
                    }

                    actualPredecessor = predecessorList[actualPredecessor];
                }
            }

            return null;
        }

        private static DijkstraVertex GetMinElement(List<DijkstraVertex> heap)
        {
            DijkstraVertex vertex = new DijkstraVertex("", null, double.PositiveInfinity);

            foreach (var item in heap)
            {
                if(item.Distance < vertex.Distance)
                {
                    vertex = item;
                }
            }

            return vertex;
        }

        private static Grapher<DijkstraVertex> ConvertToDijkstraGraph(Grapher<VertexBase> sourceGraph)
        {
            return sourceGraph.ConvertVertices(oldVertex => new DijkstraVertex(oldVertex.Name, oldVertex.Data, double.PositiveInfinity));
        }

        class DijkstraVertex : VertexBase
        {
            public double Distance;

            public DijkstraVertex(string name, string data, double distance)
                : base(name, data)
            {
                this.Distance = distance;
            }
        }

        class DijkstraVertexComparer : IComparer<DijkstraVertex>
        {
            public int Compare(DijkstraVertex x, DijkstraVertex y)
            {
                return x.Distance.CompareTo(y.Distance);
            }
        }
    }
}
