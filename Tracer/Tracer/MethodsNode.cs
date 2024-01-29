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
        public List<MethodNode> internalMethodStructs;

        private (string,string) GetCallingMethodNameAndClassName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            int skipFrames = 2;

            // Получаем вызывающий метод, пропуская методы внутри библиотеки Tracer
            for (int i = skipFrames; i < stackFrames.Length; i++)
            {
                MethodBase method = stackFrames[i].GetMethod();
                if (method.DeclaringType != typeof(Tracer))
                {
                    return (method.Name,method.DeclaringType.Name);
                }
            }

            return (string.Empty,string.Empty); // Если вызывающий метод не найден
        }


        public void StartStopwatch()
        {
            _stopwatch = Stopwatch.StartNew();
        }

        public void StopStopwatch()
        {
            TimeSpan elapsedTime = _stopwatch.Elapsed;
            _methodStruct.Time = elapsedTime.TotalMilliseconds;
        }

        public MethodNode()
        {
            internalMethodStructs = new List<MethodNode>();
            StackTrace stackTrace = new StackTrace();
            (string, string) res = GetCallingMethodNameAndClassName();

            _methodStruct.Name = res.Item1;
            //_methodStruct.Name = methodName;
            //_methodStruct.ClassName = GetCallingClassName();
            _methodStruct.ClassName = res.Item2;
            _methodStruct.MethodDepth = stackTrace.FrameCount - 2;
        }
    }
}
