using System.Reflection;

namespace ClassLibrary1
{
    public class Class1
    {
        public string AddPublic(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }

        internal string AddInternal(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }

        private string AddPrivate(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }

        public static string AddPublicStatic(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }

        internal static string AddInternalStatic(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }

        private static string AddPrivateStatic(int val1, int val2)
        {
            return MethodBase.GetCurrentMethod().Name + " " + (val1 + val2).ToString();
        }
    }

    public class Class1Sub : Class1
    {

    }
}