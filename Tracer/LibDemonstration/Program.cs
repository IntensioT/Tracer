using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tracer;

namespace LibDemonstration
{
    class MyClass
    {
        private ITracer _tracer;

        internal MyClass(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void MethodA()
        {
            _tracer.StartTrace();

            // Ваш код метода MethodA

            MethodB();
            MethodC();

            _tracer.StopTrace();
        }

        private void MethodB()
        {
            _tracer.StartTrace();

            // Ваш код метода MethodB
            _tracer.StopTrace();
        }

        private void MethodC()
        {
            _tracer.StartTrace();

            // Ваш код метода MethodC

            MethodD();
            _tracer.StopTrace();
        }

        private void MethodD()
        {
            _tracer.StartTrace();

            // Ваш код метода MethodD
            _tracer.StopTrace();
        }
    }

    class Program
    {
        static Tracer.Tracer tracer = new Tracer.Tracer();

        static void Main()
        {
            MyClass myObject = new MyClass(tracer);
            myObject.MethodA();

            tracer.GetTraceResult();
        }
        //MethodA
        //-MethodB
        //-MethodC
        //--MethodD
    }
}
