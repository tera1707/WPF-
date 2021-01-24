using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DllTestCs
{
    public class MyCsDllClass
    {
        /// <summary>
        /// staticなメソッド。ただの足し算を行う。
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Add()
        {
            return NativeMethod.Test_MyApi();
        }

    }



    public static class NativeMethod
    {
        [DllImport("DllTest.dll", EntryPoint = "Test_MyApi", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public extern static int Test_MyApi();
    }
}
