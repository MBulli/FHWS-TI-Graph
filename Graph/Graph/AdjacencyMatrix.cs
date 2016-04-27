using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class AdjacencyMatrix<TVertex>
        where TVertex : VertexBase
    {
        private readonly string[] names;
        private readonly bool[,] matrix;

        public bool this[int i, int j] { get { return matrix[i, j]; } }
        public int Size { get { return matrix.GetLength(0); } }
        
        public AdjacencyMatrix(string[] names, bool[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
                throw new ArgumentException("Square matrix required.");
            if (matrix.GetLength(0) != names.Length)
                throw new ArgumentException("Number of names must match number of rows in matrix.");

            this.names = names;
            this.matrix = matrix;
        }

        public AdjacencyMatrix(Grapher<TVertex> g)
        {
            var V = new List<TVertex>(g.Vertices);
            int n = V.Count;

            this.names = new string[n];
            this.matrix = new bool[n, n];

            for (int i = 0; i < n; i++)
            {
                this.names[i] = V[i].Name;

                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = g.Adjacent(V[i], V[j]);
                }
            }
        }

        public string Name(int index)
        {
            return names[index];
        }

        public static AdjacencyMatrix<TVertex> CreateCompleteMatrix(int size)
        {
            bool[,] matrix = new bool[size, size];
            string[] names = new string[size];

            for (int i = 0; i < size; i++)
            {
                names[i] = i.ToString();
                for (int j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = false;
                    }
                    else
                    {
                        matrix[i, j] = true;
                    }
                }
            }


            return new AdjacencyMatrix<TVertex>(names, matrix);
        }

        public Grapher<VertexBase> AsGraphUndirected()
        {
            var newGraph = new Grapher<VertexBase>();

            var vertices = this.names.Select(n => new VertexBase(n)).ToArray();
            newGraph.AddVertices(vertices);

            /* 
             * We iterate the matrix like that:
             * 0 1 0 1
             * - 0 1 0
             * - - 0 0
             * - - - 0 
             */
            for (int i = 0; i < Size; i++)
            {
                for (int j = i; j < Size; j++)
                {
                    if (matrix[i,j])
                    {
                        newGraph.AddEdge(vertices[i], vertices[j]);    
                    }
                }
            }

            return newGraph;
        }

        public Grapher<VertexBase> AsGraph(bool directed = false)
        {
            return directed ? AsGraphDirected() : AsGraphUndirected();
        }

        public Grapher<VertexBase> AsGraphDirected()
        {
            var newGraph = new Grapher<VertexBase>(directed: true);

            var vertices = this.names.Select(n => new VertexBase(n)).ToArray();
            newGraph.AddVertices(vertices);

            /* 
             * We iterate the matrix like that:
             * 0 1 0 1
             * 0 0 1 0
             * 0 1 0 0
             * 1 0 1 0 
             */
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (matrix[i, j])
                    {
                        newGraph.AddEdge(vertices[i], vertices[j]);
                    }
                }
            }

            return newGraph;
        }

        public AdjacencyMatrix<TVertex> Union(AdjacencyMatrix<TVertex> other)
        {
            int s0 = this.Size;
            int s1 = other.Size;
            int s = s0 + s1;

            // ensure unique names
            var newNames = UnionNames(this.names, other.names);
            bool[,] newMatrix = new bool[s, s];

            // Matrix A
            for (int i = 0; i < s0; i++)
            {
                for (int j = 0; j < s0; j++)
                {
                    newMatrix[i, j] = this.matrix[i, j];
                }
            }

            // Matrix B
            for (int i = s0; i < s; i++)
            {
                for (int j = s0; j < s; j++)
                {
                    newMatrix[i, j] = other.matrix[i - s0, j - s0];
                }
            }

            return new AdjacencyMatrix<TVertex>(newNames.ToArray(), newMatrix);
        }

        private string[] UnionNames(string[] A, string[] B)
        {
            List<string> result = new List<string>(A);

            foreach (var nameB in B)
            {
                string newName = nameB;

                while (result.Contains(newName))
                {
                    newName += "'";
                }

                result.Add(newName);
            }

            return result.ToArray();
        }


        public AdjacencyMatrix<TVertex> Complement()
        {
            int s = this.Size;
            bool[,] newMatrix = new bool[s, s];

            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    newMatrix[i, j] = !this.matrix[i, j];
                }
            }

            return new AdjacencyMatrix<TVertex>(this.names, newMatrix);
        }

        public string ToPrettyString()
        {
            StringBuilder sb = new StringBuilder();

            // write headline

            int maxLenght = this.names.Max(n => n.Length) + 1;

            for (int i = 0; i < this.Size; i++)
            {
                sb.AppendFormat("{0}{1}", this.names[i], new string(' ', maxLenght - this.names[i].Length));
                for (int j = 0; j < this.Size; j++)
                {
                    sb.AppendFormat("{0} ", this[i, j] ? 1 : 0);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
