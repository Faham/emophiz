// viewconfig.h : interface of the CViewConfig class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_VIEWCONFIG_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
#define AFX_VIEWCONFIG_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include ".\TTLLive.h"

/*
TODO: use IDispatch to discover properties
*/

class CViewConfig : public CPaneContainerImpl<CViewConfig>
{
public:

	typedef CPaneContainerImpl<CViewConfig> CPaneContainerImpl_type;

	DECLARE_WND_CLASS_EX( _T("WtlSample:TTLExplorer:ViewCalib"), 0, -1 )

	void ResetTree( CTTLLivePtr& oTTLLive );
	void ResetList( LPNMTREEVIEW lpTvHdr, CTTLLivePtr& oTTLLive, double nConnectTime );

protected:

	CHorSplitterWindow m_wndHSplitter;
	CTreeViewCtrl m_wndExplorer;
	CListViewCtrl m_wndProperties;

	enum
	{
		ITEM_TYPE_MASK = 0x00000003
	,	ITEM_GLOBAL    = 0x00000000
	,	ITEM_ENCODER   = 0x00000001
	,	ITEM_CHANNEL   = 0x00000002
	};
	HTREEITEM m_htiTTLLive;
	CSimpleMap<long,HTREEITEM> m_mapEncoders;
	CSimpleMap<long,HTREEITEM> m_mapChannels;

	BEGIN_MSG_MAP_EX( CViewConfig )
		MSG_WM_CREATE( OnCreate )
		CHAIN_MSG_MAP( CPaneContainerImpl_type )
	END_MSG_MAP()

