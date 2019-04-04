using System.Runtime.InteropServices;
using DllTestCs;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // C++のDLLを呼ぶ(下のNativeMethodクラスで呼び方を定義)
            NativeMethod.Test_MyApi(1);
            NativeMethod.Test_MyApi2("テキスト", "キャプション");
            NativeMethod.Test_MyApi3(3);

            // C#のDLLを呼ぶ(プロジェクトの参照に、DLLを追加し、このファイルの上にusingを追加)
            int a = MyCsDllClass.Add(1, 2);
            var mcdc = new MyCsDllClass();
            mcdc.AddMyData(new MyData() { index = 1, data = 1.1 });
            mcdc.AddMyData(new MyData() { index = 2, data = 2.2 });
            mcdc.AddMyData(new MyData() { index = 3, data = 3.3 });
            var result = mcdc.DataSearch(2);
            var result2 = mcdc.DataSearch(5);
        }
    }

    public static class NativeMethod
    {
        [DllImport("DllTest.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void Test_MyApi(int param);

        [DllImport("DllTest.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void Test_MyApi2(string lpText, string lpCaption);

        // 下記でも、うまく動く。(CharSetの代わりにMarshallAsでLPWSTRを指定する)
        //[DllImport("DllTest.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static void Test_MyApi2([MarshalAs(UnmanagedType.LPWStr)]string lpText, [MarshalAs(UnmanagedType.LPWStr)]string lpCaption);

        [DllImport("DllTest.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static void Test_MyApi3(int count);
    }
}


