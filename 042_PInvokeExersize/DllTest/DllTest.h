#pragma once
#include <Windows.h>
#include <string.h>

// �G�N�X�|�[�g�ƃC���|�[�g�̐؂�ւ�
#ifdef VC_DLL_EXPORTS
#undef VC_DLL_EXPORTS
#define VC_DLL_EXPORTS extern "C" __declspec(dllexport)
#else
#define VC_DLL_EXPORTS extern "C" __declspec(dllimport)
#endif

// �G�N�X�|�[�g�֐��̃v���g�^�C�v�錾
VC_DLL_EXPORTS void __cdecl Test_MyApi2(LPWSTR* msg);



