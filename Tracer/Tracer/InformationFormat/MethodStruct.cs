using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public struct MethodStruct
    {
        public string Name;
        public string ClassName;
        public double Time;

        [JsonProperty(PropertyName = "methods")]
        public List<MethodNode> internalMethodStructs;
        [JsonIgnore]
        public int MethodDepth;
    }
}
