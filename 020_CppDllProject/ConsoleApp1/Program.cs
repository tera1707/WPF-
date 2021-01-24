using System;
using System.Runtime.InteropServices;
using DllTestCs;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // C++のDLLを呼ぶ(下のNativeMethodクラスで呼び方を定義)
            var ret = MyCsDllClass.Add();
            Console.WriteLine("答え：" + ret );
            Console.ReadLine();
        }
    }
}


