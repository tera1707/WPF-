#include "stdafx.h"
#include "CppUnitTest.h"

#include "DllTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace UnitTestCpp
{		
	TEST_CLASS(UnitTest1)
	{
	public:

		BEGIN_TEST_METHOD_ATTRIBUTE(TestMethod_Normal)
			TEST_PRIORITY(1)
			TEST_DESCRIPTION(L"description1")
			TEST_WORKITEM(11)
		END_TEST_METHOD_ATTRIBUTE()

		TEST_METHOD(TestMethod_Normal)
		{
			wchar_t* buf_in = L"あいうえお";
			wchar_t* buf_out = nullptr;

			int ans1 = Test_MyApiAdd(1, 2);
			int ans2 = Test_MyApiSub(3, 1);
			Test_MyApiPointerCopy(buf_in, &buf_out);

			Assert::AreEqual(ans1, 1 + 2); // 正常に終了
			Assert::AreEqual(ans2, 3 - 1); // 正常に終了
			Assert::IsNotNull(buf_out);    // 正常に終了

			Logger::WriteMessage("Test OK");// デバッグ時のログ(出力欄)に出力
		}

		BEGIN_TEST_METHOD_ATTRIBUTE(TestMethod_Error)
			TEST_PRIORITY(2)
			TEST_DESCRIPTION(L"description2")
			TEST_WORKITEM(12)
		END_TEST_METHOD_ATTRIBUTE()

		TEST_METHOD(TestMethod_Error)
		{
			wchar_t* buf_in = nullptr;
			wchar_t* buf_out = nullptr;

			int ans1 = Test_MyApiAdd(1, 2);
			int ans2 = Test_MyApiSub(3, 1);
			Test_MyApiPointerCopy(buf_in, &buf_out);

			Assert::AreEqual(ans1, 1 + 5);    // ans1と1+5が一致しないので、ここでTestストップ。
			Assert::AreEqual(ans2, 100 - 1);  // 実施されない
			Assert::IsNotNull(buf_out);       // 実施されない

			Logger::WriteMessage("Test OK");
		}
	};
}
