#ifndef H_TTLLIVE
#define H_TTLLIVE

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

/////////////////////////////////////////////////////////////////////////////
//
// TTLLive.h : helpers for using TTLLiveCtrl in ATL/WTL or MFC
//
/////////////////////////////////////////////////////////////////////////////

#define USE_TTLLIVE_2

#undef USE_IMPORT
	// define USE_IMPORT to (re-)generate header for TTLLive
	// Note: while in VS, clicking anywhere on the import line will
	// regenerate the .tlh file in the intermediate folder...!

#ifndef USE_TTLLIVE_2
	#ifdef USE_IMPORT
		#import "1\TTLLiveCtrl.dll" \
			no_namespace \
			raw_interfaces_only, raw_native_types, named_guids
	#else
		#include "TTLLiveCtrl.1.tlh"
	#endif
	#include "TTLAPIDEFS.H"
	// use TTLAPI2 names:
	typedef TTLAPI_IMPCHECK_CALC_TYPES TTLAPI_IMPCHECK_CALCULATION_TYPES;
	typedef TTLAPI_DATA_CHANNELS TTLAPI_CHANNELS;
	typedef TTLAPI_CONNECTIONS_TYPES TTLAPI_CONNECTION_TYPES;
	#define TTLAPI_CT_HID TTLAPI_CT_TTCMD_DLL
	#define TTLAPI_OCCMD_FORCE_ADAPTIVE         TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_ADAPTIVE)
    #define TTLAPI_OCCMD_FORCE_BIOGRAPH         TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_BIOGRAPH)
    #define TTLAPI_OCCMD_FORCE_BIORESEARCH      TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_BIORESEARCH)
    #define TTLAPI_OCCMD_FORCE_EEG_SPECTRUM     TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_EEG_SPECTRUM)
    #define TTLAPI_OCCMD_FORCE_MYOTRAC3         TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_MYOTRAC3)
    #define TTLAPI_OCCMD_FORCE_INFINITI_PRO     TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_INFINITI_PRO)
    #define TTLAPI_OCCMD_FORCE_INFINITI_FLEX    TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_INFINITI_FLEX)
    #define TTLAPI_OCCMD_FORCE_INFINITI_MYOTRAC TTLAPI_OCCMD_FORCE_PROT(TTLAPI_PROT_INFINITI_MYOTRAC)
#else
	#ifdef USE_IMPORT
		#import "2\TTLLiveCtrl.dll" \
			no_namespace \
			raw_interfaces_only, raw_native_types, named_guids
	#else
		#include "TTLLiveCtrl.2.tlh"
	#endif
#endif

/////////////////////////////////////////////////////////////////////////////
//
// Pointers and Smart Pointers:
//
/////////////////////////////////////////////////////////////////////////////

#include "atlbase.h"

#ifdef USE_TTLLIVE_2
	typedef ITTLLive2* LPTTLLIVE;
	typedef CComPtr< ITTLLive2 > CTTLLivePtr;
	typedef CComQIPtr< ITTLLive2 > CTTLLiveQIPtr;
#else
	typedef ITTLLive* LPTTLLIVE;
	typedef CComPtr< ITTLLive > CTTLLivePtr;
	typedef CComQIPtr< ITTLLive > CTTLLiveQIPtr;
#endif

/////////////////////////////////////////////////////////////////////////////
//
// Missing from TTLAPIDEFS or typelib:
//
/////////////////////////////////////////////////////////////////////////////

struct TTL_VERSION
{
	unsigned int build : 16;
	unsigned int minor : 8;
	unsigned int major : 8;

	TTL_VERSION( void )
		: major( 0 ), minor( 0 ), build( 0 )
		{ }
	TTL_VERSION( unsigned nMajor, unsigned nMinor, unsigned nBuild )
		: major( nMajor ), minor( nMinor ), build( nBuild )
		{ }
	TTL_VERSION( long n )
		{ reinterpret_cast< long& >( *this ) = n; }
	operator long* ()
		{ return reinterpret_cast< long* >( this ); }
};

inline bool operator <= ( const TTL_VERSION& v1, const TTL_VERSION& v2 )
{
	if ( v1.major < v2.major ) return true;
	if ( v1.major > v2.major ) return false;
	if ( v1.minor < v2.minor ) return true;
	if ( v1.minor > v2.minor ) return false;
	return v1.build <= v2.build;
}

typedef __int16 TTL_RAW_DATA_T;
typedef float TTL_API_DATA_T;

enum TTL_SENSOR_STATUS
{	
	TTL_SENSOR_STATUS_NONE = 0
,	TTL_SENSOR_STATUS_ALL = 0xFFFFFFFFul
};

enum TTL_EVENT_INSERTION_MASK
{	
	TTL_EVENT_INSERTION_MASK_NONE = 0
,	TTL_EVENT_INSERTION_MASK_ALL = 0xFFFFFFFFul
};

enum TTL_INPUT_SWITCH_STATES
{
	TTL_INPUT_SWITCH_INACTIVE = 0
,	TTL_INPUT_SWITCH_ACTIVE = 1
};

#define TTLAPI_UT_DEFAULT ((TTLAPI_UNIT_TYPES)(0))

/////////////////////////////////////////////////////////////////////////////
//
// Debugging
//
/////////////////////////////////////////////////////////////////////////////

class CTtlTime
{
public:
	static double Seconds( void )
	{
		static double spc = get_spc(); // seconds per count
		static __int64 c0 = get_count(); // initial count
		return spc * ( get_count() - c0 );
	}
protected:
	static double get_spc()
	{
		__int64 li = 1;
		::QueryPerformanceFrequency( reinterpret_cast< LARGE_INTEGER* >( &li ) );
		return 1.0 / li;
	}
	static __int64 get_count()
	{
		__int64 li = 0;
		::QueryPerformanceCounter( reinterpret_cast< LARGE_INTEGER* >( &li ) );
		return li;
	}
};

