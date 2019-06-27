using ClassLibrary1;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    // 参考：https://www.gesource.jp/weblog/?p=7742

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ret = "";
            var cls1 = new Class1();            // テスト対象のクラス

            // publicなメソッドのテスト
            ret = cls1.AddPublic(1, 2);
            ret = Class1.AddPublicStatic(1, 2);

            // private、internalなメソッドのテスト
            var pbObj = new PrivateObject(cls1);
            ret = (string)pbObj.Invoke("AddPrivate", 1, 2);
            ret = (string)pbObj.Invoke("AddInternal", 1, 2);

            // private static、internal staticなメソッドのテスト
            PrivateType privateType = new PrivateType(typeof(Class1));
            ret = (string)privateType.InvokeStatic("AddPrivateStatic", 1, 2);
            ret = (string)privateType.InvokeStatic("AddInternalStatic", 1, 2);

            // 子クラスから見た親クラスのprivateメソッドをテスト
            var clssub = new Class1Sub();
            var pbObjSub = new PrivateObject(clssub, new PrivateType(typeof(Class1)));
            ret = (string)pbObjSub.Invoke("AddPrivate", 1, 2);
            ret = (string)pbObjSub.Invoke("AddInternal", 1, 2);

            // 子クラスから見た親クラスのprivate staticメソッドは、
            // 親クラスのprivate staticテストと同じ。
        }
    }
}
