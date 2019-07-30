#pragma once
#include <Windows.h>
#include <string.h>

// エクスポートとインポートの切り替え
#ifdef VC_DLL_EXPORTS
#undef VC_DLL_EXPORTS
#define VC_DLL_EXPORTS extern "C" __declspec(dllexport)
#else
#define VC_DLL_EXPORTS extern "C" __declspec(dllimport)
#endif

// エクスポート関数のプロトタイプ宣言
// 通常、__stdcallを適用する(__stdcall = WINAPI)。
VC_DLL_EXPORTS int __cdecl UnmanagedAdd(int a, int b);