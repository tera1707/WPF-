#include "framework.h"
#include "WindowsProject1.h"
#include "resource.h"

HINSTANCE hInst;
BOOL CALLBACK MyDlgProc(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPWSTR lpCmdLine, _In_ int nCmdShow)
{
    hInst = hInstance;
    DialogBox(hInst, L"MyTestDlgBase_Main", NULL, (DLGPROC)MyDlgProc);
    return (int)0;
}

BOOL CALLBACK MyDlgProc(HWND hDlg, UINT msg, WPARAM wp, LPARAM lp)
{
    GUID a = GUID_CONSOLE_DISPLAY_STATE;
    switch (msg) {
        case WM_INITDIALOG:
            RegisterPowerSettingNotification(hDlg, &a, DEVICE_NOTIFY_WINDOW_HANDLE);
            break;

        case WM_POWERBROADCAST:
            if (wp == PBT_POWERSETTINGCHANGE)
            {
                auto lppbc = (POWERBROADCAST_SETTING*)lp;
                if (lppbc->PowerSetting == GUID_CONSOLE_DISPLAY_STATE) {
                    if ((lppbc->Data[0] & 0x01) == 0)   OutputDebugString(L"OFF"); // ディスプレイがOFF
                    else                                OutputDebugString(L"ON");  // ディスプレイON
                }
            }
            break;
    }
    return FALSE;
}
