using QuickGraph;

using GraphX;
using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.PCL.Common.Models;
using GraphX.PCL.Logic.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Graph
{
    class VisualGraphArea : GraphArea<VisualVertex, VisualEdge, BidirectionalGraph<VisualVertex, VisualEdge>>
    {
        public VisualGraphArea()
        {
            this.LogicCore = new VisualGraphLogicCore();
            this.EdgeLabelFactory = new DefaultEdgelabelFactory();
            this.ControlFactory = new VisualGraphControlFactory(this);
        }

        public void GenerateGraph(Grapher<VertexBase> g)
        {
            var biGraph = ConvertGraph(g);
            base.GenerateGraph(biGraph);

            ShowAllEdgesArrows(g.IsDirected);
            SetVerticesDrag(true);
        }

        private QuickGraph.BidirectionalGraph<VisualVertex, VisualEdge> ConvertGraph(Grapher<VertexBase> graph)
        {
            var result = new QuickGraph.BidirectionalGraph<VisualVertex, VisualEdge>();

            var vertices = graph.Vertices.ToDictionary(v => v.Name, v => new VisualVertex(v.Name, v.Data, v.Color));

            result.AddVertexRange(vertices.Values);
            result.AddEdgeRange(graph.Edges.Select(e => new VisualEdge(vertices[e.V0.Name], vertices[e.V1.Name], e.Weight)));

            return result;
        }
    }

    class VisualGraphLogicCore : GXLogicCore<VisualVertex, VisualEdge, BidirectionalGraph<VisualVertex, VisualEdge>>
    {

    }

    class VisualVertex : GraphX.PCL.Common.Models.VertexBase
    {
        public readonly string Name;
        public readonly string Data;
        public readonly int Color;

        public VisualVertex(string name, string data, int color)
        {
            this.Name = name;
            this.Data = data;
            this.Color = color;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }

    class VisualEdge : GraphX.PCL.Common.Models.EdgeBase<VisualVertex>
    {
        public VisualEdge(VisualVertex source, VisualVertex target, double weight = 1)
            : base(source, target, weight)
        { }

        public override string ToString()
        {
            return Weight.ToString();
        }
    }

    class VertexColorPalette
    {
        public class ColorPair
        {
            public readonly SolidColorBrush Background;
            public readonly SolidColorBrush Foreground;

            public ColorPair(SolidColorBrush bg, SolidColorBrush fg)
            {
                Background = bg;
                Foreground = fg;
            }
        }

        public static int Length => Colors.Length;

        public static ColorPair Get(int index)
        {
            if (index < 0 || index > Length)
                throw new InvalidOperationException($"Color index {index} is out of range [0, {Length - 1}].");

            return Colors[index];
        }

        public static readonly ColorPair[] Colors = new ColorPair[]
        {
            new ColorPair(Brushes.LightGray, Brushes.Black),
            new ColorPair(Brushes.Blue, Brushes.White),
            new ColorPair(Brushes.Red, Brushes.White),
            new ColorPair(Brushes.Yellow, Brushes.Black),
            new ColorPair(Brushes.Green, Brushes.Black),
            new ColorPair(Brushes.Black, Brushes.White),
        };
    }

    class VisualGraphControlFactory : GraphControlFactory
    {
        public VisualGraphControlFactory(GraphAreaBase graphArea)
            : base(graphArea)
        {
        }

        public override VertexControl CreateVertexControl(object vertexData)
        {
            var v = vertexData as VisualVertex;

            if (v != null)
            {
                var color = VertexColorPalette.Get(v.Color);

                return new VertexControl(vertexData)
                {
                    Background = color.Background,
                    Foreground = color.Foreground
                };
            }
            else
            {
                return new VertexControl(vertexData);
            }
        }

        public override EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            return new VisualEdgeControl() {       
                Source = source,
                Target = target,
                Edge = edge,
                ShowLabel = showLabels,
                ShowArrows = showArrows,
                Visibility = visibility
            };
        }
    }

    class VisualEdgeControl : EdgeControl
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (!ShowArrows)
            {
                // Hide arrow
                EdgePointerForSource?.Hide();
                EdgePointerForTarget?.Hide();

                // Force line length to connect source/target
                _linegeometry = new PathGeometry(new[]
                {
                    new PathFigure(Source.GetCenterPosition(), new [] {
                        new LineSegment(Target.GetCenterPosition(), isStroked: true)
                    }, closed: false)
                });
                
                LinePathObject.Data = _linegeometry;
            }
        }
    }
}
