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
        private MethodNode _prevMethod;
        private int _currentMethodDepth;

        private static ConcurrentDictionary<int, TraceResultStruct> _tracersDict;

        private void CountTotalTime()
        {
            foreach (var method in _traceResultStruct.Methods)
            {
                _traceResultStruct.Time += method.GetMethodStruct.Time;
            }
        }

        public void StartTrace()
        {
            _prevMethod = _method;
            _method = new MethodNode();

            int prevMethodDepth = _prevMethod?.GetMethodStruct.MethodDepth ?? -1;
            _currentMethodDepth = _method.GetMethodStruct.MethodDepth;

            switch (prevMethodDepth)
            {
                case var depth when depth == -1:
                    _traceResultStruct.Methods.Add(_method);
                    break;
                case var depth when depth == (_currentMethodDepth - 1):
                    _prevMethod.internalMethodStructs.Add(_method);
                    break;
                default:
                    _traceResultStruct.Methods.Add(_method);
                    break;
            }

            _method.StartStopwatch();
        }

        public void StopTrace()
        {
            _method.StopStopwatch();
            _currentMethodDepth--;
        }
        public TraceResultStruct GetTraceResult()
        {
            CountTotalTime();
            _tracersDict.AddOrUpdate(_traceResultStruct.Id, _traceResultStruct, (key, existingValue) => _traceResultStruct);
            return _traceResultStruct;
        }

        public ConcurrentDictionary<int, TraceResultStruct> GetThreadDictionary()
        {
            GetTraceResult();
            return _tracersDict;
        }

        public Tracer()
        {
            _traceResultStruct.Id = Thread.CurrentThread.ManagedThreadId;
            _traceResultStruct.Methods = new List<MethodNode>();
            if (_tracersDict == null)
            {
                _tracersDict = new ConcurrentDictionary<int, TraceResultStruct>();
            }
        }

        public void Dispose()
        {
            //Some dispose
        }
    }
}
