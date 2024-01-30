using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LibDemonstration.SomeClasses;
using Tracer;


namespace LibDemonstration
{
    class Program
    {
        static Tracer.Tracer tracer = new Tracer.Tracer();

        static void Main()
        {
            //MethodA
            //-MethodB
            //-MethodC
            //--MethodD
            MyClass myObject = new MyClass(tracer);
            myObject.MethodA();

            tracer.ConsoleResult();

        }
        
    }
}
