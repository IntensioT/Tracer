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
        public Dictionary<string,MethodStruct> Methods;
        //public ConcurrentDictionary<int, MethodStruct> MethodsDict;
    }
}
