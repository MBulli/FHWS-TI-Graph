using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    static class Blatt2Aufgabe2
    {
        public static Grapher<VertexBase> B()
        {
            var graph = FileParser.Parse(@"TestFiles\Sudoku.txt"); // Knoten sind kästchenweise von links nach rechts und oben nach unten alphabetisch benannt
            var prefilledSudokuFields = new Dictionary<String, int>();
            prefilledSudokuFields.Add("E", 3);
            prefilledSudokuFields.Add("D", 1);
            prefilledSudokuFields.Add("I", 1);
            prefilledSudokuFields.Add("L", 3);
            prefilledSudokuFields.Add("P", 0);

            graph.Vertices.ForEach(v => v.Color = -1);

            SetPrefilledSudokuFields(graph, prefilledSudokuFields);

            var newgraph = graph.Clone();

            for (int i = 0; i < 1000; i++)
            {
                newgraph = GreedyColAlgorithm.ColorVerticesVarPrefilledRandom(graph);
            }
           
            return newgraph;
        }

        private static void SetPrefilledSudokuFields(Grapher<VertexBase> inputGraph, Dictionary<String, int> prefilledSudokuFields)
        {
            foreach (var n in prefilledSudokuFields.Keys)
            {
                foreach (var v in inputGraph.Vertices)
                {
                    if (v.Name == n)
                    {
                        v.Color = prefilledSudokuFields[n];
                    }
                }
            }
        }
    }
}
