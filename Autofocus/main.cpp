// Copyright (C) Jakub Kuczkowiak 2017

#include <Windows.h>
#include <CommCtrl.h>
#include <vector>
#include "resource.h"

using namespace std;

#pragma comment(lib, "ComCtl32.lib")

//#pragma comment(linker, \
//  "\"/manifestdependency:type='Win32' "\
//  "name='Microsoft.Windows.Common-Controls' "\
//  "version='6.0.0.0' "\
//  "processorArchitecture='*' "\
//  "publicKeyToken='6595b64144ccf1df' "\
//  "language='*'\"")

// TODO: Seems it might need some correction. Messagebox fucks it up?
bool IsIETopWindow()
{
	HWND IEWindow = FindWindow(L"IEFrame", nullptr);
	if (IEWindow == GetForegroundWindow())
		return !IsIconic(IEWindow);

	return false;
}

bool IsChessTabSelected()
{
	return FindWindow(L"IEFrame", L"Szachy - Windows Internet Explorer") != NULL;
}

bool _stdcall EnumProc(HWND handle, LPARAM lParam)
{
	vector<HWND>* result = reinterpret_cast<vector<HWND>*>(lParam);
	(*result).push_back(handle);

	return true;
}

vector<HWND> GetChildWindows(HWND hParent)
{
	vector<HWND> result = vector<HWND>();
	EnumChildWindows(hParent, (WNDENUMPROC)EnumProc, reinterpret_cast<LPARAM>(&result));

	return result;
}

HWND GetChessServerWindow()
{
	HWND hMainIEWindow = FindWindow(L"IEFrame", NULL);
	vector<HWND> l2_ChildWindows = GetChildWindows(hMainIEWindow);

	for (HWND l2_WindowHandle : l2_ChildWindows)
	{
		wchar_t l2_WindowClass[10];
		GetClassName(l2_WindowHandle, l2_WindowClass, 10);

		if (!wcscmp(l2_WindowClass, L"Frame Tab"))
		{
			vector<HWND> l3_ChildWindows = GetChildWindows(l2_WindowHandle);
			for (HWND l3_WindowHandle : l3_ChildWindows)
			{
				wchar_t l3_WindowName[37];
				GetWindowText(l3_WindowHandle, l3_WindowName, 37);

				if (!wcscmp(l3_WindowName, L"Szachy - Internet Explorer") || !wcscmp(l3_WindowName, L"Szachy ▼ - Internet Explorer"))
				{
					vector<HWND> l4_ChildWindows = GetChildWindows(l3_WindowHandle);
					for (HWND l4_WindowHandle : l4_ChildWindows)
					{
						wchar_t l4_WindowClass[21];
						GetClassName(l4_WindowHandle, l4_WindowClass, 21);

						if (!wcscmp(l4_WindowClass, L"Shell DocObject View"))
						{
							vector<HWND> l5_ChildWindows = GetChildWindows(l4_WindowHandle);
							for (HWND l5_WindowHandle : l5_ChildWindows)
							{
								wchar_t l5_WindowClass[26];
								GetClassName(l5_WindowHandle, l5_WindowClass, 26);
								if (!wcscmp(l5_WindowClass, L"Internet Explorer_Server"))
								{
									return l5_WindowHandle;
								}
							}
						}
					}
				}
			}
		}
	}

	return nullptr;
}

