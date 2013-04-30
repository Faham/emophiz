/******************************************************************************
:                                                                             :
: Filename       : TTLTimer.cpp                                               :
: Version        : 1.1                                                        :
: Date           : February 12th 2002                                         :
: Author         : Nicolas Montmarquette - Thought Technology Ltd.            :
: Target         : Win32                                                      :
: Project        : -                                                          :
: Description    : Implementation of the CTTLTimer class.                     :
:                  Simple dum timer class giving a level of abstraction in    :
:                  CTTLEncoder class and prevent from using any windows       :
:                  specific functions.                                        :
:                                                                             :
:----------------  Updates  --------------------------------------------------:
:                                                                             :
: Ver,   Date,          Name,            Comments.                            :
:                                                                             :
: 1.0,   aug-10-2001,   Nicolas M.,      Initial Writing.                     :
: 1.1,   feb-12-2002,   Nicolas M.,      Created CTTLTimer2 class that				:
:                                        alternatively use high precision     :
:                                        windows instead of time of day       :
:                                                                             :
******************************************************************************/
#include "TTLTimer.h"
#include <TCHAR.H>

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

bool CTTLTimer::IsDone(void)
{
	long liDelta,liCurrent, liLast;

	SYSTEMTIME systemTimeCurrent;

	GetLocalTime(&systemTimeCurrent);

	liLast = (m_systemTimeLast.wSecond*1000)+m_systemTimeLast.wMilliseconds;
	liCurrent = (systemTimeCurrent.wSecond*1000)+systemTimeCurrent.wMilliseconds;

	liDelta = abs(liCurrent - liLast);

	if( liDelta  >= m_liMilliseconds )return TRUE;
	else return FALSE;
}

void CTTLTimer::Set(long liMilliseconds)
{
	GetLocalTime(&m_systemTimeLast);			
	m_liMilliseconds = liMilliseconds;
}

void CTTLTimer::Reset()
{
	GetLocalTime(&m_systemTimeLast);
}

void CTTLTimer2::Reset(void)
{
	QueryPerformanceCounter((LARGE_INTEGER*)&m_i64_LastTimeStamp);
}

bool CTTLTimer2::IsDone(void)
{
	QueryPerformanceCounter((LARGE_INTEGER*)&m_i64_CurrentTimeStamp);
	m_i64_CurrentTimeStamp-=m_i64_LastTimeStamp;

	return ( m_i64_CurrentTimeStamp >= m_i64_TicksRequired );
}

void CTTLTimer2::Set(long liMilliseconds)
{
	__int64 i64_PerformanceFrequency;
	double lfFrequency;

	QueryPerformanceFrequency((LARGE_INTEGER*)&i64_PerformanceFrequency);
	lfFrequency = (DOUBLE)i64_PerformanceFrequency;
	lfFrequency /= 1000.0;
	lfFrequency *= liMilliseconds;

	m_i64_TicksRequired = (__int64)lfFrequency;
		
	QueryPerformanceCounter((LARGE_INTEGER*)&m_i64_LastTimeStamp);
}
