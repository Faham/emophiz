/******************************************************************************
:                                                                             :
: Filename       : TTLTimer.h                                                 :
: Version        : 1.1                                                        :
: Date           : August 10th 2001                                           :
: Author         : Nicolas Montmarquette - Thought Technology Ltd.            :
: Target         : Win32                                                      :
: Project        : -                                                          :
: Description    : Declaration of the CTTLTimer class.                        :
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

#if !defined(AFX_TTLTIMER_H__1E5A7B09_3E2E_4D8F_9C13_75D84EFD4B63__INCLUDED_)
#define AFX_TTLTIMER_H__1E5A7B09_3E2E_4D8F_9C13_75D84EFD4B63__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include <windows.h>

class CTTLTimer 
{
	public:
		CTTLTimer(){};
		virtual ~CTTLTimer(){};

    static const CTTLTimer& GetCurrentTime();
		void Set(long liMilliseconds);
		void Reset();
		bool IsDone();

	private:
		SYSTEMTIME m_systemTimeLast;
		long m_liMilliseconds;
};

class CTTLTimer2
{
	public:
		CTTLTimer2(){};
		virtual ~CTTLTimer2(){};
		
		void Set(long liMilliseconds);
		void Reset();
		bool IsDone();

	private:
		__int64 m_i64_TicksRequired;
		__int64 m_i64_LastTimeStamp;
		__int64 m_i64_CurrentTimeStamp;
};


#endif // !defined(AFX_TTLTIMER_H__1E5A7B09_3E2E_4D8F_9C13_75D84EFD4B63__INCLUDED_)
