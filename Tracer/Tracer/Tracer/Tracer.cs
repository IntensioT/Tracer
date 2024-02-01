using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Tracer.ResultOutput;
using Tracer.Serialization;

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

        private string GetJSON()
        {
            JsonTraceResultSerializer serializer = new JsonTraceResultSerializer();
            return serializer.Serialize(GetTraceResult());
        }

        private string GetXML()
        {
            XMLTraceResultSerializer serializer = new XMLTraceResultSerializer();
            return serializer.Serialize(GetTraceResult());
        }

        public void StartTrace()
        {
            _prevMethod = _method;
            _method = new MethodNode();
            _method.parentMethod = _prevMethod;

            int prevMethodDepth = _prevMethod?.GetMethodStruct.MethodDepth ?? -1; //conditional access and null ??
            _currentMethodDepth = _method.GetMethodStruct.MethodDepth;

            switch (prevMethodDepth)
            {
                case var depth when depth == -1:
                    _traceResultStruct.Methods.Add(_method);
                    break;
                case var depth when depth == (_currentMethodDepth - 1):
                    _prevMethod.GetMethodStruct.internalMethodStructs.Add(_method);
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

            

            if (_prevMethod == _method)
            {
                if(_prevMethod.parentMethod != null)
                {
                    _method = _prevMethod.parentMethod;
                }
            }
            else
            {
                if (_method.GetMethodStruct.MethodDepth == 0)
                {
                    _prevMethod = _method;
                }
                else
                {
                    _method = _prevMethod;
                }
            }

        }
        public TraceResultStruct GetTraceResult()
        {
            if(_traceResultStruct.Time == 0)
            {
                CountTotalTime();
            }
            _tracersDict.AddOrUpdate(_traceResultStruct.Id, _traceResultStruct, (key, existingValue) => _traceResultStruct);
            return _traceResultStruct;
        }

        public ConcurrentDictionary<int, TraceResultStruct> GetThreadDictionary()
        {
            GetTraceResult();
            return _tracersDict;
        }
        

        public void ConsoleResult()
        {
            IResultWritable writer = new ConsoleResultWriter();
            writer.WriteResult(GetJSON());
            writer.WriteResult(GetXML());
        }

        public void FileOutputResult()
        {
            IResultWritable writer = new FileResultWriter("..//..//..//outputJSON.txt");
            writer.WriteResult(GetJSON());
            writer = new FileResultWriter("..//..//..//outputXML.txt");
            writer.WriteResult(GetXML());
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
