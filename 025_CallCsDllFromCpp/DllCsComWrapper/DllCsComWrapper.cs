using DllCs;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace DllCsComWrapper
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("85555B74-E2E0-4493-9869-3CE95F13CB99")]
    public class DllCsComWrapperClass
    {
        public Int32 Add(Int32 param1, Int32 param2)
        {
            int ret = DllCsClass.Add(param1, param2);
            Thread.Sleep(1000);
            return (Int32)ret;
        }
    }
}
