using GraphX.PCL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge() : this(null, null, 1)
        {

        }
        public DataEdge(DataVertex source, DataVertex target, double weight = 1) : base(source, target, weight)
        {

        }
    }
}
