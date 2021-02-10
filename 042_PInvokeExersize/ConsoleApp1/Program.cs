using System;
using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ////////////////////////////
            // C++のDLLから文字列を受け取る
            ////////////////////////////
            string buf = null;
            NativeMethod.Test_MyApi2(out buf);
            Console.WriteLine(buf);

            Console.ReadLine();
        }
    }

    public static class NativeMethod
    {
        [DllImport("DllTest.dll")]
        public extern static void Test_MyApi2([MarshalAs(UnmanagedType.LPWStr)] out string lpText);
    }
}


