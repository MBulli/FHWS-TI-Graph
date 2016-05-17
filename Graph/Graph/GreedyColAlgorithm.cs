using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class GreedyColAlgorithm
    {
        public static Grapher<VertexBase> ColorVertices(Grapher<VertexBase> graphToColor)
        {
            var graph = graphToColor.Clone();

            graph.Vertices.ForEach(v => v.Color = int.MaxValue);

            foreach (var v in graph.Vertices)
            {
                var adjacentColors = from i in graph.Neighbours(v)
                                     where i.Color != int.MaxValue
                                     select i.Color;

                v.Color = Enumerable.Range(0, adjacentColors.MaxOrDefault() + 2)
                                    .Except(adjacentColors)
                                    .Min();
            }
                       
            return graph;
        }

        public static Grapher<VertexBase> ColorVerticesVar(Grapher<VertexBase> graphToColor)
        {
            var graph = graphToColor.Clone();

            graph.Vertices.ForEach(v => v.Color = int.MaxValue);

            VertexBase vertex;
            while ((vertex = graph.Vertices.FirstOrDefault(v => v.Color == int.MaxValue)) != null)
            {
                var adjacentColors = from i in graph.Neighbours(vertex)
                                     where i.Color != int.MaxValue
                                     select i.Color;

                vertex.Color = Enumerable.Range(0, adjacentColors.MaxOrDefault() + 2)
                                         .Except(adjacentColors)
                                         .Min();
            }

            return graph;
        }

        public static Grapher<VertexBase> ColorVerticesVarRandom(Grapher<VertexBase> graphToColor)
        {
            var graph = graphToColor.Clone();

            graph.Vertices.ForEach(v => v.Color = int.MaxValue);

            VertexBase vertex;
            while ((vertex = graph.Vertices.Where(v => v.Color == int.MaxValue).RandomElementOrDefault()) != null)
            {
                var adjacentColors = from i in graph.Neighbours(vertex)
                                     where i.Color != int.MaxValue
                                     select i.Color;

                vertex.Color = Enumerable.Range(0, adjacentColors.MaxOrDefault() + 2)
                                         .Except(adjacentColors)
                                         .Min();
            }

            return graph;
        }
    }
}
