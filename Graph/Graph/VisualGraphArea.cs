using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.PCL.Common.Models;
using GraphX.PCL.Logic.Models;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class VisualGraphArea : GraphArea<VisualVertex, VisualEdge, BidirectionalGraph<VisualVertex, VisualEdge>>
    {
        public VisualGraphArea()
        {
            this.LogicCore = new VisualGraphLogicCore();
            this.EdgeLabelFactory = new DefaultEdgelabelFactory();
        }

        public void GenerateGraph(Grapher g)
        {
            var biGraph = ConvertGraph(g);
            base.GenerateGraph(biGraph);
        }

        private QuickGraph.BidirectionalGraph<VisualVertex, VisualEdge> ConvertGraph(Grapher graph)
        {
            var result = new QuickGraph.BidirectionalGraph<VisualVertex, VisualEdge>();

            var vertices = graph.Vertices.ToDictionary(v => v.Name, v => new VisualVertex(v.Name, v.Data));

            result.AddVertexRange(vertices.Values);
            result.AddEdgeRange(graph.Edges.Select(e => new VisualEdge(vertices[e.V0.Name], vertices[e.V1.Name], e.Weight)));

            return result;
        }
    }

    class VisualGraphLogicCore : GXLogicCore<VisualVertex, VisualEdge, BidirectionalGraph<VisualVertex, VisualEdge>>
    {

    }

    class VisualVertex : VertexBase
    {
        public readonly string Name;
        public readonly string Data;

        public VisualVertex(string name, string data)
        {
            this.Name = name;
            this.Data = data;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    class VisualEdge : EdgeBase<VisualVertex>
    {
        public VisualEdge(VisualVertex source, VisualVertex target, double weight = 1)
            : base(source, target, weight)
        { }

        public override string ToString()
        {
            return Weight.ToString();
        }
    }
}
