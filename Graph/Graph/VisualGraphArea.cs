using GraphX.Controls;
using GraphX.Controls.Models;
using GraphX.PCL.Logic.Models;
using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class VisualGraphArea : GraphArea<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {
        public VisualGraphArea()
        {
            this.LogicCore = new VisualGraphLogicCore();
            this.EdgeLabelFactory = new DefaultEdgelabelFactory();
        }
    }

    class VisualGraphLogicCore : GXLogicCore<DataVertex, DataEdge, BidirectionalGraph<DataVertex, DataEdge>>
    {

    }
}