#define TTL_SECONDS() CTtlTime::Seconds()

#ifdef NDEBUG

	inline void CTtlTrace_checkparams( LPCTSTR, ... ) { }
	#define TTL_TRACE CTtlTrace_checkparams

	inline void CTtlTraceBlock_checkparams( LPCTSTR ) { }
	#define TTL_TRACE_BLOCK(szName) CTtlTraceBlock_checkparams( szName )

#else

	class CTtlTrace
	{
	public:
		static void Trace( LPCTSTR szFormat, ... )
		{
			va_list args;
			va_start( args, szFormat ); TraceV( GET, szFormat, args ); va_end( args );
		}
	protected:
		enum INDENT { GET = 0, INC, DEC };
		static size_t get_indent( INDENT ind )
		{
			static size_t n = 0;
			if ( GET == ind ) return n;
			return INC == ind ? n++ : --n;
		}
		static void TraceI( INDENT ind, LPCTSTR szFormat, ... )
		{
			va_list args;
			va_start( args, szFormat ); TraceV( ind, szFormat, args ); va_end( args );
		}
		static void TraceV( INDENT i, LPCTSTR szFormat, va_list args )
		{
			enum { MAX_CHARS = 1024 };
			TCHAR szBuffer[ MAX_CHARS ]; *szBuffer = 0;
			_sntprintf( szBuffer, MAX_CHARS, _T("%X %8.4f : %*s")
				, ::GetCurrentThreadId(), TTL_SECONDS()
				, 4*get_indent( i ), _T("")
				);
			::OutputDebugString( szBuffer );
			_vsntprintf( szBuffer, MAX_CHARS, szFormat, args );
			::OutputDebugString( szBuffer );
		}
	};

	#define TTL_TRACE CTtlTrace::Trace

	class CTtlTraceBlock : CTtlTrace
	{
	public:
		CTtlTraceBlock( LPCTSTR szName ) : m_szName( szName )
		{
			TraceI( INC, _T(">>> %s\n"), m_szName );
			m_s0 = TTL_SECONDS();
		}
		~CTtlTraceBlock( void )
		{
			m_s1 = TTL_SECONDS();
			TraceI( DEC, _T("<<< %s [%.1fms]\n"), m_szName, 1000.0*( m_s1 - m_s0 ) );
		}
	protected:
		LPCTSTR m_szName;
		double m_s0, m_s1;
	};

	#define TTL_TRACE_BLOCK(szName) CTtlTraceBlock ttl_auto_trace( szName )

#endif


