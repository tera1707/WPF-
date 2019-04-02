#include "pch.h"
#include <iostream>
#include <windows.h>
#include <string.h>

#include "DllTest.h" // DllTestのプロジェクトのフォルダ「$(SolutionDir)DllTest\」を見に行っている(追加のインクルードディレクトリに左記を設定)

///////////////////////
// 静的にDLLを呼び出す
///////////////////////

///////////////////////
// 静的にDLLを呼び出す方法
// ①Dllと対になる.libファイルを参照するように設定追加
//   1.pjのプロパティ > リンカー > 入力 の、追加の依存ファイルに、必要な.lib(ここではDllTest.lib)を追加
//   2.pjのプロパティ > リンカー > 全般 の、追加のライブラリディレクトリに、その.libのあるフォルダを追加
// ②extern "C" __declspec(dllimport) でDLLをインポート(ここでは.hで実施
// ③必要なところで関数を呼ぶ
///////////////////////
int main()
{
	Test_MyApi();
	Test_MyApi2(L"あああ", L"いいい");
	Test_MyApi3(2);
}
