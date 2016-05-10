using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Grapher<TVertex> : ICloneable
        where TVertex : VertexBase
    {
        protected readonly Dictionary<string, TVertex> vertices = new Dictionary<string, TVertex>();
        protected readonly HashSet<EdgeBase<TVertex>> edges = new HashSet<EdgeBase<TVertex>>();

        public IEnumerable<TVertex> Vertices { get { return vertices.Values; } }
        public IEnumerable<EdgeBase<TVertex>> Edges { get { return edges; } }
        public TVertex this[string name] { get { return vertices[name]; } }

        public readonly bool IsDirected = false;

        public bool IsEmpty { get { return vertices.Count == 0; } }

        public Grapher(bool directed = false)
        {
            this.IsDirected = directed;
        }

        public Grapher(bool directed, IDictionary<string, TVertex> vertices, IEnumerable<EdgeBase<TVertex>> edges)
            : this(directed)
        {
            this.vertices = new Dictionary<string, TVertex>(vertices);
            this.edges = new HashSet<EdgeBase<TVertex>>(edges);
        }

        protected Grapher(Grapher<TVertex> g)
        {
            this.IsDirected = g.IsDirected;
            this.vertices = new Dictionary<string, TVertex>(g.vertices);
            this.edges = new HashSet<EdgeBase<TVertex>>(g.edges);
        }

        public TVertex AnyVertex()
        {
            return vertices.FirstOrDefault().Value;
        }

        public bool ContainsVertex(string name)
        {
            return vertices.ContainsKey(name);
        }

        public TVertex VertexForName(string name)
        {
            return vertices[name];
        }

        public TVertex AddVertex(TVertex v)
        {
            vertices.Add(v.Name, v);
            return v;
        }
        public void AddVertices(IEnumerable<TVertex> newVertices)
        {
            foreach (var v in newVertices)
                AddVertex(v);
        }

        public void AddEdge(EdgeBase<TVertex> e)
        {
            edges.Add(e);
        }

        public virtual EdgeBase<TVertex> AddEdge(TVertex v0, TVertex v1, double weight = 0)
        {
            var e = new EdgeBase<TVertex>(v0, v1, weight);
            AddEdge(e);
            return e;
        }

        public EdgeBase<TVertex> AddEdge(string name0, string name1, double weight = 0)
        {
            var v0 = VertexForName(name0);
            var v1 = VertexForName(name1);

            return AddEdge(v0, v1, weight);
        }

        public void AddEdges(IEnumerable<EdgeBase<TVertex>> newEdges)
        {
            foreach(var e in newEdges)
            {
                AddEdge(e);
            }
        }

        public IEnumerable<EdgeBase<TVertex>> GetEdge(TVertex v0, TVertex v1)
        {
            return from e in Edges
                   where (e.V0 == v0 && e.V1 == v1) || (e.V0 == v1 && e.V1 == v0)
                   select e;
        }

        public IEnumerable<EdgeBase<TVertex>> GetEdge(string v0, string v1)
        {
            return from e in Edges
                   where (e.V0.Name == v0 && e.V1.Name == v1) || (e.V0.Name == v1 && e.V1.Name == v0)
                   select e;
        }

        public Grapher<TNewVertex> ConvertVertices<TNewVertex>(Func<TVertex, TNewVertex> factory)
            where TNewVertex : VertexBase
        {
            var convertedGraph = new Grapher<TNewVertex>();

            var vertices = this.Vertices.ToDictionary(v => v.Name, v => factory(v));

            convertedGraph.AddVertices(vertices.Values);
            convertedGraph.AddEdges(this.Edges.Select(e => new EdgeBase<TNewVertex>(vertices[e.V0.Name], vertices[e.V1.Name], e.Weight)));

            return convertedGraph;
        }

        public bool Adjacent(TVertex v0, TVertex v1)
        {
            return Adjacent(v0.Name, v1.Name);
        }

        public bool Adjacent(string k, string l)
        {
            if (IsDirected)
            {
                return Edges.Any(e => {
                    return e.V0.Name == k && e.V1.Name == l;
                });
            }
            else
            {
                return Edges.Any((e) => {
                    return (e.V0.Name == k && e.V1.Name == l)
                        || (e.V0.Name == l && e.V1.Name == k);
                });
            }
        }

        public IEnumerable<TVertex> Neighbours(TVertex vertex)
        {
            if (IsDirected)
            {
                return from e in Edges
                       where e.V0 == vertex
                       select e.V1;
            }
            else
            {
                return from e in Edges
                       where e.V0 == vertex || e.V1 == vertex
                       let v = (e.V0 == vertex ? e.V1 : e.V0)
                       select v;
            }
        }

        public int MaxDegree()
        {
            return Vertices.Max(v => Degree(v));
        }

        public int Degree(TVertex vertex)
        {
            return Neighbours(vertex).Count();
        }

        public int Size()
        {
            return vertices.Count;
        }

        public int NumberOfEdges()
        {
            return edges.Count;
        }

        public bool IsComplete(int k)
        {
            int n = vertices.Count;
            return edges.Count == (n * (n - 1)) / 2;
        }

        //public Grapher<TVertex> Union(Grapher<TVertex> other)
        //{
        //    var A = this.AdjacencyMatrix();
        //    var B = other.AdjacencyMatrix();
        //    return A.Union(B).AsGraph(IsDirected);
        //}
        //
        //public Grapher<TVertex> Complement()
        //{
        //    return this.AdjacencyMatrix().Complement().AsGraph(IsDirected);
        //}

        public AdjacencyMatrix<TVertex> AdjacencyMatrix()
        {
            return new AdjacencyMatrix<TVertex>(this);
        }

        public bool HasCycle()
        {
            var clone = new Grapher<TVertex>(this);

            while (clone.RemoveLeafs() > 0) { /* nothing to do */ }

            return clone.Size() != 0;
        }

        protected int RemoveLeafs()
        {
            int count = 0;
            Vertices.Where(v => Neighbours(v).Count() <= 1)
                     .ToList()
                     .ForEach(v =>
                     {
                        count++;
                        RemoveVertex(v);
                     });

            return count;
        }

        public bool RemoveVertex(string name)
        {
            edges.RemoveWhere(e =>
            {
                return e.V0.Name == name || e.V1.Name == name;
            });

            return vertices.Remove(name);
        }

        public bool RemoveVertex(TVertex vertex)
        {
            return RemoveVertex(vertex.Name);
        }

        public bool RemoveEdge(EdgeBase<TVertex> edge)
        {
            return edges.Remove(edge);
        }

        public MultiValueDictionary<int, TVertex> DepthFirstSearch(Action<TVertex> processVertexPreorder)
        {
            return DepthFirstSearch(AnyVertex(), processVertexPreorder);
        }

        public MultiValueDictionary<int, TVertex> DepthFirstSearch(TVertex start, Action<TVertex> processVertexPreorder)
        {
            var components = new MultiValueDictionary<int, TVertex>();
            var visited = new HashSet<TVertex>();

            visited.Add(start);
            processVertexPreorder(start);

            int c = -1;
            foreach (var n in Neighbours(start))
            {
                if (!visited.Contains(n))
                {
                    c++;
                    var stack = new Stack<TVertex>();
                    stack.Push(n);

                    // visit(n)
                    while (stack.Count != 0)
                    {
                        var v = stack.Pop();

                        if (!visited.Contains(v))
                        {
                            visited.Add(v);
                            components.Add(c, v);
                            processVertexPreorder(v);

                            stack.Push(Neighbours(v));
                        }
                    }
                }
            }

            return components;
        }

        public MultiValueDictionary<int, TVertex> BreadthFirstSearch(TVertex start, Action<TVertex> processVertex = null)
        {
            if (processVertex == null)
                processVertex = _ => { };

            var components = new MultiValueDictionary<int, TVertex>();
            var visited = new HashSet<TVertex>();
            visited.Add(start);

            int c = -1;
            foreach (var v in Neighbours(start))
            {
                if (!visited.Contains(v))
                {
                    c++;
                    var queue = new Queue<TVertex>();
                    queue.Enqueue(v);

                    while (queue.Count > 0)
                    {
                        var u = queue.Dequeue();

                        if (!visited.Contains(u))
                        {
                            visited.Add(u);
                            components.Add(c, u);
                            processVertex(u);

                            foreach (var vv in Neighbours(u))
                            {
                                if (!visited.Contains(vv))
                                {
                                    // predecessor[u] <- v;
                                    queue.Enqueue(vv);
                                }
                            }
                        }
                    }
                }
            }

            return components;
        }

        public string AsDotLanguage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} G {{", this.IsDirected ? "digraph" : "graph");
            sb.AppendLine();

            foreach (var vertex in this.Vertices)
            {
                sb.AppendLineFormat("{0};", vertex.Name);
            }

            foreach (var edge in this.Edges)
            {
                sb.AppendLineFormat("{0} {1} {2};", edge.V0.Name, this.IsDirected ? "->" : "--", edge.V1.Name);
            }

            sb.AppendLine("}");
            return sb.ToString();
        }


        object ICloneable.Clone() => Clone();

        public Grapher<TVertex> Clone()
        {
            return new Grapher<TVertex>(this);
        }
    }

    public class VertexBase
    {
        public readonly string Name;
        public string Data;
        public int Color;

        public VertexBase(string name, string data = null, int color = 0)
        {
            this.Name = name;
            this.Data = data;
            this.Color = color;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class EdgeBase<TVertex>
        where TVertex : VertexBase
    {
        public readonly TVertex V0;
        public readonly TVertex V1;
        public double Weight;
        public int Color;

        public EdgeBase(TVertex v0, TVertex v1, double weight = 0, int color = 0)
        {
            this.V0 = v0;
            this.V1 = v1;
            this.Weight = weight;
            this.Color = color;
        }

        public override string ToString()
        {
            return string.Format("{0} -{1}- {2}", V0.Name, Weight, V1.Name);
        }
    }
}
