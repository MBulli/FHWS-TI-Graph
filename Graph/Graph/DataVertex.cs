using GraphX.PCL.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    class DataVertex : VertexBase
    {
        public string Name { get; private set; }
        public string Data { get; set; }
        public DataVertex(string name)
        {
            Name = name;
        }
        public DataVertex(string name, string data) : this (name)
        {
            Data = data;
        }
    }
}
