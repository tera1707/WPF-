using ClassLibrary1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp10
{
    class Program
    {
        // https://github.com/Fody/Costura
        static void Main(string[] args)
        {
            Console.WriteLine("Start..");
            Console.ReadLine();

            int a = Class1.Add(1, 2);
            Console.WriteLine("answer:" + a);

            Console.ReadLine();
        }
    }
}
