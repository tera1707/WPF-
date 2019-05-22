#include "pch.h"
#include <iostream>
#include <Windows.h>

using namespace std;

typedef int(*Add)(int p1, int p2);

int main()
{
	auto dll = ::LoadLibrary(L"CsWrapperCppCLI.dll");
	auto add = reinterpret_cast<Add>(::GetProcAddress(dll, "Add"));
	wcout << add(10, 20) << endl;

	system("pause");
}
