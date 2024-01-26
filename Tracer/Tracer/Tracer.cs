using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    /*Tracer должен собирать следующую информацию об измеряемом методе:
    имя метода;
    имя класса с измеряемым методом;
    время выполнения метода.
    */
    public class Tracer : ITracer
    {
        private int _id;
        private int _totalTime;
        private Stopwatch _stopwatch;

        private static ConcurrentDictionary<int, MethodNode> _methodsDict;

        private string GetCallingMethodName()
        {
            MethodBase method = new StackFrame(1).GetMethod();
            return method.Name;
        }

        private static string GetCallingClassName()
        {
            MethodBase method = new StackFrame(1).GetMethod();
            Type declaringType = method.DeclaringType;
            return declaringType.Name;
        }

        public Tracer() 
        { 
            if (_methodsDict == null)
            {
                _methodsDict = new ConcurrentDictionary<int, MethodNode>();
            }
        }

        public void StartTrace()
        {
            // Call stopwatch in node
        }

        public void StopTrace()
        {
            // Call stopwatch in node
        }
        public TraceResultStruct GetTraceResult()
        {

            return new TraceResultStruct();
        }
        public void Dispose()
        {
            //Some dispose
        }
    }
}
