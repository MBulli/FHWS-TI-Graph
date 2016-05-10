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
        public readonly int Color;

        public VisualEdge(VisualVertex source, VisualVertex target, double weight = 1, int color = 0)
            : base(source, target, weight)
        {
            this.Color = color;
        }

        public override string ToString()
        {
            return Weight.ToString();
        }
    }

    class ColorPalette
    {
        public class ColorInfo
        {
            public readonly SolidColorBrush VertexBackground;
            public readonly SolidColorBrush VertexForeground;
            public readonly SolidColorBrush EdgeStroke;

            public ColorInfo(SolidColorBrush bg, SolidColorBrush fg, SolidColorBrush edgeStroke)
            {
                VertexBackground = bg;
                VertexForeground = fg;
                EdgeStroke = edgeStroke;
            }
        }

        public static int Length => Colors.Length;

        public static ColorInfo Get(int index)
        {
            if (index < 0 || index > Length)
                throw new InvalidOperationException($"Color index {index} is out of range [0, {Length - 1}].");

            return Colors[index];
        }

        public static readonly ColorInfo[] Colors = new ColorInfo[]
        {
            // (Vertex BG, Vertex FB, Edge Stroke)
            new ColorInfo(Brushes.LightGray, Brushes.Black, Brushes.Black),
            new ColorInfo(Brushes.Blue,      Brushes.White, Brushes.Blue),
            new ColorInfo(Brushes.Red,       Brushes.White, Brushes.Red),
            new ColorInfo(Brushes.Yellow,    Brushes.Black, Brushes.Yellow),
            new ColorInfo(Brushes.Green,     Brushes.Black, Brushes.Green),
            new ColorInfo(Brushes.Black,     Brushes.White, Brushes.Black),
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
                var color = ColorPalette.Get(v.Color);

                return new VertexControl(vertexData)
                {
                    Background = color.VertexBackground,
                    Foreground = color.VertexForeground
                };
            }
            else
            {
                return new VertexControl(vertexData);
            }
        }

        public override EdgeControl CreateEdgeControl(VertexControl source, VertexControl target, object edge, bool showLabels = false, bool showArrows = true, Visibility visibility = Visibility.Visible)
        {
            var result = new VisualEdgeControl() {       
                Source = source,
                Target = target,
                Edge = edge,
                ShowLabel = showLabels,
                ShowArrows = showArrows,
                Visibility = visibility
            };

            var e = edge as VisualEdge;
            if (e != null)
            {
                result.Foreground = ColorPalette.Get(e.Color).EdgeStroke;
            }

            return result;
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
