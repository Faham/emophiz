// TTLExplorer.cpp : main source file for TTLExplorer.exe
//

#include "stdafx.h"

#include "resource.h"

#include "MainFrm.h"

CAppModule _Module;

int Run(LPTSTR /*lpstrCmdLine*/ = NULL, int nCmdShow = SW_SHOWDEFAULT)
{
	CMessageLoop theLoop;
	_Module.AddMessageLoop(&theLoop);

	// use static because object is too big for stack!
	static CMainFrame wndMain;

	if(wndMain.CreateEx() == NULL)
	{
		ATLTRACE(_T("Main window creation failed!\n"));
		return 0;
	}

	wndMain.ShowWindow(nCmdShow);

	int nRet = theLoop.Run();

	_Module.RemoveMessageLoop();
	return nRet;
}

int WINAPI _tWinMain(HINSTANCE hInstance, HINSTANCE /*hPrevInstance*/, LPTSTR lpstrCmdLine, int nCmdShow)
{
#ifdef FORCE_SINGLE_INSTANCE
	// Switch to other instance if already running
	//
	// This is the simplest method, using FindWindow
	// For a more robust method, use a named mutex
	// For an even more robust method, use a named semaphor

	HWND hWnd = ::FindWindow( CMainFrame::GetWndClassInfo().m_wc.lpszClassName, NULL );
	if ( NULL != hWnd )
	{
		if ( ::IsIconic( hWnd ) )
			::ShowWindow( hWnd, SW_RESTORE );
		else
			::SetForegroundWindow( ::GetLastActivePopup( hWnd ) );
		return 0;
	}
#endif

	HRESULT hRes = ::CoInitialize(NULL);
	ATLASSERT(SUCCEEDED(hRes));

	// this resolves ATL window thunking problem when Microsoft Layer for Unicode (MSLU) is used
	::DefWindowProc(NULL, 0, 0, 0L);

	AtlInitCommonControls(ICC_COOL_CLASSES | ICC_BAR_CLASSES);	// add flags to support other controls

	hRes = _Module.Init(NULL, hInstance);
	ATLASSERT(SUCCEEDED(hRes));

	int nRet = Run(lpstrCmdLine, nCmdShow);

	_Module.Term();
	::CoUninitialize();

	return nRet;
}
