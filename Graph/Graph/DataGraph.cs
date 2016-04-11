using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class DataGraph : BidirectionalGraph<DataVertex, DataEdge>
    {
        public DataGraph()
            : this(null, null)
        { }

        public DataGraph(IEnumerable<DataVertex> vertices = null, IEnumerable<DataEdge> edges = null)
        {
            if (vertices != null)
                AddVertexRange(vertices);

            if (edges != null)
                AddEdgeRange(edges);
        }
    }
}
