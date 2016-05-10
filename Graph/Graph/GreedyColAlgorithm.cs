using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class GreedyColAlgorithm
    {
        public static Grapher<VertexBase> ColorGraph(Grapher<VertexBase> graphToColor)
        {
            var graph = graphToColor.Clone();

            graph.Vertices.ForEach(v => v.Color = int.MaxValue);

            foreach (var v in graph.Vertices)
            {
                var incidentColors = from i in graph.Neighbours(v)
                                     where i.Color != int.MaxValue
                                     select i.Color;

                v.Color = Enumerable.Range(0, incidentColors.MaxOrDefault() + 2)
                                    .Except(incidentColors)
                                    .Min();
            }
                       
            return graph;
        }
    }
}
