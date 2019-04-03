#include "stdafx.h"
#include <windows.h>
#include <stdio.h>

#define VC_DLL_EXPORTS
#include "DllTest.h"

// エクスポート関数の実装
// 通常、__stdcallを適用する(__stdcall = WINAPI)。
void __cdecl Test_MyApi()
{
	const wchar_t* lpText = L"Test\0";
	wprintf_s(lpText);
}

void __cdecl Test_MyApi2(const wchar_t* lpText, const wchar_t* lpCaption)
{
	// MessageBoxを呼び出すだけ。
	wprintf_s(L"%s", lpText);
	wprintf_s(L"%s", L" ");
	wprintf_s(L"%s", lpCaption);
}

void __cdecl Test_MyApi3(int count)
{
	for (int i = 0; i < count; i++)
	{
		wprintf_s(L"%d,", i);
	}
}

int __cdecl Test_MyApiAdd(int p1, int p2)
{
	return p1 + p2;
}

int __cdecl Test_MyApiSub(int p1, int p2)
{
	return p1 - p2;
}

void __cdecl Test_MyApiPointerCopy(wchar_t* p_in, wchar_t** p_out)
{
	if (p_in != nullptr)
	{
		*p_out = p_in;
	}
	else
	{
		p_out = nullptr;
	}
}
