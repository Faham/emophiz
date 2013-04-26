// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__576B6B71_65ED_4E3A_A5D2_F243250B7CDA__INCLUDED_)
#define AFX_STDAFX_H__576B6B71_65ED_4E3A_A5D2_F243250B7CDA__INCLUDED_

// Change these values to use different versions
//#define WINVER		0x0400
//#define WINVER		0x0500 // for HDEVNOTIFY, HPDEV_BROADCAST_DEVICEINTERFACE
	// Note: with VC6, need to install a more recent PDK
	// Also, must include the InetSDK in the PDK, because of changes in MSHTML.H
//#define _WIN32_WINNT	0x0400
//#define _WIN32_WINNT	0x0500
//#define _WIN32_WINNT	0x0501 // for DEVICE_NOTIFY_ALL_INTERFACE_CLASSES
#define _WIN32_IE	0x0400
#define _RICHEDIT_VER	0x0100

#define ATL_TRACE_CATEGORY (0xFFFFFFFF & ~(0x10000000)) //~(atlTraceUI)
#define ATL_TRACE_LEVEL 0

#define _ATL_USE_CSTRING_FLOAT

#include <atlbase.h>
#if (_ATL_VER < 0x0800) && ! defined( TTL_ATL_VER )
	#pragma message( "Warning: ATL is not DEP compatible, out of the box, before VS2005." )
	#pragma message( "For details, refer to http://support.microsoft.com/kb/948468" )
#endif
#include <atlapp.h>

extern CAppModule _Module;

#include <atlcom.h>
#include <atlhost.h>
#include <atlwin.h>
#include <atlctl.h>

#include <atlframe.h>
#include <atlctrls.h>
#include <atldlgs.h>
#include <atlctrlw.h>

#include <atlcrack.h>
#include <atlmisc.h>
#include <atlsplit.h>
#include <atlctrlx.h>

// Compilation switches:
//

#define USE_TTLLIVE_SIDE_BY_SIDE

#define FORCE_SINGLE_INSTANCE

#undef USE_WM_TIMER

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__576B6B71_65ED_4E3A_A5D2_F243250B7CDA__INCLUDED_)
