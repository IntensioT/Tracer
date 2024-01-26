using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class MethodNode
    {
        public string name;
        public string classStr;
        public double time;

        private Stopwatch _stopwatch;
        private MethodNode _headMethod;
        private List<MethodNode> _internalMethods;

        public MethodNode(string name, string classStr, MethodNode headMethod)
        {
            this.name = name;
            this.classStr = classStr;
            this._headMethod = headMethod;
        }

        public void StartStopwatch()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void StopWatch()
        {
            TimeSpan elapsedTime = _stopwatch.Elapsed;
            time = elapsedTime.TotalMilliseconds;
        }
    }
}
