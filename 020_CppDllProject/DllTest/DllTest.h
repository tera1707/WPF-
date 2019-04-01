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

