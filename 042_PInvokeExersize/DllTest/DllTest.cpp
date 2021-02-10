#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <combaseapi.h>
#include <wchar.h>

#define VC_DLL_EXPORTS
#include "DllTest.h"

void __cdecl Test_MyApi2(LPWSTR* msg)
{
	const wchar_t* tmp = L"あいうえおかきくけこ";
	*msg = (LPWSTR) ::CoTaskMemAlloc((wcslen(tmp) + 1) * sizeof(WCHAR));
	wmemcpy_s(*msg, wcslen(tmp) + 1, tmp, wcslen(tmp) + 1);
}

