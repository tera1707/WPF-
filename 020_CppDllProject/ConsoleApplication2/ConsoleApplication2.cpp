#include "pch.h"
#include <windows.h>

typedef void(*FUNC1)();
typedef void(*FUNC2)(const wchar_t* str1, const wchar_t* str2);
typedef void(*FUNC3)(int count);

///////////////////////
// 動的にDLLを呼び出す
///////////////////////

int main()
{
	// DLL呼び出し
	HMODULE hModule = LoadLibrary(L"DllTest.dll");

	if (NULL == hModule) return 1;

	// 関数読み込み
	FUNC1 func1 = (FUNC1)GetProcAddress(hModule, "Test_MyApi");
	FUNC2 func2 = (FUNC2)GetProcAddress(hModule, "Test_MyApi2");
	FUNC3 func3 = (FUNC3)GetProcAddress(hModule, "Test_MyApi3");

	// 関数コール実行
	func1();
	func2(L"あいうえお", L"かきくけこ");
	func3(4);

	return 0;
}
