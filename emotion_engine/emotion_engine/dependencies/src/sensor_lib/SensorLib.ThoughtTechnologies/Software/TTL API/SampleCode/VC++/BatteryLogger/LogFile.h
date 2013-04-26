// LogFile.h: interface for the CLogFile class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_LOGFILE_H__FBC13A99_FE94_4F39_ACEB_BD50E2E6F2BB__INCLUDED_)
#define AFX_LOGFILE_H__FBC13A99_FE94_4F39_ACEB_BD50E2E6F2BB__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <windows.h>
#include <stdio.h>
#include <TCHAR.H>

class CLogFile 
{
	public:
		CLogFile();
		CLogFile( TCHAR* lpszFilename );
		virtual ~CLogFile();

		virtual void Add( LPCTSTR lpStr );
		virtual void Add( LPCTSTR lpStr, bool bTimeStamp );
    virtual void Add( bool bTimeStamp, const TCHAR* format, ... ); 
		virtual void Close(void);
		virtual void Stop(void);
		virtual void Start(void);

	protected:
		HANDLE	m_hFile;
};

class CLogFile2 : public CLogFile
{
	public:
		CLogFile2();
		CLogFile2( TCHAR* lpszFilename );

		virtual void Add( unsigned char* lpBuffer, unsigned long dwByteCount );
	protected:
};


#endif // !defined(AFX_LOGFILE_H__FBC13A99_FE94_4F39_ACEB_BD50E2E6F2BB__INCLUDED_)
