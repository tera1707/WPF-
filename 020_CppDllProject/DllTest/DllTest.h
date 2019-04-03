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
VC_DLL_EXPORTS void __cdecl Test_MyApi();
VC_DLL_EXPORTS void __cdecl Test_MyApi2(const wchar_t* lpText, const wchar_t* lpCaption);
VC_DLL_EXPORTS void __cdecl Test_MyApi3(int count);
VC_DLL_EXPORTS int __cdecl Test_MyApiAdd(int p1, int p2);
VC_DLL_EXPORTS int __cdecl Test_MyApiSub(int p1, int p2);
VC_DLL_EXPORTS void __cdecl Test_MyApiPointerCopy(wchar_t* p_in, wchar_t** p_out);