HWND GetEDGEChessServerWindow()
{
	HWND hMainIEWindow = FindWindow(L"ApplicationFrameWindow", NULL);
	vector<HWND> l2_ChildWindows = GetChildWindows(hMainIEWindow);

	for (HWND l2_WindowHandle : l2_ChildWindows)
	{
		wchar_t l2_WindowClass[50];
		GetClassName(l2_WindowHandle, l2_WindowClass, 50);

		if (!wcscmp(l2_WindowClass, L"ApplicationFrameInputSinkWindow"))
		{
			//return l2_WindowHandle;

			vector<HWND> l3_ChildWindows = GetChildWindows(l2_WindowHandle);
			for (HWND l3_WindowHandle : l3_ChildWindows)
			{
				wchar_t l3_WindowName[37];
				GetWindowText(l3_WindowHandle, l3_WindowName, 37);

				if (!wcscmp(l3_WindowName, L"Szachy - Microsoft Edge") || !wcscmp(l3_WindowName, L"Szachy ▼ - Microsoft Edge"))
				{
					vector<HWND> l4_ChildWindows = GetChildWindows(l3_WindowHandle);
					for (HWND l4_WindowHandle : l4_ChildWindows)
					{
						wchar_t l4_WindowClass[21];
						GetClassName(l4_WindowHandle, l4_WindowClass, 21);

						if (!wcscmp(l4_WindowClass, L"Shell DocObject View"))
						{
							vector<HWND> l5_ChildWindows = GetChildWindows(l4_WindowHandle);
							for (HWND l5_WindowHandle : l5_ChildWindows)
							{
								wchar_t l5_WindowClass[26];
								GetClassName(l5_WindowHandle, l5_WindowClass, 26);
								if (!wcscmp(l5_WindowClass, L"ApplicationFrameInputSinkWindow"))
								{
									return l5_WindowHandle;
								}
							}
						}
					}
				}
			}
		}
	}

	return nullptr;
}

void IDTMRSETFOCUS_OnEvent(HWND, UINT, UINT, DWORD)
{
	if (!(IsIETopWindow() && IsChessTabSelected()))
	{
		HWND hChessServerWindow = GetChessServerWindow();//GetEDGEChessServerWindow();//GetChessServerWindow();
		if (hChessServerWindow != nullptr)
		{
			SendMessage(hChessServerWindow, WM_SETFOCUS, NULL, NULL);
		}
	}
}

INT_PTR CALLBACK WindowProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam);

int CALLBACK WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	InitCommonControls();
	HWND hWnd = CreateDialogParam(hInstance, MAKEINTRESOURCE(IDD_MAIN), NULL, WindowProc, 0);

	if (hWnd == NULL)
	{
		return 0;
	}

	ShowWindow(hWnd, nCmdShow);

	BOOL bReturn;
	MSG msg = { };
	while ((bReturn = GetMessage(&msg, NULL, 0, 0)) != 0)
	{
		if (bReturn == -1)
		{
			// Handle the error and possibly exit.
		}
		else if (!IsWindow(hWnd) || !IsDialogMessage(hWnd, &msg))
		{
			TranslateMessage(&msg); 
			DispatchMessage(&msg); 
		}
	}

	return 0;
}

INT_PTR CALLBACK WindowProc(HWND hWnd, UINT Msg, WPARAM wParam, LPARAM lParam)
{
	switch (Msg)
	{
	case WM_COMMAND:
		{
			switch (HIWORD(wParam))
			{
			case BN_CLICKED:
				{
					switch (LOWORD(wParam))
					{
					case IDCHKSETFOCUS:
						{
							switch((SendMessage((HWND)lParam, BM_GETCHECK, 0, 0)))
							{
							case BST_CHECKED:
								{
									SetTimer(hWnd, IDTMRSETFOCUS, 100, (TIMERPROC)IDTMRSETFOCUS_OnEvent);
								}
								break;

							case BST_UNCHECKED:
								{
									KillTimer(hWnd, IDTMRSETFOCUS);
								}
								break;
							}
						}
						break;
					}
				}
				break;
			default:
				break;
			}

			
		}
		break;

	case WM_SYSCOMMAND:
		{
		switch( wParam & 0xfff0 )  // (filter out reserved lower 4 bits:  see msdn remarks http://msdn.microsoft.com/en-us/library/ms646360(VS.85).aspx)
		{
			case SC_MINIMIZE:
				{
					//MessageBox(hWnd, L"MINIMIZED", L"MINIMIZED", 0);
				}
				break;
			case SC_RESTORE:
				{
					//ShowWindow(hWnd, SW_SHOW);
					//MessageBox(hWnd, L"RESTORED", L"RESTORED", 0);
				}
				break;
		}
		break;
		}


	case WM_DESTROY:
		{
			PostQuitMessage(wParam);
			return 0;
		}
		break;

	default:
		break;
	}

	return DefWindowProc(hWnd, Msg, wParam, lParam);
}