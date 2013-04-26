// SimpleTimer.h : interface of the CSimpleTimer class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_SIMPLETIMER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
#define AFX_SIMPLETIMER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include "mmsystem.h"
#pragma comment(lib, "winmm.lib")

#ifdef USE_WM_TIMER
#else
	// define to switch back to creating thread OR to not block other mm timers
	#define USE_POST_SIMPLE_TIMER_MESSAGE
#endif

template <class T>
class ATL_NO_VTABLE CSimpleTimer
{
public:

	typedef CSimpleTimer<T> CSimpleTimer_type;

	CSimpleTimer()
		: m_nTimerId( 0 )
	#ifdef USE_POST_SIMPLE_TIMER_MESSAGE
		, m_bPending( FALSE )
	#endif
	{
	}

	~CSimpleTimer()
	{
		ATLASSERT( 0 == m_nTimerId );
	}

	bool SimpleTimerStart( UINT nPeriod /* [ms] */ )
	{
		ATLASSERT( 0 == m_nTimerId && 0 < nPeriod );
		T* pT = static_cast<T*>( this );
	#ifdef USE_WM_TIMER
		m_nTimerId = pT->SetTimer( 42, nPeriod );
	#else
		m_nTimerId = ::timeSetEvent( nPeriod, 0
			, OnTimerThunk
			, reinterpret_cast<DWORD>( pT )
			, TIME_PERIODIC /*| TIME_KILL_SYNCHRONOUS : causes error on W2K, so don't use*/
			);
	#endif
		ATLASSERT( 0 != m_nTimerId );
		return 0 != m_nTimerId;
	}

	bool SimpleTimerStop( void )
	{
		if ( 0 == m_nTimerId ) return true;
	#ifdef USE_WM_TIMER
		T* pT = static_cast<T*>( this );
		pT->KillTimer( m_nTimerId );
	#else
		::timeKillEvent( m_nTimerId );
	#endif
		m_nTimerId = 0;
		return true;
	}

	BEGIN_MSG_MAP( CSimpleTimer_type )
	#ifdef USE_WM_TIMER
		MESSAGE_HANDLER( WM_TIMER, OnTimerThunk )
	#endif
	#ifdef USE_POST_SIMPLE_TIMER_MESSAGE
		MESSAGE_HANDLER( WM_MM_TIMER(), OnMmTimerThunk )
	#endif
	END_MSG_MAP()

protected:

#ifdef USE_WM_TIMER

	UINT m_nTimerId;

	LRESULT OnTimerThunk( UINT, WPARAM wParam, LPARAM, BOOL& bHandled )
	{
		if ( m_nTimerId != (UINT)( wParam ) )
		{
			bHandled = FALSE;
			return 0;
		}
		T* pT = static_cast<T*>( this );
		pT->OnSimpleTimer();
		return 0;
	}

#else

	MMRESULT m_nTimerId;

	#ifndef USE_POST_SIMPLE_TIMER_MESSAGE

		static void CALLBACK OnTimerThunk( UINT, UINT, DWORD dwUser, DWORD, DWORD )
		{
			T* pT = reinterpret_cast<T*>( dwUser );
			pT->OnSimpleTimer();
		};

	#else

		LONG m_bPending;

		static UINT WM_MM_TIMER( void )
		{
			static UINT MSG_ID = ::RegisterWindowMessage( _T("MPR_2004_WM_MM_TIMER") );
			return MSG_ID;
		}

		static void CALLBACK OnTimerThunk( UINT nId, UINT, DWORD dwUser, DWORD, DWORD )
		{
			T* pT = reinterpret_cast<T*>( dwUser );
			if ( ::InterlockedExchange( &pT->m_bPending, TRUE ) ) return;
			pT->SendNotifyMessage( WM_MM_TIMER(), nId, dwUser );
		};

		LRESULT OnMmTimerThunk( UINT, WPARAM wParam, LPARAM lParam, BOOL& )
		{
			T* pT = static_cast<T*>( this );
			wParam; ATLASSERT( m_nTimerId == wParam || 0 == m_nTimerId );
			lParam; ATLASSERT( reinterpret_cast<LPARAM>( pT ) == lParam );
			if ( 0 != m_nTimerId ) pT->OnSimpleTimer();
			::InterlockedExchange( &m_bPending, FALSE );
			return 0;
		}

	#endif

#endif
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_SIMPLETIMER_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
