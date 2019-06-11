#include "pch.h"
#include <iostream>
#include <Windows.h>//追加

//グローバル変数
IDispatch *pIDisp = NULL;
IUnknown *pIUnk = NULL;

//プロトタイプ宣言
long _Init(void);
long _Finalize(void);
long _Add(long p_Number1, long p_Number2);

int main()
{
	//変数宣言
	int l_Result = 0;

	//初期処理
	_Init();

	//合計処理
	l_Result = _Add(400, 500);

	//後処理
	_Finalize();

	printf("Calc Result : %d", l_Result);

	return l_Result;
}

//***************************************************************************//
//初期化関数
//***************************************************************************//
long _Init(void)
{
	CLSID clsid;

	//COM初期化
	::CoInitialize(NULL);

	//ProcIDからCLSIDを取得（ネームスペース名．クラス名）
	HRESULT h_result = CLSIDFromProgID(L"DllCsComWrapper.DllCsComWrapperClass", &clsid);
	if (FAILED(h_result))
	{
		return -1;
	}

	//Instanceの生成
	//h_result = CoCreateInstance(clsid, NULL, CLSCTX_INPROC_SERVER, IID_IUnknown, (void**)&pIUnk);
	h_result = CoCreateInstance(clsid, NULL, CLSCTX_LOCAL_SERVER, IID_IUnknown, (void**)&pIUnk);
	if (FAILED(h_result))
	{
		return -2;
	}

	//インターフェースの取得（pIDispは共通変数）
	h_result = pIUnk->QueryInterface(IID_IDispatch, (void**)&pIDisp);
	if (FAILED(h_result))
	{
		return -3;
	}

	//正常終了
	return 0;
}

//***************************************************************************//
//修了処理
//***************************************************************************//
long _Finalize(void)
{
	pIDisp->Release();
	pIUnk->Release();
	::CoUninitialize();
	return 0;
}

//***************************************************************************//
//合計処理呼出処理
//***************************************************************************//
long _Add(long p_Number1, long p_Number2)
{
	//DISPIDの取得（関数名の設定）
	DISPID dispid = 0;
	OLECHAR *Func_Name[] = { SysAllocString (L"Add") };
	HRESULT h_result = pIDisp->GetIDsOfNames(IID_NULL, Func_Name, 1, LOCALE_SYSTEM_DEFAULT, &dispid);
	if (FAILED(h_result))
	{
		return -1;
	}

	//パラメータ作成
	DISPPARAMS params;
	::memset(&params, 0, sizeof(DISPPARAMS));

	params.cNamedArgs = 0;
	params.rgdispidNamedArgs = NULL;
	params.cArgs = 2; //呼び出す関数の引数の数

	//引数設定（順番に注意…逆になる）
	VARIANTARG* pVarg = new VARIANTARG[params.cArgs];
	pVarg[0].vt = VT_I4;
	pVarg[0].lVal = p_Number2;
	pVarg[1].vt = VT_I4;
	pVarg[1].lVal = p_Number1;
	params.rgvarg = pVarg;

	VARIANT vRet;
	VariantInit(&vRet);

	//呼び出し
	pIDisp->Invoke(dispid, IID_NULL, LOCALE_SYSTEM_DEFAULT, DISPATCH_METHOD, &params, &vRet, NULL, NULL);

	delete[] pVarg;
	return vRet.lVal;
}
