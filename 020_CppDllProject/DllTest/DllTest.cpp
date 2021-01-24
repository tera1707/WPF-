#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include "DllTest2.h"

#define VC_DLL_EXPORTS
#include "DllTest.h"

// エクスポート関数の実装
// 通常、__stdcallを適用する(__stdcall = WINAPI)。
int __cdecl Test_MyApi()
{
	return Test_MyApi2(11,22);
}
