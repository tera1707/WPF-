#include "pch.h"
#include <windows.h>
#include <stdio.h>

#define VC_DLL_EXPORTS
#include "Dll1.h"

// エクスポート関数の実装
int __cdecl UnmanagedAdd(int a, int b)
{
	return a + b;
}