﻿using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            NativeMethod.Test_MyApi(1);

            NativeMethod.Test_MyApi2("テキスト", "キャプション");

            NativeMethod.Test_MyApi3(3);
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

