// LogFile.cpp: implementation of the CLogFile class.
//
//////////////////////////////////////////////////////////////////////
#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
//#define new DEBUG_NEW
#endif

#include "LogFile.h"

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

CLogFile::CLogFile()
{
	m_hFile = CreateFile(
		_T("LogFile.txt"), GENERIC_WRITE,			0, // not shared
		NULL, //security attributes
		OPEN_ALWAYS, // how to create		
		FILE_ATTRIBUTE_NORMAL,
		NULL // handle to template file  
	); 
}

CLogFile::CLogFile( _TCHAR* lpszFilename )
{
	m_hFile = CreateFile(
		lpszFilename, GENERIC_WRITE,			0, // not shared
		NULL, //security attributes
		OPEN_ALWAYS, // how to create
		//OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL,
		NULL // handle to template file  
	); 

}

CLogFile::~CLogFile()
{
	CloseHandle(m_hFile);
}

void CLogFile::Start()
{

}

void CLogFile::Stop()
{

}

void CLogFile::Add( bool bTimeStamp, const TCHAR* pFormat, ...)
{
  TCHAR szStr[256];

  va_list arglist;

  va_start(arglist, pFormat);
  vsprintf( szStr, pFormat, arglist );

  Add(szStr, bTimeStamp);
}

void CLogFile::Add(LPCTSTR lpStr, bool bTimeStamp)
{
	unsigned long dwWritten;

	TCHAR* months[] = {
		_T("nul"),
		_T("Jan"), _T("Feb"), _T("Mar"), _T("Apr"), _T("Mai"), _T("Jun"), 
		_T("Jul"), _T("Aug"), _T("Sep"), _T("Oct"), _T("Nov"), _T("Dec")
	};

	_TCHAR strTimeStr[32];
	
	SYSTEMTIME systemTime;

	if( bTimeStamp ){
		GetLocalTime(&systemTime);
		_stprintf(strTimeStr,
            "\r\n%02d-%s-%04d, %02d:%02d:%02d.%-3d : ",
            systemTime.wDay,
						months[systemTime.wMonth],
						systemTime.wYear,
						systemTime.wHour,
						systemTime.wMinute,
						systemTime.wSecond,
						systemTime.wMilliseconds);
    WriteFile( m_hFile , strTimeStr, _tcsclen(strTimeStr)*sizeof(strTimeStr[0]), &dwWritten,0);
	}
	
	WriteFile( m_hFile , lpStr, _tcsclen(lpStr)*sizeof(lpStr[0]), &dwWritten,0);
}

void CLogFile::Add(LPCTSTR lpStr)
{
	unsigned long dwWritten;

	WriteFile( m_hFile , lpStr, _tcsclen(lpStr)*sizeof(lpStr[0]), &dwWritten,0);
}

void CLogFile::Close()
{
	CloseHandle(m_hFile);
}

void CLogFile2::Add(unsigned char *lpBuffer, unsigned long dwByteCount)
{
	unsigned long dwWritten;

	WriteFile( m_hFile , lpBuffer, dwByteCount, &dwWritten,0);
}

CLogFile2::CLogFile2():CLogFile()
{
}

CLogFile2::CLogFile2( _TCHAR* lpszFilename ): CLogFile( lpszFilename )
{
}


