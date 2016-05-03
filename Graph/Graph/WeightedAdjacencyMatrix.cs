using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    public class WeightedAdjacencyMatrix<TVertex>
        where TVertex : VertexBase
    {
        private readonly string[] names;
        private readonly double?[,] matrix;

        public double? this[int i, int j] { get { return matrix[i, j]; } }
        public int Size { get { return matrix.GetLength(0); } }

        public WeightedAdjacencyMatrix(Grapher<TVertex> g)
        {
            var V = new List<TVertex>(g.Vertices);
            int n = V.Count;

            this.names = new string[n];
            this.matrix = new double?[n, n];

            for (int i = 0; i < n; i++)
            {
                this.names[i] = V[i].Name;

                for (int j = 0; j < n; j++)
                {
                    var edge = g.GetEdge(V[i], V[j]).FirstOrDefault();

                    if (edge != null)
                    {
                        matrix[i, j] = edge.Weight;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the adjancent vertex which is connected to vertexKey with the minmum edge weight.
        /// May return null if there is no minimum edge.
        /// </summary>
        public string MinimumEdge(string vertexKey)
        {
            int v = Array.IndexOf(names, vertexKey);
            int? m = MinimumEdge(v);
            return m.HasValue ? names[m.Value] : null;
        }

        /// <summary>
        /// Takes an index of an vertex and returns the index of a adjancent vertex which edge has the minimum weight.
        /// If vertex has no edges the method returns null.
        /// </summary>
        public int? MinimumEdge(int v)
        {
            double minWeight = double.PositiveInfinity;
            int? minIndex = null;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                if (matrix[v, i].HasValue && matrix[v, i].Value < minWeight)
                {
                    minWeight = matrix[v, i].Value;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public double? Weight(string vertex0, string vertex1)
        {
            int v0 = Array.IndexOf(names, vertex0);
            int v1 = Array.IndexOf(names, vertex1);

            return this[v0, v1];
        }

        public void SetWeight(string vertex0, string vertex1, double? weight)
        {
            int v0 = Array.IndexOf(names, vertex0);
            int v1 = Array.IndexOf(names, vertex1);

            matrix[v0, v1] = weight;
            matrix[v1, v0] = weight;
        }

        /// <summary>
        /// Removes all edges for a vertex
        /// </summary>
        /// <param name="vertex"></param>
        public void RemoveAllEdges(string vertex)
        {
            int v0 = Array.IndexOf(names, vertex);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[v0, i] = null;
                matrix[i, v0] = null;
            }
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
                    if (this[i, j].HasValue)
                    {
                        sb.Append($"{this[i, j]} ");
                    }
                    else
                    {
                        sb.Append("- ");
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