inline LPCTSTR TtlToStr( const TTLAPI_IMPCHECK_CALCULATION_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_IMPCHECK_TYPE_RESISTIVE : sz = _T("Resistive"); break;
	case TTLAPI_IMPCHECK_TYPE_TOTAL     : sz = _T("Total"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_CONNECTION_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_CT_UNKNOWN   : sz = _T("Unknown"); break;
	case TTLAPI_CT_COM       : sz = _T("Com"); break;
	case TTLAPI_CT_USB       : sz = _T("USB"); break;
	case TTLAPI_CT_TELEMETRY : sz = _T("Telemetry"); break;
	case TTLAPI_CT_UDP       : sz = _T("UDP"); break;
	case TTLAPI_CT_TCP       : sz = _T("TCP"); break;
	case TTLAPI_CT_FILE      : sz = _T("File"); break;
	case TTLAPI_CT_SSF_FILE  : sz = _T("SSF File"); break;
	case TTLAPI_CT_HID       : sz = _T("HID"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_ENCODER_STATES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_ENC_ST_OFFLINE   : sz = _T("Offline"); break;
	case TTLAPI_ENC_ST_DETECTING : sz = _T("Detecting"); break;
	case TTLAPI_ENC_ST_ONLINE    : sz = _T("Online"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_SENSOR_STATES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_SENS_ST_ENCODER_OFFLINE : sz = _T("Offline"); break;
	case TTLAPI_SENS_ST_UNCONNECTED     : sz = _T("Unconnected"); break;
	case TTLAPI_SENS_ST_NORMAL          : sz = _T("Normal"); break;
	case TTLAPI_SENS_ST_CAL_IN_PROGRESS : sz = _T("Calibrating"); break;
	case TTLAPI_SENS_ST_IC_IN_PROGRESS  : sz = _T("Impedancing"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_CHANNELS nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_CHANNEL_A : sz = _T("A"); break;
	case TTLAPI_CHANNEL_B : sz = _T("B"); break;
	case TTLAPI_CHANNEL_C : sz = _T("C"); break;
	case TTLAPI_CHANNEL_D : sz = _T("D"); break;
	case TTLAPI_CHANNEL_E : sz = _T("E"); break;
	case TTLAPI_CHANNEL_F : sz = _T("F"); break;
	case TTLAPI_CHANNEL_G : sz = _T("G"); break;
	case TTLAPI_CHANNEL_H : sz = _T("H"); break;
	case TTLAPI_CHANNEL_I : sz = _T("I"); break;
	case TTLAPI_CHANNEL_J : sz = _T("J"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_SENSOR_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_SENS_TYPE_ENCODER_OFFLINE   : sz = _T("Offline"); break;
	case TTLAPI_SENS_TYPE_UNKNOWN           : sz = _T("Unknown"); break;
	case TTLAPI_SENS_TYPE_UNCONNECTED       : sz = _T("Unconnected"); break;
	case TTLAPI_SENS_TYPE_EEG               : sz = _T("EEG"); break;
	case TTLAPI_SENS_TYPE_HR_BVP            : sz = _T("HR BVP"); break;
	case TTLAPI_SENS_TYPE_SKIN_CONDUCTANCE  : sz = _T("Skin cond."); break;
	case TTLAPI_SENS_TYPE_EKG_Z             : sz = _T("EKG Z"); break;
	case TTLAPI_SENS_TYPE_EEG_Z             : sz = _T("EEG Z"); break;
	case TTLAPI_SENS_TYPE_MYOSCAN           : sz = _T("MyoScan"); break;
	case TTLAPI_SENS_TYPE_MYOSCAN_PRO_400   : sz = _T("MyoScan Pro 400"); break;
	case TTLAPI_SENS_TYPE_RESPIRATION       : sz = _T("Respiration"); break;
	case TTLAPI_SENS_TYPE_MYOSCAN_Z_HR      : sz = _T("MyoScan Z HR"); break;
	case TTLAPI_SENS_TYPE_MYOSCAN_Z         : sz = _T("MyoScan Z"); break;
	case TTLAPI_SENS_TYPE_GONIOMETER        : sz = _T("Goniometer"); break;
	case TTLAPI_SENS_TYPE_FORCE             : sz = _T("Force"); break;
	case TTLAPI_SENS_TYPE_MYOSCAN_PRO_1600W : sz = _T("MyoScan Pro 1600"); break;
	case TTLAPI_SENS_TYPE_VOLT_ISOLATOR     : sz = _T("Volt isolator"); break;
	case TTLAPI_SENS_TYPE_TEMPERATURE       : sz = _T("Temperature"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_UNIT_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
#pragma warning( disable : 4063 ) // invalid case value
	switch ( nValue )
	{
	case TTLAPI_UT_PERCENT   : sz = _T("Percent"); break;
	case TTLAPI_UT_SENSVOLT  : sz = _T("Sensor Volt"); break;
	case TTLAPI_UT_VRMS      : sz = _T("Rms Volt"); break;
	case TTLAPI_UT_DEGC      : sz = _T("Degree Celcius"); break;
	case TTLAPI_UT_DEGF      : sz = _T("Degree Farenheit"); break;
	case TTLAPI_UT_DEGREE    : sz = _T("Degree"); break;
	case TTLAPI_UT_SIEMENS   : sz = _T("Siemens"); break;
	case TTLAPI_UT_COUNT     : sz = _T("Encoder Count"); break;
	case TTLAPI_UT_ENCVOLT   : sz = _T("Encoder Volt"); break;
	case TTLAPI_UT_PU        : sz = _T("PU"); break;
	case TTLAPI_UT_SENSMVOLT : sz = _T("Sensor milliVolt"); break;
	case TTLAPI_UT_SENSUVOLT : sz = _T("Sensor microVolt"); break;
	case TTLAPI_UT_MVRMS     : sz = _T("Rms milliVolt"); break;
	case TTLAPI_UT_UVRMS     : sz = _T("Rms microVolt"); break;
	case TTLAPI_UT_USIEMENS  : sz = _T("microSiemens"); break;
	/*
	TTLLIVE bug: get_UnitType can return 0xbaadf00d if sensor type not set
	*/
	case 0xbaadf00d : sz = _T("0xBAADF00D"); break;
	default : ATLASSERT( false ); break;
	}
#pragma warning( default : 4063 )
	return sz;
}

#ifdef USE_TTLLIVE_2

inline LPCTSTR TtlToStr( const TTLAPI_SYNC_STATES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_SST_FAILED_RETRYING : sz = _T("Failed Retrying"); break;
	case TTLAPI_SST_FAILED_TIMEOUT  : sz = _T("Failed Timeout"); break;
	case TTLAPI_SST_FAILED_OFFLINE  : sz = _T("Failed Offline"); break;
	case TTLAPI_SST_FAILED          : sz = _T("Failed"); break;
	case TTLAPI_SST_NOT_STARTED     : sz = _T("Not Started"); break;
	case TTLAPI_SST_IN_PROGRESS     : sz = _T("In Progress"); break;
	case TTLAPI_SST_SUCCEEDED       : sz = _T("Succeeded"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_PROTOCOL_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
	switch ( nValue )
	{
	case TTLAPI_PROT_UNKNOWN              : sz = _T("Unknown"); break;
	case TTLAPI_PROT_ADAPTIVE             : sz = _T("Adaptive"); break;
	case TTLAPI_PROT_BIOGRAPH             : sz = _T("BioGraph"); break;
	case TTLAPI_PROT_BIORESEARCH          : sz = _T("BioResearch"); break;
	case TTLAPI_PROT_EEG_SPECTRUM         : sz = _T("EEG Spectrum"); break;
	case TTLAPI_PROT_MYOTRAC3             : sz = _T("MyoTrac3"); break;
	case TTLAPI_PROT_INFINITI_PRO         : sz = _T("Infiniti Pro"); break;
	case TTLAPI_PROT_INFINITI_FLEX        : sz = _T("Infiniti Flex"); break;
	case TTLAPI_PROT_INFINITI_CF_RECORDED : sz = _T("Infiniti Recorded"); break;
	case TTLAPI_PROT_INFINITI_MYOTRAC     : sz = _T("Infiniti MyoTrac"); break;
	default : ATLASSERT( false ); break;
	}
	return sz;
}

inline LPCTSTR TtlToStr( const TTLAPI_EVENT_TYPES nValue )
{
	LPTSTR sz = _T("UNDOCUMENTED");
#pragma warning( disable : 4063 ) // invalid case value
	switch ( nValue )
	{
	case 0 : sz = _T("None"); break;
	case TTLAPI_EVT_EVENT_SWITCH         : sz = _T("Switch ON"); break;
	case TTLAPI_EVT_SENSOR_STATE_CHANGE  : sz = _T("Sensor State"); break;
	case TTLAPI_EVT_ENCODER_STATE_CHANGE : sz = _T("Encoder State"); break;
	case TTLAPI_EVT_SYNC_STATE_CHANGE    : sz = _T("Sync State"); break;
	case TTLAPI_EVT_EVENT_SWITCH_OFF     : sz = _T("Switch OFF"); break;
	case TTLAPI_EVT_SENSOR_REMOVED       : sz = _T("Sensor Removed"); break;
	case TTLAPI_EVT_DATA_GAP             : sz = _T("Data Gap"); break;
	default : ATLASSERT( false ); break;
	}
#pragma warning( default : 4063 )
	return sz;
}

#endif

/////////////////////////////////////////////////////////////////////////////
//
// Default (debugging) ITTLLiveEvents handlers.
//
/////////////////////////////////////////////////////////////////////////////

struct ATL_NO_VTABLE CTTLLiveDefaultEventHandlers
{
	void OnChannelData( long liChannelHND, long liSamplesCount )
	{
		TTL_TRACE( _T("OnChannelData %ld %ld\n")
			, liChannelHND
			, liSamplesCount
			);
	}
	void OnEventSwitch( long liEncoderHND, long liTickCount )
	{
		TTL_TRACE( _T("OnEventSwitch %ld %ld\n")
			, liEncoderHND
			, liTickCount
			);
	}
	void OnSensorStateChange( long liChannelHND, long liNewState )
	{
		TTL_TRACE( _T("OnSensorStateChange %ld %ld=%s\n")
			, liChannelHND
			, liNewState, TtlToStr( static_cast< TTLAPI_SENSOR_STATES >( liNewState ) )
			);
	}
	void OnEncoderStateChange( long liEncoderHND, long liNewState )
	{
		TTL_TRACE( _T("OnEncoderStateChange %ld %ld=%s\n")
			, liEncoderHND
			, liNewState, TtlToStr( static_cast< TTLAPI_ENCODER_STATES >( liNewState ) )
			);
	}
	void OnDataOverflow( long liChannelHND, long liReserved )
	{
		TTL_TRACE( _T("OnDataOverflow %ld [%ld]\n")
			, liChannelHND
			, liReserved
			);
	}
	void OnDataError( long liEncoderHND, long liReserved )
	{
		TTL_TRACE( _T("OnDataError %ld [%ld]\n")
			, liEncoderHND
			, liReserved
			);
	}
	void OnDataTimeOut( long liEncoderHND, long liReserved )
	{
		TTL_TRACE( _T("OnDataTimeOut %ld [%ld]\n")
			, liEncoderHND
			, liReserved
			);
	}
	void OnImpCheckResults( long liChannelHND, long liReserved )
	{
		TTL_TRACE( _T("OnImpCheckResults %ld [%ld]\n")
			, liChannelHND
			, liReserved
			);
	}
#ifdef USE_TTLLIVE_2
	void OnSyncStateChange( long liNewState, long liReserved )
	{
		TTL_TRACE( _T("OnSyncStateChange %ld=%s [%ld]\n")
			, liNewState, TtlToStr( static_cast< TTLAPI_SYNC_STATES >( liNewState ) )
			, liReserved
			);
	}
	void OnEventSwitchOff( long liEncoderHND, long liTickCount )
	{
		TTL_TRACE( _T("OnEventSwitchOff %ld %ld\n")
			, liEncoderHND
			, liTickCount
			);
	}
	void OnDataGap( long liEncoderHND, long li2 )
	{
		TTL_TRACE( _T("OnDataGap %ld %ld\n")
			, liEncoderHND
			, li2
			);
	}
	void OnTicksAheadUpdate( long liEncoderHND, long liReserved )
	{
		TTL_TRACE( _T("OnTicksAheadUpdate %ld [%ld]\n")
			, liEncoderHND
			, liReserved
			);
	}
	void OnEventOutputFailure( long liEncoderHND, long bRequestedState )
	{
		TTL_TRACE( _T("OnEventOutputFailure %ld %ld\n")
			, liEncoderHND
			, bRequestedState
			);
	}
#endif
};

/////////////////////////////////////////////////////////////////////////////
//
// Simple IDispatch for sinking the events fired on _ITTLLiveEvents.
//
/////////////////////////////////////////////////////////////////////////////

template< typename T >
class ATL_NO_VTABLE ITTLLiveEventsImpl : public IDispatch
{
public:

	typedef ITTLLiveEventsImpl< T > ITTLLiveEventsImpl_type;

private:

	/*
	We could "simply" derive from .IDispEventSimpleImpl and use

		// [id(0x00000001)] HRESULT ChannelData( [in] long liChannelHND, [in] long liSamplesCount);
		static _ATL_FUNC_INFO s_fiChannelData = { CC_STDCALL, VT_EMPTY, 2, { VT_INT, VT_INT } };

		BEGIN_SINK_MAP(T)
			SINK_ENTRY_INFO( 1, DIID__ITTLLiveEvents, 1, OnChannelData, &s_fiChannelData )
		END_SINK_MAP()

		STDMETHOD_(void,OnChannelData)( long liChannelHND, long liSamplesCount );

	but
		1) We would need to modify BEGIN_SINK_MAP and SINK_ENTRY_INFO to
		   have a place to define the static _ATL_FUNC_INFO's
		2) I don't like the confusion between return type VT_EMPTY and VT_HRESULT
		3) The events are simple enough that we don't need even IDispEventSimpleImpl's simple
		   lookup mechanism.
	*/

	enum
	{ DISPID_ChannelData = 1
	, DISPID_EventSwitch
	, DISPID_SensorStateChange
	, DISPID_EncoderStateChange
	, DISPID_DataOverflow
	, DISPID_DataError
	, DISPID_DataTimeOut
	, DISPID_ImpCheckResults
#ifdef USE_TTLLIVE_2
	, DISPID_SyncStateChange
	, DISPID_EventSwitchOff
	, DISPID_DataGap
	, DISPID_TicksAheadUpdate
	, DISPID_EventOutputFailure
#endif
	};

	STDMETHOD(QueryInterface)( REFIID riid, LPVOID* ppv )
	{
		if	(  IID_IUnknown == riid
			|| IID_IDispatch ==  riid
			|| DIID__ITTLLiveEvents == riid
			)
		{
			if ( ppv == NULL ) return E_POINTER;
			*ppv = this;
			AddRef();
			return S_OK;
		}
		return E_NOINTERFACE;
	}

	STDMETHOD_(ULONG, AddRef)() { return 1; }
	STDMETHOD_(ULONG, Release)() { return 1; }

	STDMETHOD(GetTypeInfoCount)( UINT* ) { return E_NOTIMPL; }
	STDMETHOD(GetTypeInfo)( UINT, LCID, ITypeInfo** ) { return E_NOTIMPL; }
	STDMETHOD(GetIDsOfNames)( REFIID, LPOLESTR*, UINT, LCID, DISPID* ) { return E_NOTIMPL; }

	STDMETHOD(Invoke)( DISPID dispidMember
		, REFIID /*riid*/
		, LCID /*lcid*/
		, WORD /*wFlags*/
		, DISPPARAMS* pdispparams
		, VARIANT* /*pvarResult*/
		, EXCEPINFO* /*pexcepinfo*/
		, UINT* /*puArgErr*/
		)
	{
		UINT
		uArgErr = 0;
		CComVariant varg0( 0, VT_I4 );
		::DispGetParam( pdispparams, 0, VT_I4, &varg0, &uArgErr );
		long l0 = varg0.lVal;
		uArgErr = 0;
		CComVariant varg1( 0, VT_I4 );
		::DispGetParam( pdispparams, 1, VT_I4, &varg1, &uArgErr );
		long l1 = varg1.lVal;

		T* pT = static_cast< T* >( this );
		switch ( dispidMember )
		{
		case DISPID_ChannelData        : pT->OnChannelData( l0, l1 ); break;
		case DISPID_EventSwitch        : pT->OnEventSwitch( l0, l1 ); break;
		case DISPID_SensorStateChange  : pT->OnSensorStateChange( l0, l1 ); break;
		case DISPID_EncoderStateChange : pT->OnEncoderStateChange( l0, l1 ); break;
		case DISPID_DataOverflow       : pT->OnDataOverflow( l0, l1 ); break;
		case DISPID_DataError          : pT->OnDataError( l0, l1 ); break;
		case DISPID_DataTimeOut        : pT->OnDataTimeOut( l0, l1 ); break;
		case DISPID_ImpCheckResults    : pT->OnImpCheckResults( l0, l1 ); break;
	#ifdef USE_TTLLIVE_2
		case DISPID_SyncStateChange    : pT->OnSyncStateChange( l0, l1 ); break;
		case DISPID_EventSwitchOff     : pT->OnEventSwitchOff( l0, l1 ); break;
		case DISPID_DataGap            : pT->OnDataGap( l0, l1 ); break;
		case DISPID_TicksAheadUpdate   : pT->OnTicksAheadUpdate( l0, l1 ); break;
		case DISPID_EventOutputFailure : pT->OnEventOutputFailure( l0, l1 ); break;
	#endif
		default:
			ATLTRACE( _T("*** UNKNOWN ITTLLiveEvents %ld ( %ld %ld )\n")
				, dispidMember
				, l0
				, l1
				);
			return E_NOTIMPL;
		}

		return S_OK;
	}
};

/////////////////////////////////////////////////////////////////////////////
//
// Three choices for sinking events: IDispatch, thread messages, no events.
//
/////////////////////////////////////////////////////////////////////////////

template< typename T >
class ATL_NO_VTABLE CTTLLiveDispatchSink : public ITTLLiveEventsImpl< T >
{
public:

	typedef CTTLLiveDispatchSink< T > CTTLLiveSink_type;

	CTTLLiveDispatchSink(): m_dwEventCookie( 0 ) { }
	~CTTLLiveDispatchSink() { ATLASSERT( 0 == m_dwEventCookie ); }

	BOOL PreTranslateMessage( MSG* /*pMsg*/ )
	{
		return FALSE;
	}

	HRESULT TtlHookupSink( CTTLLivePtr & oTTLLive )
	{
		ATLASSERT( 0 == m_dwEventCookie );
		return AtlAdvise( oTTLLive, (IUnknown*)this, DIID__ITTLLiveEvents, &m_dwEventCookie );
		/* AtlAdvise ==
		CComPtr< IConnectionPointContainer > pCPC;
		CComPtr< IConnectionPoint > pCP;
		HRESULT hRes = pUnkCP->QueryInterface(IID_IConnectionPointContainer, (void**)&pCPC);
		if (SUCCEEDED(hRes)) hRes = pCPC->FindConnectionPoint(iid, &pCP);
		if (SUCCEEDED(hRes)) hRes = pCP->Advise(pUnk, pdw);
		return hRes;
		*/
	}
	HRESULT TtlUnhookSink( CTTLLivePtr & oTTLLive )
	{
		if ( 0 == m_dwEventCookie ) return S_OK;
		HRESULT hr = AtlUnadvise( oTTLLive, DIID__ITTLLiveEvents, m_dwEventCookie );
		m_dwEventCookie = 0;
		return hr;
		/* AtlUnadvise ==
		CComPtr< IConnectionPointContainer > pCPC;
		CComPtr< IConnectionPoint > pCP;
		HRESULT hRes = pUnkCP->QueryInterface(IID_IConnectionPointContainer, (void**)&pCPC);
		if (SUCCEEDED(hRes)) hRes = pCPC->FindConnectionPoint(iid, &pCP);
		if (SUCCEEDED(hRes)) hRes = pCP->Unadvise(dw);
		return hRes;
		*/
	}

private:

	DWORD m_dwEventCookie;
};

template< typename T >
class ATL_NO_VTABLE CTTLLiveMessageSink
{
public:

	typedef CTTLLiveMessageSink< T > CTTLLiveSink_type;

	BOOL PreTranslateMessage( MSG* pMsg )
	{
		if ( NULL != pMsg->hwnd ) return FALSE;

		T* pT = static_cast< T* >( this );
		switch ( pMsg->message )
		{
		case TTLAPI_WM_ON_CHANNEL_DATA         : pT->OnChannelData( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_EVENT_SWITCH         : pT->OnEventSwitch( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_SENSOR_STATE_CHANGE  : pT->OnSensorStateChange( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_ENCODER_STATE_CHANGE : pT->OnEncoderStateChange( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_DATA_OVERFLOW        : pT->OnDataOverflow( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_DATA_ERROR           : pT->OnDataError( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_DATA_TIMEOUT         : pT->OnDataTimeOut( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_IMP_CHECK_RESULTS    : pT->OnImpCheckResults( pMsg->wParam, pMsg->lParam ); break;
	#ifdef USE_TTLLIVE_2
		case TTLAPI_WM_ON_SYNC_STATE_CHANGE    : pT->OnSyncStateChange( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_EVENT_SWITCH_OFF     : pT->OnEventSwitchOff( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_DATA_GAP             : pT->OnDataGap( pMsg->wParam, pMsg->lParam ); break;
		case TTALPI_WM_ON_TICKSAHEAD_UPDATE    : pT->OnTicksAheadUpdate( pMsg->wParam, pMsg->lParam ); break;
		case TTLAPI_WM_ON_EVENT_OUTPUT_FAILURE : pT->OnEventOutputFailure( pMsg->wParam, pMsg->lParam ); break;
	#endif
		default: return FALSE;
		}

		return TRUE;
	}

	HRESULT TtlHookupSink( CTTLLivePtr & oTTLLive )
	{
		return oTTLLive->RegisterClientThreadId( ::GetCurrentThreadId() );
	}

	HRESULT TtlUnhookSink( CTTLLivePtr & oTTLLive )
	{
		return oTTLLive->RegisterClientThreadId( 0 );
	}
};

template< typename T >
class ATL_NO_VTABLE CTTLLiveNoSink
{
public:

	typedef CTTLLiveNoSink< T > CTTLLiveSink_type;

	BOOL PreTranslateMessage( MSG* /*pMsg*/ )
	{
		return FALSE;
	}

	HRESULT TtlHookupSink( CTTLLivePtr & /*oTTLLive*/ )
	{
		ATLTRACE( _T("Not receiving ITTLLiveEvents\n") );
		return S_OK;
	}

	HRESULT TtlUnhookSink( CTTLLivePtr & /*oTTLLive*/ )
	{
		return S_OK;
	}
};

/////////////////////////////////////////////////////////////////////////////
//
// CTTLLive interface pointer management
//
/////////////////////////////////////////////////////////////////////////////

template< typename T >
class ATL_NO_VTABLE CTTLLive : public CTTLLiveDefaultEventHandlers
{
public:

	typedef CTTLLive< T > CTTLLive_type;

	CTTLLivePtr m_oTTLLive;

	CTTLLive()
		: m_hr( S_OK )
	#ifdef USE_TTLLIVE_SIDE_BY_SIDE
		, m_hTTLLiveCtrl( NULL )
	#endif
	{
	}

	~CTTLLive()
	{
		ATLASSERT( ! m_oTTLLive );
	#ifdef USE_TTLLIVE_SIDE_BY_SIDE
		ATLASSERT( NULL == m_hTTLLiveCtrl );
	#endif
	}

	HRESULT TtlGetLastError() const { return m_hr; }

	HRESULT TtlAttachControl( LPTTLLIVE pTTLLive )
	{
		ATLASSERT( ! m_oTTLLive );
		if ( NULL != m_oTTLLive.p ) TtlDestroyControl();
		m_oTTLLive = pTTLLive;
		return ! m_oTTLLive ? E_NOINTERFACE : S_OK;
	}

	HRESULT TtlCreateControl( LPCTSTR szTTLLiveCtrl = NULL )
	{
		ATLASSERT( ! m_oTTLLive );
		if ( NULL != m_oTTLLive.p ) TtlDestroyControl();
		m_hr = CreateInstance( szTTLLiveCtrl );
		if ( ! m_oTTLLive ) return m_hr;
		m_hr = static_cast< T* >( this )->TtlHookupSink( m_oTTLLive );
		return m_hr;
	}

	void TtlDestroyControl()
	{
		if ( ! m_oTTLLive ) return;
		m_oTTLLive->put_NotificationMask( 0 );
		m_oTTLLive->CloseConnections();
		static_cast< T* >( this )->TtlUnhookSink( m_oTTLLive );
		ReleaseInstance();
	}

protected:

	mutable HRESULT m_hr;

#ifdef USE_TTLLIVE_SIDE_BY_SIDE

	HMODULE m_hTTLLiveCtrl;

	HRESULT CreateInstance( LPCTSTR szTTLLiveCtrl )
	{
	#ifdef USE_TTLLIVE_SIDE_BY_SIDE
		do
		{
			if ( NULL == szTTLLiveCtrl ) szTTLLiveCtrl = _T("TTLLiveCtrl.dll");
			m_hTTLLiveCtrl = ::LoadLibrary( szTTLLiveCtrl );
			if ( NULL == m_hTTLLiveCtrl ) break;

			LPFNGETCLASSOBJECT pfGetClassObject = reinterpret_cast< LPFNGETCLASSOBJECT >(
				::GetProcAddress( m_hTTLLiveCtrl, "DllGetClassObject" )
				);
			if ( NULL == pfGetClassObject ) break;

			CComPtr< IClassFactory > oClassFactory;
			pfGetClassObject( CLSID_TTLLive, IID_IClassFactory
				, reinterpret_cast< LPVOID* >( &oClassFactory )
				);
			if ( ! oClassFactory ) break;

			oClassFactory->CreateInstance( NULL, __uuidof(CTTLLivePtr::_PtrClass)
				, reinterpret_cast< LPVOID* >( &m_oTTLLive )
				);
			if ( ! m_oTTLLive ) break;

			return S_OK;
		}
		while ( 0 );
		if ( NULL != m_hTTLLiveCtrl )
		{
			::FreeLibrary( m_hTTLLiveCtrl );
			m_hTTLLiveCtrl = NULL;
		}
	#else
		szTTLLiveCtrl;
	#endif

		return m_oTTLLive.CoCreateInstance( CLSID_TTLLive );
	}

	void ReleaseInstance( void )
	{
		m_oTTLLive.Release();

	#ifdef USE_TTLLIVE_SIDE_BY_SIDE
		if ( NULL == m_hTTLLiveCtrl ) return;
		::FreeLibrary( m_hTTLLiveCtrl );
		m_hTTLLiveCtrl = NULL;
	#endif
	}

#endif
};

/////////////////////////////////////////////////////////////////////////////
//
// Simple implementation for watching PnP events related to TT-USB
//
/////////////////////////////////////////////////////////////////////////////

#ifdef BEGIN_MSG_MAP

struct ATL_NO_VTABLE CTTLLivePnPEventsSinkDefaultHandlers
{
	void OnDeviceEnumerate( int nIndex, LPCTSTR szDevice, bool bPresent )
	{
		TTL_TRACE( _T("OnDeviceEnumerate %d %s %s\n")
			, nIndex
			, szDevice
			, bPresent ? _T("PRESENT") : _T("ABSENT")
			);
	}
	void OnDeviceArrival( LPCTSTR szDevice )
	{
		TTL_TRACE( _T("OnDeviceArrival %s\n")
			, szDevice
			);
	}
	void OnDeviceRemoved( LPCTSTR szDevice )
	{
		TTL_TRACE( _T("OnDeviceRemoved %s\n")
			, szDevice
			);
	}
};

#if (WINVER >= 0x0500)

	#include "dbt.h" // for WM_DEVICECHANGE related defines
	#include "shlwapi.h" // for StrChr and friends
	#pragma comment(lib, "shlwapi.lib")
	#include "setupapi.h" // for SetupDiGetClassDevs and friends
	#pragma comment(lib, "setupapi.lib")

	template< typename T >
	class ATL_NO_VTABLE CTTLLivePnPEventsSink
		: public CTTLLivePnPEventsSinkDefaultHandlers
	{
	public:

		typedef CTTLLivePnPEventsSink< T > CTTLLivePnPEventsSink_type;

		CTTLLivePnPEventsSink() : m_hDevNotify( NULL )
		{
		}

		~CTTLLivePnPEventsSink()
		{
			ATLASSERT( NULL == m_hDevNotify );
		}

		void TtlPnPEnumerate( void )
		{
			GUID guid = { 0, };
			GetTtlGuid( guid );
			HDEVINFO hDevs = ::SetupDiGetClassDevs( &guid, NULL, NULL, DIGCF_DEVICEINTERFACE );
			if ( INVALID_HANDLE_VALUE == hDevs ) return;
			for ( DWORD nIndex = 0; ; ++nIndex )
			{
				SP_DEVICE_INTERFACE_DATA did = { sizeof(did), 0, };
				if ( ! ::SetupDiEnumDeviceInterfaces( hDevs, NULL, &guid, nIndex, &did ) ) break;
				BYTE abBuffer[ MAX_DEVICE_DETAIL ];
				PSP_DEVICE_INTERFACE_DETAIL_DATA pdidd = reinterpret_cast< PSP_DEVICE_INTERFACE_DETAIL_DATA >( abBuffer );
				pdidd->cbSize = sizeof( SP_DEVICE_INTERFACE_DETAIL_DATA );
				if ( ! ::SetupDiGetDeviceInterfaceDetail( hDevs, &did, pdidd, sizeof(abBuffer), NULL, NULL ) ) break;
				TCHAR szSerialNumber[ MAX_SERIAL_NUMBER ] = _T("");
				GetSerialNumber( pdidd->DevicePath, szSerialNumber, MAX_SERIAL_NUMBER );
				T* pT = static_cast< T* >( this );
				pT->OnDeviceEnumerate( nIndex, szSerialNumber, 0 != ( SPINT_ACTIVE & did.Flags ) );
			}
			::SetupDiDestroyDeviceInfoList( hDevs );
		}

		bool TtlPnPRegister( void )
		{
			if ( NULL != m_hDevNotify ) return true; // already registered
			DEV_BROADCAST_DEVICEINTERFACE dbdi = { sizeof(dbdi), DBT_DEVTYP_DEVICEINTERFACE, 0, };
			GetTtlGuid( dbdi.dbcc_classguid );
			DWORD dwFlags = DEVICE_NOTIFY_WINDOW_HANDLE /*| DEVICE_NOTIFY_ALL_INTERFACE_CLASSES*/;
			T* pT = static_cast< T* >( this );
			return NULL != ( m_hDevNotify = ::RegisterDeviceNotification( pT->m_hWnd, &dbdi, dwFlags ) );
		}

		void TtlPnPUnregister( void )
		{
			if ( NULL == m_hDevNotify ) return; // never registered
			::UnregisterDeviceNotification( m_hDevNotify );
			m_hDevNotify = NULL;
		}

		BEGIN_MSG_MAP( CTTLLivePnPEventsSink_type )
			MESSAGE_HANDLER( WM_DEVICECHANGE, OnDeviceChange )
		END_MSG_MAP()

	private:

		HDEVNOTIFY m_hDevNotify;

		enum { MAX_DEVICE_DETAIL = 128*sizeof(TCHAR) };
		enum { MAX_SERIAL_NUMBER = 32 };

		void GetTtlGuid( GUID & guid ) const
		{
			// TTUSB interface guid: {219D0508-57A8-4FF5-97A1-BD86587C6C7E}
			// TODO: how do I find these out dynamically?
			::CLSIDFromString( OLESTR("{219D0508-57A8-4FF5-97A1-BD86587C6C7E}"), &guid );
		}

		void GetSerialNumber( LPCTSTR szDeviceName, LPTSTR szSerialNumber, int cMaxLength ) const
		{
			// Extract Serial Number from device path.
			// Proper way to do this would be to open device and do some usb device i/o control.
			if ( NULL == szDeviceName || NULL == szSerialNumber || 1 > cMaxLength ) return;
			// find second '#':
			LPCTSTR szStart;
			if ( NULL == ( szStart = ::StrChr( szDeviceName, _T('#') ) ) ) return;
			if ( NULL == ( szStart = ::StrChr( szStart+1, _T('#') ) ) ) return;
			++szStart;
			// find third '#':
			LPCTSTR szEnd;
			if ( NULL == ( szEnd = ::StrChr( szStart, _T('#') ) ) ) return;
			// copy from szStart up to but not including szEnd:
			int cLength = szEnd - szStart;
			if ( cMaxLength <= cLength ) cLength = cMaxLength - 1;
			::StrCpyN( szSerialNumber, szStart, cLength + 1 ); // or use lstrcpyn, same thing actually
			::CharUpper( szSerialNumber ); // give uniform case
		}

		LRESULT OnDeviceChange( UINT /*uMsg*/, WPARAM wParam, LPARAM lParam, BOOL& /*bHandled*/ )
		{
			switch ( wParam )
			{
			case DBT_DEVICEARRIVAL :
				{
					PDEV_BROADCAST_HDR pDbtHdr = reinterpret_cast< PDEV_BROADCAST_HDR >( lParam );
					if ( DBT_DEVTYP_DEVICEINTERFACE != pDbtHdr->dbch_devicetype ) break;
					PDEV_BROADCAST_DEVICEINTERFACE pDbtDi = reinterpret_cast< PDEV_BROADCAST_DEVICEINTERFACE >( pDbtHdr );
					TCHAR szSerialNumber[ MAX_SERIAL_NUMBER ] = _T("");
					GetSerialNumber( pDbtDi->dbcc_name, szSerialNumber, MAX_SERIAL_NUMBER );
					T* pT = static_cast< T* >( this );
					pT->OnDeviceArrival( szSerialNumber );
					break;
				}
			case DBT_DEVICEREMOVECOMPLETE :
				{
					PDEV_BROADCAST_HDR pDbtHdr = reinterpret_cast< PDEV_BROADCAST_HDR >( lParam );
					if ( DBT_DEVTYP_DEVICEINTERFACE != pDbtHdr->dbch_devicetype ) break;
					PDEV_BROADCAST_DEVICEINTERFACE pDbtDi = reinterpret_cast< PDEV_BROADCAST_DEVICEINTERFACE >( pDbtHdr );
					TCHAR szSerialNumber[ MAX_SERIAL_NUMBER ] = _T("");
					GetSerialNumber( pDbtDi->dbcc_name, szSerialNumber, MAX_SERIAL_NUMBER );
					T* pT = static_cast< T* >( this );
					pT->OnDeviceRemoved( szSerialNumber );
					break;
				}
			}
			return 0;
		}
	};

#else

	template< typename T >
	class ATL_NO_VTABLE CTTLLivePnPEventsSink
		: public CTTLLivePnPEventsSinkDefaultHandlers
	{
	public:

		typedef CTTLLivePnPEventsSink< T > CTTLLivePnPEventsSink_type;

		void TtlPnPEnumerate( void )
		{
		}

		bool TtlPnPRegister( void )
		{
			ATLTRACE( _T("*** TTLLive PnP events not supported. Define WINVER >= 0x0500 ***\n") );
			return false;
		}

		void TtlPnPUnregister( void )
		{
		}

		// needed so T can keep CHAIN_MSG_MAP( CTTLLivePnPEventsSink_type )
		BEGIN_MSG_MAP( CTTLLivePnPEventsSink_type )
		END_MSG_MAP()
	};

#endif

template<>
class ATL_NO_VTABLE CTTLLivePnPEventsSink< void >
	: public CTTLLivePnPEventsSinkDefaultHandlers
{
public:

	typedef CTTLLivePnPEventsSink< T > CTTLLivePnPEventsSink_type;

	void TtlPnPEnumerate( void )
	{
	}

	bool TtlPnPRegister( void )
	{
		return true;
	}

	void TtlPnPUnregister( void )
	{
	}

	// needed so T can keep CHAIN_MSG_MAP( CTTLLivePnPEventsSink_type )
	BEGIN_MSG_MAP( CTTLLivePnPEventsSink_type )
	END_MSG_MAP()
};

#endif // ! BEGIN_MSG_MAP

#endif //  H_TTLLIVE