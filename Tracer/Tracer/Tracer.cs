using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private TraceResultStruct _traceResultStruct;
        private MethodNode _method;

        private static ConcurrentDictionary<int, TraceResultStruct> _tracersDict;

        private void CountTotalTime()
        {
            foreach (var method in _traceResultStruct.Methods)
            {
                _traceResultStruct.Time += method.Value.Time;
            }
        }

        private string GetCallingMethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            int skipFrames = 1;

            // Получаем вызывающий метод, пропуская методы внутри библиотеки Tracer
            for (int i = skipFrames; i < stackFrames.Length; i++)
            {
                MethodBase method = stackFrames[i].GetMethod();
                if (method.DeclaringType != typeof(Tracer))
                {
                    return method.Name;
                }
            }

            return string.Empty; // Если вызывающий метод не найден
        }

        private static string GetCallingClassName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            // Пропускаем методы внутри библиотеки Tracer и метод GetCallingClassName()
            int skipFrames = 1;

            // Получаем вызывающий класс, пропуская методы внутри библиотеки Tracer и метод GetCallingClassName()
            for (int i = skipFrames; i < stackFrames.Length; i++)
            {
                MethodBase method = stackFrames[i].GetMethod();
                Type declaringType = method.DeclaringType;
                if (declaringType != typeof(Tracer))
                {
                    return declaringType.Name;
                }
            }

            return string.Empty; // Если вызывающий класс не найден
        }

        public Tracer() 
        {
            _traceResultStruct.Id = Thread.CurrentThread.ManagedThreadId;
            _traceResultStruct.Methods = new Dictionary<string,MethodStruct>();
            if (_tracersDict == null)
            {
                _tracersDict = new ConcurrentDictionary<int, TraceResultStruct>();
            }
        }

        public void StartTrace()
        {
            string methodName = GetCallingMethodName();
            _method = new MethodNode(methodName, GetCallingClassName());
            _method.StartStopwatch();
            _traceResultStruct.Methods.Add(methodName, _method.GetMethodStruct);
        }

        public void StopTrace()
        {
            _method.StopStopwatch();

            string methodName = GetCallingMethodName();
            //MethodStruct method = _traceResultStruct.Methods.FirstOrDefault(MethodStruct =>  MethodStruct.Name == methodName);
            //method = _method.GetMethodStruct;

            _traceResultStruct.Methods[methodName] = _method.GetMethodStruct;


            //_traceResultStruct.Methods.Add(_method.GetMethodStruct);
        }
        public TraceResultStruct GetTraceResult()
        {
            _tracersDict.AddOrUpdate(_traceResultStruct.Id, _traceResultStruct, (key, existingValue) => _traceResultStruct);
            return _traceResultStruct;
        }
        public void Dispose()
        {
            //Some dispose
        }
    }
}
