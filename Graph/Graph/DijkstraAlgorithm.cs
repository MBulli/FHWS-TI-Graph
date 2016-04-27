using C5;
using System;
using System.Collections.Generic;
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
            var A = new IntervalHeap<DijkstraVertex>(new DijkstraVertexComparer());
            // Non-active set
            var N = new IntervalHeap<DijkstraVertex>(new DijkstraVertexComparer());

            var predecessorList = new Dictionary<DijkstraVertex, DijkstraVertex>();

            foreach(var p in G.Vertices)
            {
                predecessorList.Add(p,null);
            }

            // Init
            s.Distance = 0;
            N.Add(s);

            var handleMap = A.AddRange(G.Vertices);

            for (int i = 0; i < A.Count; i++)
            {
                var v = A.ElementAt(i);
                if (v.Distance != double.PositiveInfinity)
                {
                    var predecessor = A.FindMin();
                    double min = predecessor.Distance;
                    predecessorList[v] = predecessor; //Add actual predecessor to v with shortest path to v
                    v.Distance = min;
                    if (!N.Contains(v))
                    {
                        N.Add(v);
                    }


                    foreach (var d in N)
                    {
                        if(A.Contains(d))
                        {
                            A.Delete(handleMap[d]);
                            handleMap.Remove(d);
                        }
                    }

                    foreach (var e in G.Edges)
                    {
                        if (N.Contains(e.V1) && A.Contains(e.V0))
                        {
                            e.V0.Distance = Math.Min(e.V0.Distance, e.V1.Distance + e.Weight);

                        }
                        else if (N.Contains(e.V0) && A.Contains(e.V1))
                        {
                            e.V1.Distance = Math.Min(e.V1.Distance, e.V0.Distance + e.Weight);
                        }
                    }
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
