using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class MethodNode
    {
        private MethodStruct _methodStruct;
        private Stopwatch _stopwatch;

        public MethodStruct GetMethodStruct { get { return _methodStruct; } }

        


        public void StartStopwatch()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void StopStopwatch()
        {
            TimeSpan elapsedTime = _stopwatch.Elapsed;
            _methodStruct.Time = elapsedTime.TotalMilliseconds;
        }

        public MethodNode(string methodName, string className)
        {
            StackTrace stackTrace = new StackTrace();
            //_methodStruct.Name = GetCallingMethodName();
            _methodStruct.Name = methodName;
            //_methodStruct.ClassName = GetCallingClassName();
            _methodStruct.ClassName = className;
            _methodStruct.MethodDepth = stackTrace.FrameCount - 2;
        }
    }
}
