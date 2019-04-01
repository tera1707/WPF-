// ConsoleApplication1.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include "pch.h"
#include <iostream>
#include <windows.h>
#include <string.h>

#include "DllTest.h"

int main()
{
	Test_MyApi();
	Test_MyApi2(L"あああ", L"いいい");
	Test_MyApi3(2);
}
