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
            NativeMethod.Test_MyApi();

            // C#のDLLを呼ぶ(プロジェクトの参照に、DLLを追加し、このファイルの上にusingを追加)
            //int a = MyCsDllClass.Add(1, 2);
            //var mcdc = new MyCsDllClass();
            //mcdc.AddMyData(new MyData() { index = 1, data = 1.1 });
            //mcdc.AddMyData(new MyData() { index = 2, data = 2.2 });
            //mcdc.AddMyData(new MyData() { index = 3, data = 3.3 });
            //var result = mcdc.DataSearch(2);
            //var result2 = mcdc.DataSearch(5);

            Console.ReadLine();
        }
    }

    public static class NativeMethod
    {
        [DllImport("DllTest.dll", EntryPoint = "Test_MyApi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void Test_MyApi();
    }
}


