using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class Grapher
    {
        protected readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();
        protected readonly HashSet<Edge> edges = new HashSet<Edge>();

        public IEnumerable<Vertex> Vertices { get { return vertices.Values; } }
        public IEnumerable<Edge> Edges { get { return edges; } }
        public Vertex this[string name] { get { return vertices[name]; } }

        public readonly bool IsDirected = false;

        public bool IsEmpty { get { return vertices.Count == 0; } }

        public Grapher(bool directed = false)
        {
            this.IsDirected = directed;
        }

        public Grapher(bool directed, IDictionary<string, Vertex> vertices, IEnumerable<Edge> edges)
            : this(directed)
        {
            this.vertices = new Dictionary<string, Vertex>(vertices);
            this.edges = new HashSet<Edge>(edges);
        }

        protected Grapher(Grapher g)
        {
            this.IsDirected = g.IsDirected;
            this.vertices = new Dictionary<string,Vertex>(g.vertices);
            this.edges = new HashSet<Edge>(g.edges);
        }

        public Vertex AnyVertex()
        {
            return vertices.FirstOrDefault().Value;
        }

        public Vertex VertexForName(string name)
        {
            return vertices[name];
        }

        public Vertex AddVertex(string name)
        {
            return AddVertex(new Vertex(name));
        }
        public Vertex AddVertex(Vertex v)
        {
            vertices.Add(v.Name, v);
            return v;
        }
        public void AddVerticies(IEnumerable<Vertex> newVerticies)
        {
            foreach (var v in newVerticies)
                AddVertex(v);
        }

        public virtual Edge AddEdge(Vertex v0, Vertex v1)
        {
            var e = new Edge(v0, v1);
            edges.Add(e);
            return e;
        }

        public bool Adjacent(Vertex v0, Vertex v1)
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

        public IEnumerable<Vertex> Neighbours(Vertex vertex)
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

        public int Degree(Vertex vertex)
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

        public Grapher Union(Grapher other)
        {
            var A = this.AdjacencyMatrix();
            var B = other.AdjacencyMatrix();
            return A.Union(B).AsGraph(IsDirected);
        }

        public Grapher Complement()
        {
            return this.AdjacencyMatrix().Complement().AsGraph(IsDirected);
        }

        public AdjacencyMatrix AdjacencyMatrix()
        {
            return new AdjacencyMatrix(this);
        }

        public bool HasCycle()
        {
            var clone = new Grapher(this);

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

        public bool RemoveVertex(Vertex vertex)
        {
            return RemoveVertex(vertex.Name);
        }

        public bool RemoveEdge(Edge edge)
        {
            return edges.Remove(edge);
        }

        public MultiValueDictionary<int, Vertex> DepthFirstSearch(Action<Vertex> processVertexPreorder)
        {
            return DepthFirstSearch(AnyVertex(), processVertexPreorder);
        }

        public MultiValueDictionary<int, Vertex> DepthFirstSearch(Vertex start, Action<Vertex> processVertexPreorder)
        {
            var components = new MultiValueDictionary<int, Vertex>();
            var visited = new HashSet<Vertex>();

            visited.Add(start);
            processVertexPreorder(start);

            int c = -1;
            foreach (var n in Neighbours(start))
            {
                if (!visited.Contains(n))
                {
                    c++;
                    var stack = new Stack<Vertex>();
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

        public MultiValueDictionary<int, Vertex> BreadthFirstSearch(Vertex start, Action<Vertex> processVertex = null)
        {
            if (processVertex == null)
                processVertex = _ => {};

            var components = new MultiValueDictionary<int, Vertex>();
            var visited = new HashSet<Vertex>();
            visited.Add(start);

            int c = -1;
            foreach (var v in Neighbours(start))
	        {
                if (!visited.Contains(v))
                {
                    c++;
                    var queue = new Queue<Vertex>();
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

        public static Grapher FromFile(string path = "GraphDef.txt")
        {
            var G = new Grapher();
            var verticiesLookup = new Dictionary<string, Vertex>();

            foreach (var line in System.IO.File.ReadAllLines(path))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                string[] split = line.Split(' ');

                if (split.Length == 1)
                {
                    var v = new Vertex(split[0]);
                    verticiesLookup.Add(line, v);
                    G.AddVertex(v);
                }
                else if (split.Length == 2)
                {
                    var v0 = verticiesLookup[split[0]];
                    var v1 = verticiesLookup[split[1]];

                    G.AddEdge(v0, v1);
                }
                else
                {
                    throw new Exception(string.Format("Invalid GraphDef format in line: '{0}'", line));
                }
            }

            return G;
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
    }

    public class Vertex
    {
        public readonly string Name;
        public readonly string Data;

        public Vertex(string name, string data = null)
        {
            this.Name = name;
            this.Data = data;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Edge
    {
        public readonly Vertex V0;
        public readonly Vertex V1;
        public readonly double Weight;

        public Edge(Vertex v0, Vertex v1, double weight = 0)
        {
            this.V0 = v0;
            this.V1 = v1;
            this.Weight = weight;
        }

        public override string ToString()
        {
            return string.Format("{0} -{1}- {2}", V0.Name, Weight, V1.Name);
        }
    }
}
