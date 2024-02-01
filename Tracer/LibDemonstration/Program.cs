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
        static private Tracer.Tracer _tracer;
        static private Tracer.Tracer _tracer1;
        static private Tracer.Tracer _tracer2;

        static void Main()
        {
            _tracer1 = new Tracer.Tracer(Thread.CurrentThread.ManagedThreadId);

            Thread thread1 = new Thread(Thread1);
            Thread thread2 = new Thread(Thread2);

            thread1.Start();
            thread2.Start();

            Foo foo = new Foo(_tracer1);
            foo.MyMethod();

            thread1.Join();
            thread2.Join();

            _tracer2.GetTraceResult();
            _tracer.GetTraceResult();
            _tracer1.GetTraceResult();

            _tracer.GetMultiThreadResult("..//..//..//outputJSON.txt", "..//..//..//outputXML.txt");
        }

        private void SimpleTest()
        {
            _tracer = new Tracer.Tracer(Thread.CurrentThread.ManagedThreadId);

            //MethodA
            //-MethodB
            //-MethodC
            //--MethodD
            MyClass myObject = new MyClass(_tracer);
            myObject.MethodA();

            _tracer.GetTraceResult();

            _tracer.GetMultiThreadResult("..//..//..//outputJSON.txt", "..//..//..//outputXML.txt");
            //_tracer.ConsoleResult();
            //_tracer.FileOutputResult("..//..//..//outputJSON.txt", "..//..//..//outputXML.txt");
        }

        static public void Thread1()
        {
            _tracer = new Tracer.Tracer(Thread.CurrentThread.ManagedThreadId);
            Foo foo = new Foo(_tracer);

            foo.MyMethod();
            foo.MySecondMethod();
        }
        static public void Thread2()
        {
            _tracer2 = new Tracer.Tracer(Thread.CurrentThread.ManagedThreadId);
            Foo foo = new Foo(_tracer2);
            Bar bar = new Bar(_tracer2);

            foo.MyMethod();
            bar.InnerMethod();
        }

    }
}
