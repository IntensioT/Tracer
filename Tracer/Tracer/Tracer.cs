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
        private int _currentListPointer;
        private TraceResultStruct _traceResultStruct;
        private MethodNode _method;

        private static ConcurrentDictionary<int, TraceResultStruct> _tracersDict;

        private void CountTotalTime()
        {
            foreach (var method in _traceResultStruct.Methods)
            {
                _traceResultStruct.Time += method.GetMethodStruct.Time;
            }
        }
        

        public Tracer() 
        {
            _currentListPointer = -1;
            _traceResultStruct.Id = Thread.CurrentThread.ManagedThreadId;
            _traceResultStruct.Methods = new List<MethodNode>();
            if (_tracersDict == null)
            {
                _tracersDict = new ConcurrentDictionary<int, TraceResultStruct>();
            }
        }

        public void StartTrace()
        {
            _method = new MethodNode();
            _method.StartStopwatch();
            _traceResultStruct.Methods.Add(_method);
            _currentListPointer++;
        }

        public void StopTrace()
        {

            if (_currentListPointer > -1)
            {
                _method = _traceResultStruct.Methods[_currentListPointer--];
                _method.StopStopwatch();

                //_traceResultStruct.Methods[_currentListPointer--] = _method.GetMethodStruct.Time;
            }
            else
            { 
                //exception}
            }
            
        }
        public TraceResultStruct GetTraceResult()
        {
            CountTotalTime();
            _tracersDict.AddOrUpdate(_traceResultStruct.Id, _traceResultStruct, (key, existingValue) => _traceResultStruct);
            return _traceResultStruct;
        }
        public void Dispose()
        {
            //Some dispose
        }
    }
}
