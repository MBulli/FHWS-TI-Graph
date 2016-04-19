using QuickGraph.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    abstract class DepthFirstSearchVisitorBase
    {
        DepthFirstSearchAlgorithm<DataVertex, DataEdge> algo;

        public DepthFirstSearchVisitorBase(DataGraph grapToVisit)
            : this(new DepthFirstSearchAlgorithm<DataVertex, DataEdge>(grapToVisit))
        { }

        public DepthFirstSearchVisitorBase(DepthFirstSearchAlgorithm<DataVertex, DataEdge> algo)
        {
            if (algo == null)
                throw new ArgumentNullException(nameof(algo));

            algo.InitializeVertex += Algo_InitializeVertex;
            algo.DiscoverVertex += Algo_DiscoverVertex;
            algo.FinishVertex += Algo_FinishVertex;

            algo.Started += Algo_Started;
            algo.Finished += Algo_Finished;
            algo.Aborted += Algo_Aborted;
        }

        public void Start()
        {
            algo.Compute();
        }
        
        protected virtual void SearchStarted() { }
        protected virtual void SearchFinished() { }
        protected virtual void SearchAborted() { }

        protected virtual void InitializeVertex(DataVertex vertex) { }
        protected virtual void DiscoverVertex(DataVertex vertex) { }
        protected virtual void FinishVertex(DataVertex vertex) { }



        private void Algo_InitializeVertex(DataVertex vertex) { InitializeVertex(vertex); }
        private void Algo_Started(object sender, EventArgs e) { SearchStarted(); }
        private void Algo_FinishVertex(DataVertex vertex) { FinishVertex(vertex); }
        private void Algo_Finished(object sender, EventArgs e) { SearchFinished(); }
        private void Algo_DiscoverVertex(DataVertex vertex) { DiscoverVertex(vertex); }
        private void Algo_Aborted(object sender, EventArgs e) { SearchAborted(); }
    }
}