	LRESULT OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ );

	void AppendProp( LPCTSTR szProp, LPCTSTR szValue )
	{
		LVITEM item = { 0 };
		item.mask = LVIF_TEXT;
		item.iItem = m_wndProperties.GetItemCount();
		item.pszText = (LPTSTR)( szProp );
		item.iItem = m_wndProperties.InsertItem( &item );

		item.iSubItem = 1;
		item.pszText = (LPTSTR)( szValue );
		m_wndProperties.SetItem( &item );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, LPCTSTR szValue )
	{
		if ( SUCCEEDED(hr) )
		{
			AppendProp( szProp, szValue );
			return;
		}

		CString szError( _T("ERROR: ") );

		do
		{
			// try system error message:

			LPTSTR szBuffer = TEXT("");
			DWORD bSysError =
			::FormatMessage( FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM
				, NULL, hr, 0, (LPTSTR)( &szBuffer ), 0, NULL
				);
			if ( bSysError )
			{
				szError += szBuffer;
				::LocalFree( szBuffer );
				break;
			}

			// try IErrorInfo description:

			CComPtr<IErrorInfo> spErrorInfo;
			if ( FAILED( ::GetErrorInfo( 0, &spErrorInfo ) ) )
			{
				szError += _T("No error information present or supported");
				break;
			}

			BSTR bstrDescription = NULL;
			if ( FAILED( spErrorInfo->GetDescription( &bstrDescription ) ) )
			{
				szError += _T("No error description given");
				break;
			}

			szError += bstrDescription;
			::SysFreeString( bstrDescription );

		}
		while ( 0 );

		AppendProp( szProp, szError );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const bool bValue )
	{
		AppendProp( hr, szProp, bValue ? _T("True") : _T("False") );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const BSTR bstrValue )
	{
		AppendProp( hr, szProp, CString(bstrValue) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const DATE dValue, bool bTime )
	{
		CComBSTR bstrValue;
		::VarBstrFromDate( dValue, 0, bTime ? VAR_TIMEVALUEONLY : 0, &bstrValue );
		AppendProp( hr, szProp, bstrValue );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const long nValue )
	{
		CString sz;
		sz.Format( _T("%ld"), nValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const long nValue, LPCTSTR szValue )
	{
		CString sz;
		sz.Format( _T("%ld - %s"), nValue, szValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const long nValue, const BSTR bstrValue )
	{
		AppendProp( hr, szProp, nValue, CString(bstrValue) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const float nValue )
	{
		CString sz;
		sz.Format( _T("%g"), nValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const double nValue )
	{
		CString sz;
		sz.Format( _T("%.4f"), nValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTL_VERSION & vValue )
	{
		CString sz;
		sz.Format( _T("%u.%u.%u"), vValue.major, vValue.minor, vValue.build );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTL_SENSOR_STATUS nValue )
	{
		CString sz;
		sz.Format( _T("%#0.4lx"), nValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTL_EVENT_INSERTION_MASK nValue )
	{
		CString sz;
		sz.Format( _T("%#0.4lx"), nValue );
		AppendProp( hr, szProp, sz );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_IMPCHECK_CALCULATION_TYPES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_CONNECTION_TYPES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_ENCODER_STATES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_SENSOR_STATES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_CHANNELS nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_SENSOR_TYPES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_UNIT_TYPES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

#ifdef USE_TTLLIVE_2

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_PROTOCOL_TYPES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

	void AppendProp( HRESULT hr, LPCTSTR szProp, const TTLAPI_SYNC_STATES nValue )
	{
		AppendProp( hr, szProp, nValue, TtlToStr( nValue ) );
	}

#endif
};


/////////////////////////////////////////////////////////////////////////////

inline LRESULT CViewConfig::OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ )
{
	this->SetPaneContainerExtendedStyle( PANECNT_NOCLOSEBUTTON ); // for now

	m_wndClient =
	m_wndHSplitter.Create( *this, rcDefault, NULL
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		);
	m_wndExplorer.Create( m_wndHSplitter, rcDefault, NULL
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		| TVS_HASBUTTONS | TVS_HASLINES | TVS_LINESATROOT | TVS_SHOWSELALWAYS
		, WS_EX_CLIENTEDGE // WS_EX_STATICEDGE is maybe nicer?
		);
	m_wndProperties.Create( m_wndHSplitter, rcDefault, NULL
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		| LVS_REPORT | LVS_NOSORTHEADER
		, WS_EX_CLIENTEDGE
		);
	m_wndHSplitter.SetSplitterPanes( m_wndExplorer, m_wndProperties );
	m_wndHSplitter.m_nProportionalPos = m_wndHSplitter.m_nPropMax / 3;

	m_wndProperties.SetExtendedListViewStyle( LVS_EX_FULLROWSELECT );
	m_wndProperties.InsertColumn( 0, _T("Property"), LVCFMT_LEFT, 110, 0 );
	m_wndProperties.InsertColumn( 1, _T("Value"), LVCFMT_LEFT, 110, 1 );

	m_htiTTLLive = m_wndExplorer.InsertItem( TVIF_TEXT | TVIF_PARAM, _T("TTLLive"), 0,0,0,0, ITEM_GLOBAL, TVI_ROOT, TVI_LAST );
	m_wndExplorer.SelectItem( m_htiTTLLive );

	SetMsgHandled( FALSE );
	return 0;
}

inline void CViewConfig::ResetTree( CTTLLivePtr& oTTLLive )
{
	HRESULT hr = S_OK;

	m_wndExplorer.DeleteAllItems();
	m_mapChannels.RemoveAll();
	m_mapEncoders.RemoveAll();

	m_htiTTLLive = m_wndExplorer.InsertItem( TVIF_TEXT | TVIF_PARAM, _T("TTLLive"), 0,0,0,0, ITEM_GLOBAL, TVI_ROOT, TVI_LAST );

	long hEncoder = -1;
	for ( hr = oTTLLive->GetFirstEncoderHND( &hEncoder )
		; SUCCEEDED(hr) && 0 <= hEncoder
		; hr = oTTLLive->GetNextEncoderHND( &hEncoder )
		)
	{
		CString sz; sz.Format( _T("Encoder %d"), hEncoder );
		long lParam = ITEM_ENCODER | ( hEncoder<<2 );
		HTREEITEM htiE = m_wndExplorer.InsertItem( TVIF_TEXT | TVIF_PARAM, sz, 0,0,0,0, lParam, m_htiTTLLive, TVI_LAST );
		m_mapEncoders.Add( hEncoder, htiE );
	}

	long hChannel = -1;
	for ( hr = oTTLLive->GetFirstChannelHND( &hChannel )
		; SUCCEEDED(hr) && 0 <= hChannel
		; hr = oTTLLive->GetNextChannelHND( &hChannel )
		)
	{
		long hEncoder = 0;
		hr = oTTLLive->get_ChannelEncoderHND( hChannel, &hEncoder );
		HTREEITEM htiE = m_mapEncoders.Lookup( hEncoder );

		CString sz; sz.Format( _T("Channel %d"), hChannel );
		long lParam = ITEM_CHANNEL | ( hChannel<<2 );
		HTREEITEM htiC = m_wndExplorer.InsertItem( TVIF_TEXT | TVIF_PARAM, sz, 0,0,0,0, lParam, htiE, TVI_LAST );
		m_mapChannels.Add( hChannel, htiC );
	}

	m_wndExplorer.Expand( m_htiTTLLive );
	m_wndExplorer.SelectItem( m_htiTTLLive );
}

inline void CViewConfig::ResetList( LPNMTREEVIEW lpTvHdr, CTTLLivePtr& oTTLLive, double nConnectTime )
{
	HRESULT hr = S_OK;

	m_wndProperties.DeleteAllItems();

	switch ( ITEM_TYPE_MASK & lpTvHdr->itemNew.lParam )
	{
	case ITEM_GLOBAL :
		{
		AppendProp( hr, _T("(connect time)"), nConnectTime );

		TTL_VERSION vVersion;
		hr = oTTLLive->get_Version( vVersion );
		AppendProp( hr, _T("Version"), vVersion );

		long nEncoderCount = 0;
		hr = oTTLLive->get_EncoderCount( &nEncoderCount );
		AppendProp( hr, _T("Encoder Count"), nEncoderCount );

		long nChannelCount = 0;
		hr = oTTLLive->get_ChannelCount( &nChannelCount );
		AppendProp( hr, _T("Channel Count"), nChannelCount );

		long nImpCheckType = 0;
		hr = oTTLLive->get_ImpCheckType( &nImpCheckType );
		AppendProp( hr, _T("ImpCheck Type"), (TTLAPI_IMPCHECK_CALCULATION_TYPES)( nImpCheckType ) );

	#ifdef USE_TTLLIVE_2

		long nGlobalTickRate = 0;
		hr = oTTLLive->get_GlobalTickRate( &nGlobalTickRate );
		AppendProp( hr, _T("Global Tick Rate"), nGlobalTickRate );

		long nGapChannelFill = 0;
		hr = oTTLLive->get_GapChannelFill( &nGapChannelFill );
		AppendProp( hr, _T("Gap Chan. Fill"), nGapChannelFill );

		long nGapChannelForceFill = 0;
		hr = oTTLLive->get_GapChannelForceFill( &nGapChannelForceFill );
		AppendProp( hr, _T("Gap Chan. Force Fill"), nGapChannelForceFill );

		float nGapFillValue = 0.0;
		hr = oTTLLive->get_GapFillValue( &nGapFillValue );
		AppendProp( hr, _T("Fill Value"), nGapFillValue );

		long nShortReadMask = 0;
		hr = oTTLLive->get_ShortReadMask( &nShortReadMask );
		AppendProp( hr, _T("Short Read Mask"), nShortReadMask );

		long nForcedProtocol = 0;
		hr = oTTLLive->get_ForcedProtocol( &nForcedProtocol );
		AppendProp( hr, _T("Forced Protocol"), (TTLAPI_PROTOCOL_TYPES)( nForcedProtocol ) );

		TTLAPI_SYNC_STATES nSyncState = TTLAPI_SST_NOT_STARTED;
		hr = oTTLLive->get_SyncState( &nSyncState );
		AppendProp( hr, _T("Sync State"), nSyncState );

		long nSyncSendTrigger = 0;
		hr = oTTLLive->get_SyncSendTrigger( &nSyncSendTrigger );
		AppendProp( hr, _T("Sync Send Trigger"), nSyncSendTrigger );

		long nSyncWaitTrigger = 0;
		hr = oTTLLive->get_SyncWaitTrigger( &nSyncWaitTrigger );
		AppendProp( hr, _T("Sync Wait Trigger"), nSyncWaitTrigger );

		long nSyncSendPeriodic = 0;
		hr = oTTLLive->get_SyncSendPeriodic( &nSyncSendPeriodic );
		AppendProp( hr, _T("Sync Send Periodic"), nSyncSendPeriodic );

		long nSyncProcessPeriodic = 0;
		hr = oTTLLive->get_SyncProcessPeriodic( &nSyncProcessPeriodic );
		AppendProp( hr, _T("Sync Process Periodic"), nSyncProcessPeriodic );

		long nSyncSendPeriodicInterval = 0;
		hr = oTTLLive->get_SyncSendPeriodicInterval( &nSyncSendPeriodicInterval );
		AppendProp( hr, _T("Sync Interval"), nSyncSendPeriodicInterval );

		long nSyncTimeout = 0;
		hr = oTTLLive->get_SyncTimeout( &nSyncTimeout );
		AppendProp( hr, _T("Sync Timeout"), nSyncTimeout );

		long nMinTicksAvailable = 0;
		hr = oTTLLive->get_MinTicksAvailable( &nMinTicksAvailable );
		AppendProp( hr, _T("Min Ticks Available"), nMinTicksAvailable );

		long hMasterEncoderHND = 0;
		hr = oTTLLive->get_MasterEncoderHND( &hMasterEncoderHND );
		AppendProp( hr, _T("Master Encoder"), hMasterEncoderHND );

	#endif // USE_TTLLIVE_2

		long hFirstEncoderHND = 0;
		hr = oTTLLive->GetFirstEncoderHND( &hFirstEncoderHND );
		AppendProp( hr, _T("First Encoder"), hFirstEncoderHND );

		long hFirstChannelHND = 0;
		hr = oTTLLive->GetFirstChannelHND( &hFirstChannelHND );
		AppendProp( hr, _T("First Channel"), hFirstChannelHND );
		}
		break;

	case ITEM_ENCODER :
		{
		long hEncoder = ( lpTvHdr->itemNew.lParam >> 2 );

		CComBSTR bstrSerialNumber;
		hr = oTTLLive->get_SerialNumber( hEncoder, &bstrSerialNumber );
		AppendProp( hr, _T("Serial Number"), bstrSerialNumber );

		long nEncoderModelType = 0;
		hr = oTTLLive->get_EncoderModelType( hEncoder, &nEncoderModelType );
		CComBSTR bstrEncoderModelName;
		hr = oTTLLive->get_EncoderModelName( hEncoder, &bstrEncoderModelName );
		AppendProp( hr, _T("Model"), nEncoderModelType, bstrEncoderModelName );

		long nProtocolType = 0;
		hr = oTTLLive->get_ProtocolType( hEncoder, &nProtocolType );
		CComBSTR bstrProtocolName;
		hr = oTTLLive->get_ProtocolName( hEncoder, &bstrProtocolName );
		AppendProp( hr, _T("Protocol"), nProtocolType, bstrProtocolName );

		long nConnectionType = 0;
		hr = oTTLLive->get_ConnectionType( hEncoder, &nConnectionType );
		AppendProp( hr, _T("Connection Type"), (TTLAPI_CONNECTION_TYPES)( nConnectionType ) );

		CComBSTR bstrConnectionName;
		hr = oTTLLive->get_ConnectionName( hEncoder, &bstrConnectionName );
		AppendProp( hr, _T("Connection Name"), bstrConnectionName );

		TTL_VERSION vFirmwareVersion;
		hr = oTTLLive->get_FirmwareVersion( hEncoder, vFirmwareVersion );
		AppendProp( hr, _T("Firmware Version"), vFirmwareVersion );

		TTL_VERSION vHardwareVersion;
		hr = oTTLLive->get_HardwareVersion( hEncoder, vHardwareVersion );
		AppendProp( hr, _T("Hardware Version"), vHardwareVersion );

	#ifdef USE_TTLLIVE_2

		CComBSTR bstrTTUSB_SN;
		hr = oTTLLive->get_TTUSB_SN( hEncoder, &bstrTTUSB_SN );
		AppendProp( hr, _T("TTUSB SN"), bstrTTUSB_SN );

		TTL_VERSION vTTUSB_FirmwareVersion;
		hr = oTTLLive->get_TTUSB_FirmwareVersion( hEncoder, vTTUSB_FirmwareVersion );
		AppendProp( hr, _T("TTUSB Firmware Version"), vTTUSB_FirmwareVersion );

	#endif // USE_TTLLIVE_2

		long nEncoderState = 0;
		hr = oTTLLive->get_EncoderState( hEncoder, &nEncoderState );
		AppendProp( hr, _T("State"), (TTLAPI_ENCODER_STATES)( nEncoderState ) );

		float nTickRate = 0.0;
		hr = oTTLLive->get_TickRate( hEncoder, &nTickRate );
		AppendProp( hr, _T("Tick Rate"), nTickRate );

		DATE dEncoderTime = 0;
		hr = oTTLLive->get_EncoderTime( hEncoder, &dEncoderTime );
		AppendProp( hr, _T("Time"), dEncoderTime, false );

		DATE dEncoderTimeAdjustment = 0;
		/***/
		double s0 = TTL_SECONDS();
		hr = oTTLLive->get_EncoderTimeAdjustment( hEncoder, &dEncoderTimeAdjustment );
		double s1 = TTL_SECONDS();
		/***/
		AppendProp( hr, _T("Time Adjustment"), dEncoderTimeAdjustment, false );
		AppendProp( hr, _T("(call time)"), s1 - s0 );

		DATE dCheckTime = dEncoderTime + dEncoderTimeAdjustment;
		AppendProp( hr, _T("(check time)"), dCheckTime, false );

		float nBatteryLevelVolt = 0.0;
		hr = oTTLLive->get_BatteryLevelVolt( hEncoder, &nBatteryLevelVolt );
		AppendProp( hr, _T("Batt. Level [V]"), nBatteryLevelVolt );

		float nBatteryLevelPct = 0.0;
		hr = oTTLLive->get_BatteryLevelPct( hEncoder, &nBatteryLevelPct );
		AppendProp( hr, _T("Batt. Level [%]"), nBatteryLevelPct );

		float nBatteryTimeLeft = 0.0;
		hr = oTTLLive->get_BatteryTimeLeft( hEncoder, &nBatteryTimeLeft );
		AppendProp( hr, _T("Batt. Time [h]"), nBatteryTimeLeft );

		long nEncoderChannelCount = 0;
		hr = oTTLLive->get_EncoderChannelCount( hEncoder, &nEncoderChannelCount );
		AppendProp( hr, _T("Channel Count"), nEncoderChannelCount );

		long nSensorStatus = 0;
		hr = oTTLLive->get_SensorStatus( hEncoder, &nSensorStatus );
		AppendProp( hr, _T("Sensor Status"), (TTL_SENSOR_STATUS)( nSensorStatus ) );

		long nEventInputState = 0;
		hr = oTTLLive->get_EventInputState( hEncoder, &nEventInputState );
		AppendProp( hr, _T("Event Input State"), nEventInputState );

		long nSwitchState = 0;
		hr = oTTLLive->get_SwitchState( hEncoder, &nSwitchState );
		AppendProp( hr, _T("Switch"), nSwitchState );

		long hFirstChannelHND = 0;
		hr = oTTLLive->get_FirstChannelHND( hEncoder, &hFirstChannelHND );
		AppendProp( hr, _T("First Channel"), hFirstChannelHND );

	#ifdef USE_TTLLIVE_2

		CComPtr<ITTLEncoder> pEncoder;
		hr = oTTLLive->get_Encoder( hEncoder, &pEncoder );
		if ( ! pEncoder ) break;

		/* Duplicates of above:
		get_State( enum TTLAPI_ENCODER_STATES * pVal )
		get_ConnectionName( BSTR * pVal )
		get_ConnectionType( enum TTLAPI_CONNECTION_TYPES * pVal )
		get_SerialNumber( BSTR * pVal )
		get_ModelName( BSTR * pVal )
		get_ModelType( enum TTLAPI_ENCODER_MODEL_TYPES * pVal )
		get_ProtocolName( BSTR * pVal )
		get_ProtocolType( enum TTLAPI_PROTOCOL_TYPES * pVal )
		get_FirmwareVersion( long * pVal )
		get_HardwareVersion( long * pVal )
		get_TTUSB_SN( BSTR * pVal )
		get_TTUSB_FirmwareVersion( long * pVal )
		get_TickRate( float * pVal )
		get_Time( DATE * pVal )
		get_TimeAdjustment( DATE * pVal )
		get_BatteryLevelVolt( float * pVal )
		get_BatteryLevelPct( float * pVal )
		get_BatteryTimeLeft( float * pVal )
		get_PhysicalChannelCount( long * pVal )
		get_SensorStatus( long * pVal )
		get_SwitchState( long * pVal )
		get_EventInputState( long * pVal )
		get_FirstChannelHND( long * pVal )
		*/

		float nTicksAhead = 0;
		hr = pEncoder->get_TicksAhead( &nTicksAhead );
		AppendProp( hr, _T("Ticks Ahead"), nTicksAhead );

		long nTimeoutPeriod = 0;
		hr = pEncoder->get_TimeoutPeriod( &nTimeoutPeriod );
		AppendProp( hr, _T("Timeout Period"), nTimeoutPeriod );

		long nEncoderID = 0;
		hr = pEncoder->get_ID( &nEncoderID );
		AppendProp( hr, _T("ID"), nEncoderID );

		float nStreamQuality = 0;
		hr = pEncoder->get_StreamQuality( &nStreamQuality );
		AppendProp( hr, _T("Stream Quality"), nStreamQuality );

	#endif // USE_TTLLIVE_2
		}
		break;

	case ITEM_CHANNEL :
		{
		long hChannel = ( lpTvHdr->itemNew.lParam >> 2 );

		long nSensorState = 0;
		hr = oTTLLive->get_SensorState( hChannel, &nSensorState );
		AppendProp( hr, _T("State"), (TTLAPI_SENSOR_STATES)( nSensorState ) );

		long nPhysicalIndex = 0;
		hr = oTTLLive->get_ChannelPhysicalIndex( hChannel, &nPhysicalIndex );
		AppendProp( hr, _T("Physical Index"), (TTLAPI_CHANNELS)( nPhysicalIndex ) );

		long nSensorConnected = 0;
		hr = oTTLLive->get_SensorConnected( hChannel, &nSensorConnected );
		AppendProp( hr, _T("Connected"), TRUE == nSensorConnected );

		long nSensorID = 0;
		hr = oTTLLive->get_SensorID( hChannel, &nSensorID );
		AppendProp( hr, _T("ID"), (TTLAPI_SENSOR_TYPES)( nSensorID ) );

		long nSensorType = 0;
		hr = oTTLLive->get_SensorType( hChannel, &nSensorType );
		AppendProp( hr, _T("Type"), (TTLAPI_SENSOR_TYPES)( nSensorType ) );

		float nNominalSampleRate = 0;
		hr = oTTLLive->get_NominalSampleRate( hChannel, &nNominalSampleRate );
		AppendProp( hr, _T("Nominal Rate"), nNominalSampleRate );

		long nChannelActive = 0;
		hr = oTTLLive->get_ChannelActive( hChannel, &nChannelActive );
		AppendProp( hr, _T("Active"), 0 != nChannelActive );

		long nNotification = 0;
		hr = oTTLLive->get_Notification( hChannel, &nNotification );
		AppendProp( hr, _T("Notification"), nNotification );

		long nUnitType = 0;
		hr = oTTLLive->get_UnitType( hChannel, &nUnitType );
		AppendProp( hr, _T("Unit Type"), (TTLAPI_UNIT_TYPES)( nUnitType ) );

		float nChannelScale = 0;
		hr = oTTLLive->get_ChannelScale( hChannel, &nChannelScale );
		AppendProp( hr, _T("Scale"), nChannelScale );

		float nChannelOffset = 0;
		hr = oTTLLive->get_ChannelOffset( hChannel, &nChannelOffset );
		AppendProp( hr, _T("Offset"), nChannelOffset );

		float nAutoZeroSeconds = 0;
		hr = oTTLLive->get_AutoZero( hChannel, &nAutoZeroSeconds );
		AppendProp( hr, _T("DC Filter [s]"), nAutoZeroSeconds );

		long nSamplesAvailable = 0;
		hr = oTTLLive->get_SamplesAvailable( hChannel, &nSamplesAvailable );
		AppendProp( hr, _T("Samples Available"), nSamplesAvailable );

		float nImpCheckAge = 0;
		hr = oTTLLive->get_ImpCheckAge( hChannel, &nImpCheckAge );
		AppendProp( hr, _T("ImpCheck Age"), nImpCheckAge );

		float aImpCheckResults[ TTLAPI_IMPCHECK_MAX_RESULT ] = {0,};
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_ELECTRODE_PLUS  , aImpCheckResults+0 );
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_ELECTRODE_MINUS , aImpCheckResults+1 );
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_ELECTRODE_REF   , aImpCheckResults+2 );
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_PAIRS_PLUS_REF  , aImpCheckResults+3 );
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_PAIRS_MINUS_REF , aImpCheckResults+4 );
		hr = oTTLLive->get_ImpCheckResults( hChannel, TTLAPI_IMPCHECK_PAIRS_PLUS_MINUS, aImpCheckResults+5 );
		CString sz;
		sz.Format( _T("ep:%g, em:%g, er:%g"), aImpCheckResults[0], aImpCheckResults[1], aImpCheckResults[2] );
		AppendProp( hr, _T("ImpCheck Elec."), sz );
		sz.Format( _T("pr:%g, mr:%g, pm:%g"), aImpCheckResults[3], aImpCheckResults[4], aImpCheckResults[5] );
		AppendProp( hr, _T("ImpCheck Pairs"), sz );

	#ifdef USE_TTLLIVE_2

		CComPtr<ITTLChannel> pChannel;
		hr = oTTLLive->get_Channel( hChannel, &pChannel );
		if ( ! pChannel ) break;

		/* Duplicates of above:
		get_SensorState( enum TTLAPI_SENSOR_STATES * pVal )
		get_PhysicalIndex( long * pVal )
		get_SensorConnected( long * pVal )
		get_SensorID( enum TTLAPI_SENSOR_TYPES * pVal )
		get_SensorType( enum TTLAPI_SENSOR_TYPES * pVal )
		get_NominalSampleRate( float * pVal )
		get_Active( long * pVal )
		get_Notification( long * pliNotification )
		get_UnitType( enum TTLAPI_UNIT_TYPES * pVal )
		get_Scale( float * pVal )
		get_Offset( float * pVal )
		get_AutoZero( float * pVal )
		get_SamplesAvailable( long * pVal )
		get_ImpCheckAge( float * pVal )
		get_ImpCheckResults( enum TTLAPI_IMPCHECK_RESULTS liResultIndex, float * pVal )
		get_ShortReadMask( long * pVal )
		get_EventInsertionMask( long * pVal )
		*/

		/* Don't know:
		get_EventHorizon( enum TTLAPI_EVENT_HORIZON_MODES eMode, long * pVal )
		get_EventType( TTLAPI_EVENT_TYPES * pVal )
		get_EventCount( long * pVal )
		get_EventTicks( long * pVal )
		get_EventLength( long * pVal )
		*/

		long nEventInsertionMask = 0;
		hr = pChannel->get_EventInsertionMask( &nEventInsertionMask );
		AppendProp( hr, _T("Insertion Mask"), (TTL_EVENT_INSERTION_MASK)( nEventInsertionMask ) );

		long nShortReadMask = 0;
		hr = pChannel->get_ShortReadMask( &nShortReadMask );
		AppendProp( hr, _T("ShortRead Mask"), (TTL_EVENT_INSERTION_MASK)( nShortReadMask ) );

		long nNextSampleTicks = 0;
		hr = pChannel->get_NextSampleTicks( &nNextSampleTicks );
		AppendProp( hr, _T("Next Sample Ticks"), nNextSampleTicks );

	#endif // USE_TTLLIVE_2
		}
		break;

	default:
		ATLASSERT( false );
		break;
	}
}

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_VIEWCONFIG_H__1BA203D0_2088_44CF_B7D6_330D852512C6__INCLUDED_)
