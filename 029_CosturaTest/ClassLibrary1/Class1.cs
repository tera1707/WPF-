using ClassLibrary2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1
    {
        public static int Add(int a, int b)
        {
            return NativeMethod.UnmanagedAdd(a, b);
        }
    }

    public static class NativeMethod
    {
        [DllImport("Dll1.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int UnmanagedAdd(int a, int b);
    }
}
