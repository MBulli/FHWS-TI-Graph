using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class GreedyEdgeColAlgorithm
    {
        public static Grapher<VertexBase> ColorEdges(Grapher<VertexBase> graphToColor)
        {
            var graph = graphToColor.Clone();

            graph.Edges.ForEach(e => e.Color = int.MaxValue);

            foreach (var v in graph.Vertices)
            {
                foreach (var e in graph.Incidents(v).Where(x => x.Color == int.MaxValue))
                {
                    var adjacentColors = from i in graph.Incidents(v).Concat(graph.Neighbours(v).SelectMany(x => graph.Incidents(x)))
                                         where i.Color != int.MaxValue
                                         select i.Color;

                    e.Color = Enumerable.Range(0, adjacentColors.MaxOrDefault() + 2)
                                        .Except(adjacentColors)
                                        .Min();
                }
            }
                       
            return graph;
        }
    }
}
