using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Main_class
    {
        static void Main(string[] args)
        {
            Program program = new Program();
           // Program.SwapExample();
            // Console.WriteLine();
            //Program.divideExample();
           // Program.UsageExample();

            Runtime runtime = new Runtime();
            runtime.sum(5, 6);
            Runtime rt = new Test();
            rt.sum(10, 20);

            Test ob1 = new Test();
            ob1.sum(5, 6);
            /*Console.Write("Enter your name : ");
            string name = Console.ReadLine();
            Console.Write("Name is : " + name);*/

            DelegateExample.test();
        }
    }
}
