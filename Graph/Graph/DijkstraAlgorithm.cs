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
        public static List<VertexBase> FindShortestPath(Grapher<VertexBase> graph)
        {
            var Q = new IntervalHeap<DijkstraVertex>(new DijkstraVertexComparer());
            

            return null;
        }

        private static Grapher<DijkstraVertex> ConvertToDijkstraGraph(Grapher<VertexBase> sourceGraph)
        {
            return sourceGraph.ConvertVertices(oldVertex => new DijkstraVertex(oldVertex.Name, oldVertex.Data, null));
        }

        class DijkstraVertex : VertexBase
        {
            public int? Priority;

            public DijkstraVertex(string name, string data, int? priority)
                : base(name, data)
            {
                this.Priority = priority;
            }
        }

        class DijkstraVertexComparer : IComparer<DijkstraVertex>
        {
            public int Compare(DijkstraVertex x, DijkstraVertex y)
            {
                if (x.Priority == null && y.Priority == null)
                    return 0;
                else if (x.Priority ==  null)
                    return -1;
                else if (y.Priority == null)
                    return 1;
                else
                    return x.Priority.Value.CompareTo(y.Priority.Value);
            }
        }
    }
}
