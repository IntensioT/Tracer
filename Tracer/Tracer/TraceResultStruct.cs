using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public struct TraceResultStruct
    {
        public int Id;
        public double Time;
        public List<MethodNode> Methods;
        //public ConcurrentDictionary<int, MethodStruct> MethodsDict;
    }
}
