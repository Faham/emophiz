// MainFrm.cpp : implmentation of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "resource.h"

#include "MainFrm.h"

BOOL CMainFrame::PreTranslateMessage( MSG* pMsg )
{
	return CTTLLiveSink_type::PreTranslateMessage( pMsg )
		|| CFrameWindowImpl_type::PreTranslateMessage( pMsg )
		;
}

BOOL CMainFrame::OnIdle()
{
	UIUpdateToolBar();
	return FALSE;
}

LRESULT CMainFrame::OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ )
{
	if ( FAILED( TtlCreateControl() ) )
	{
		LPTSTR lpBuffer = _T("Unknown Error");
		DWORD bFreeIt = ::FormatMessage( FORMAT_MESSAGE_ALLOCATE_BUFFER
			| FORMAT_MESSAGE_FROM_SYSTEM
			, NULL, TtlGetLastError(), 0, (LPTSTR) &lpBuffer, 0, NULL
			);
		CString sz;
		sz.Format( _T("An error occured trying to get the TTLive interface.\n"
			"The error was:\n"
			"%s")
			, lpBuffer
			);
		MessageBox( sz, _T("Error"), MB_OK | MB_ICONERROR );
		if ( bFreeIt ) ::LocalFree( lpBuffer );
		return -1; // fail OnCreate
	}

	TtlPnPRegister();

	// wizard generated UI:

	HWND hWndCmdBar = m_CmdBar.Create( m_hWnd, rcDefault, NULL, ATL_SIMPLE_CMDBAR_PANE_STYLE );
	m_CmdBar.AttachMenu( GetMenu() );
	m_CmdBar.LoadImages( IDR_MAINFRAME );
	SetMenu( NULL );

	HWND hWndToolBar = CreateSimpleToolBarCtrl( m_hWnd, IDR_MAINFRAME, FALSE, ATL_SIMPLE_TOOLBAR_PANE_STYLE );

	CreateSimpleReBar( ATL_SIMPLE_REBAR_NOBORDER_STYLE );
	AddSimpleReBarBand( hWndCmdBar );
	AddSimpleReBarBand( hWndToolBar, NULL, TRUE );

	CreateSimpleStatusBar();

	UIAddToolBar( hWndToolBar );
	UISetCheck( ID_VIEW_TOOLBAR, 1 );
	UISetCheck( ID_VIEW_STATUS_BAR, 1 );

	CMessageLoop* pLoop = _Module.GetMessageLoop();
	ATLASSERT( pLoop != NULL );
	pLoop->AddMessageFilter( this );
	pLoop->AddIdleHandler( this );

	// create remainder of UI:

	m_hWndClient =
	m_wndVSplitter.Create( *this, rcDefault, NULL
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		);
	m_wndGraphs.Create( m_wndVSplitter, rcDefault, NULL
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		, WS_EX_CLIENTEDGE
		);
	m_wndConfig.Create( m_wndVSplitter, _T("Explorer")
		, WS_CHILD | WS_VISIBLE | WS_CLIPCHILDREN | WS_CLIPSIBLINGS
		);
	m_wndVSplitter.SetSplitterPanes( m_wndConfig, m_wndGraphs );
	m_wndVSplitter.m_nProportionalPos = m_wndVSplitter.m_nPropMax / 3;

	UISetBlockAccelerators( true );
	UISetCheck( ID_OPTIONS_KEEPEXISTING  , m_bKeepExisting  );
	UISetCheck( ID_OPTIONS_AUTODETECT    , m_bAutoDetect    );
	UISetCheck( ID_OPTIONS_FORCEPROTOCOL , m_bForceProtocol );
	UISetCheck( ID_OPTIONS_FILLGAPS      , m_bFillGaps      );
	UISetCheck( ID_OPTIONS_AUTOSETUP     , m_bAutoSetup     );
	UISetCheck( ID_OPTIONS_FORCEALL      , m_bForceAll      );
	UISetCheck( ID_OPTIONS_DISACTIVATE   , m_bDisactivate   );
	UISetCheck( ID_OPTIONS_SETSENSORTYPE , m_bSetSensorType );
	UISetCheck( ID_OPTIONS_SHORTREADS    , m_bShortReads    );
	UISetCheck( ID_OPTIONS_SYNCHRONIZE   , m_bSynchronize   );
	UISetCheck( ID_OPTIONS_SYNCH_INTERNAL, m_bSynchInternal );
	UISetCheck( ID_OPTIONS_SYNCH_PERIODIC, m_bSynchPeriodic );
	UISetCheck( ID_DAC_RECORD            , m_bRecord );
	UISetCheck( m_idUnitType, true );
	UISetCheck( m_idFilter, true );

#ifdef USE_TTLLIVE_2
	// all options available
#else
	UIEnable( ID_OPTIONS_FILLGAPS      , false );
	UIEnable( ID_OPTIONS_SHORTREADS    , false );
	UIEnable( ID_OPTIONS_SYNCHRONIZE   , false );
	UIEnable( ID_OPTIONS_SYNCH_INTERNAL, false );
	UIEnable( ID_OPTIONS_SYNCH_PERIODIC, false );
#endif

	UIUpdate();

	SetMsgHandled( false );
	return 0;
}

void CMainFrame::OnClose( void )
{
	SetMsgHandled( false );
	OnDacStop( 0, ID_DAC_STOP, NULL ); // just in case
	TtlPnPUnregister();
	TtlDestroyControl();
}

void CMainFrame::OnFileExit( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	PostMessage( WM_CLOSE );
}

void CMainFrame::OnViewToolBar( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	static BOOL bVisible = TRUE; // initially visible
	bVisible = !bVisible;
	CReBarCtrl rebar = m_hWndToolBar;
	int nBandIndex = rebar.IdToIndex( ATL_IDW_BAND_FIRST + 1 ); // toolbar is 2nd added band
	rebar.ShowBand( nBandIndex, bVisible );
	UISetCheck( ID_VIEW_TOOLBAR, bVisible );
	UpdateLayout();
}

void CMainFrame::OnViewStatusBar( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	BOOL bVisible = !::IsWindowVisible( m_hWndStatusBar );
	::ShowWindow( m_hWndStatusBar, bVisible ? SW_SHOWNOACTIVATE : SW_HIDE );
	UISetCheck( ID_VIEW_STATUS_BAR, bVisible );
	UpdateLayout();
}

void CMainFrame::OnAppAbout( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	CSimpleDialog<IDD_ABOUTBOX, true> dlg;
	dlg.DoModal();
}

void CMainFrame::OnOptions( UINT /*uNotifyCode*/, int nID, CWindow /*wndCtl*/ )
{
	bool *pbOption = NULL;

	switch ( nID )
	{
	case ID_OPTIONS_KEEPEXISTING  : pbOption = &m_bKeepExisting;  break;
	case ID_OPTIONS_AUTODETECT    : pbOption = &m_bAutoDetect;    break;
	case ID_OPTIONS_FORCEPROTOCOL : pbOption = &m_bForceProtocol; break;
	case ID_OPTIONS_FILLGAPS      : pbOption = &m_bFillGaps;      break;
	case ID_OPTIONS_AUTOSETUP     : pbOption = &m_bAutoSetup;     break;
	case ID_OPTIONS_FORCEALL      : pbOption = &m_bForceAll;      break;
	case ID_OPTIONS_DISACTIVATE   : pbOption = &m_bDisactivate;   break;
	case ID_OPTIONS_SETSENSORTYPE : pbOption = &m_bSetSensorType; break;
	case ID_OPTIONS_SHORTREADS    : pbOption = &m_bShortReads;    break;
	case ID_OPTIONS_SYNCHRONIZE   : pbOption = &m_bSynchronize;   break;
	case ID_OPTIONS_SYNCH_INTERNAL: pbOption = &m_bSynchInternal; break;
	case ID_OPTIONS_SYNCH_PERIODIC: pbOption = &m_bSynchPeriodic; break;
	case ID_DAC_RECORD            : pbOption = &m_bRecord;        break;
	default : ATLASSERT( false ); break;
	}
	if ( NULL != pbOption )
	{
		*pbOption ^= true;
		UISetCheck( nID, *pbOption );
	}
}

void CMainFrame::OnOptionsUnitType( UINT /*uNotifyCode*/, int nID, CWindow /*wndCtl*/ )
{
	if ( nID == m_idUnitType ) return;

	UISetCheck( m_idUnitType, false );

	switch ( nID )
	{
	case ID_OPTIONS_UNITTYPE_NONE : m_nUnitType = TTLAPI_UT_DEFAULT;  m_nScale = 0.004; break;
	case ID_OPTIONS_UNITTYPE_SENV : m_nUnitType = TTLAPI_UT_SENSVOLT; m_nScale = 0.004; break;
	case ID_OPTIONS_UNITTYPE_ENCV : m_nUnitType = TTLAPI_UT_ENCVOLT;  m_nScale = 1.7;   break;
	case ID_OPTIONS_UNITTYPE_ENCC : m_nUnitType = TTLAPI_UT_COUNT;    m_nScale = 8192;  break;
	default : ATLASSERT( false ); break;
	}

	UISetCheck( m_idUnitType = nID, true );
}

void CMainFrame::OnOptionsFilter( UINT /*uNotifyCode*/, int nID, CWindow /*wndCtl*/ )
{
	if ( nID == m_idFilter ) return;

	static CSimpleFilter::BIQUAD s_bqNotch50 =
		// Command line: mkfilter -t -x -Bu -Bs -a 0.02392578125 0.02490234375 -o 4
		{	1.00804920800941
		,	4
		,	{ { -1.97652443736188, 1, -1.97053514909160, 0.994304341100123 }
			, { -1.97652443736188, 1, -1.97133532863430, 0.994389978361416 }
			, { -1.97652443736188, 1, -1.97328316061101, 0.997611701618417 }
			, { -1.97652443736188, 1, -1.97509838616325, 0.997697598090677 }
			,
		}	};
	static CSimpleFilter::BIQUAD s_bqNotch60 =
		// Command line: mkfilter -t -x -Bu -Bs -a 0.02880859375 0.02978515625 -o 4
		{	1.00804921394798
		,	4
		,	{ { -1.96622022826924, 1, -1.96019662768289, 0.994311605562208 }
			, { -1.96622022826924, 1, -1.96112370816225, 0.994382713326738 }
			, { -1.96622022826924, 1, -1.96282725907159, 0.997618984985037 }
			, { -1.96622022826924, 1, -1.96497020168460, 0.997690314150123 }
			,
		}	};
	static CSimpleFilter::BIQUAD s_bqWide =
		// Command line: mkfilter -t -x -Bu -Hp -a 0.009765625 -o 4
		{	1.08348688675524
		,	2
		,	{ { -2, 1, -1.88920703055163, 0.892769008131026 }
			, { -2, 1, -1.95046575793010, 0.954143234875077 }
			,
		}	};
	static CSimpleFilter::BIQUAD s_bqNarrow =
		// Command line: mkfilter -t -x -Bu -Bp -a 0.048828125 0.09765625 -o 8
		{	6749469.95368123
		,	8
		,	{ { 0, -1, -1.49494651917532, 0.736659746906706 }
			, { 0, -1, -1.50723780541482, 0.806429284180075 }
			, { 0, -1, -1.53533899295441, 0.721216896212596 }
			, { 0, -1, -1.57820643438517, 0.925780011424751 }
			, { 0, -1, -1.61143983493698, 0.752850089356582 }
			, { 0, -1, -1.69989815966168, 0.812562428167180 }
			, { 0, -1, -1.78618757655956, 0.883702117989885 }
			, { 0, -1, -1.86739797816512, 0.959968438542827 }
			,
		}	};

	UISetCheck( m_idFilter, false );

	switch ( nID )
	{
	case ID_OPTIONS_FILTER_NONE    : m_oFilter.ClearFilter(); break;
	case ID_OPTIONS_FILTER_FILTER1 : m_oFilter.SetFilter( s_bqNotch50 ); break;
	case ID_OPTIONS_FILTER_FILTER2 : m_oFilter.SetFilter( s_bqNotch60 ); break;
	case ID_OPTIONS_FILTER_FILTER3 : m_oFilter.SetFilter( s_bqWide    ); break;
	case ID_OPTIONS_FILTER_FILTER4 : m_oFilter.SetFilter( s_bqNarrow  ); break;
	default : ATLASSERT( false ); break;
	}

	UISetCheck( m_idFilter = nID, true );
}

/////////////////////////////////////////////////////////////////////////////
//

enum
{
	TICK_RATE = 2048
,	FRAME_SIZE = 128
,	MAX_FRAMES = 8
,	MAX_DATA_COUNT = ( MAX_FRAMES * FRAME_SIZE )
,	DESIRED_FRAMES = 1
	,	FRAMES_LO = DESIRED_FRAMES/2
	,	FRAMES_HI = DESIRED_FRAMES*2
,	DESIRED_DATA_COUNT = ( DESIRED_FRAMES * FRAME_SIZE )
,	DESIRED_FREQUENCY = ( TICK_RATE / DESIRED_DATA_COUNT )
,	DESIRED_PERIOD = ( 1000 / DESIRED_FREQUENCY )
,	GAP_FILL_VALUE = 0
,	SYNC_TIMEOUT = 300 /*ms*/
,	SYNC_PERIODIC_INTERVAL = 5000 /*ms*/
};

void CMainFrame::OnDacStart( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	int cActiveChannels = m_aActiveChannels.GetSize();
	ATLASSERT( 0 < cActiveChannels );
	if ( cActiveChannels <= 0 ) return;

	if ( ! SimpleTimerStart( DESIRED_PERIOD ) )
	{
		MessageBox( _T("No timer available"), _T("Error"), MB_OK | MB_ICONERROR );
		return;
	}

	m_wndGraphs.OnStart( TICK_RATE, cActiveChannels, m_nScale );

	m_oRecorder.OnStart( m_bRecord, cActiveChannels );

	m_bRunning = true;

	UIUpdate();

#ifdef USE_TTLLIVE_2
	{
		TTL_TRACE_BLOCK( _T("synch setup") );
		// gap filling ON
		if ( m_bFillGaps )
		{
			m_hr = m_oTTLLive->put_GapFillValue( GAP_FILL_VALUE );
			m_hr = m_oTTLLive->put_GapChannelFill( TRUE );
			m_hr = m_oTTLLive->put_GapEncoderFill( TRUE );
			m_hr = m_oTTLLive->put_GapEncoderOfflineFill( TRUE );
		}
		// synchronisation ON
		if ( m_bSynchronize )
		{
			m_hr = m_oTTLLive->put_SyncTimeout( SYNC_TIMEOUT );
			m_hr = m_oTTLLive->put_SyncWaitTrigger( TRUE );
		}
		if ( m_bSynchInternal )
		{
			m_hr = m_oTTLLive->put_SyncSendTrigger( TRUE );
		}
		if ( m_bSynchPeriodic )
		{
			m_hr = m_oTTLLive->put_SyncProcessPeriodic( TRUE );
			m_hr = m_oTTLLive->put_SyncSendPeriodicInterval( SYNC_PERIODIC_INTERVAL );
			m_hr = m_oTTLLive->put_SyncSendPeriodic( TRUE );
		}
	}
#endif

	{
		TTL_TRACE_BLOCK( _T("start channels") );
		m_hr = m_oTTLLive->StartChannels(); // main
	}
}

void CMainFrame::OnDacStop( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	{
		TTL_TRACE_BLOCK( _T("stop channels") );
		m_hr = m_oTTLLive->StopChannels(); // main
	}

#ifdef USE_TTLLIVE_2
	{
		TTL_TRACE_BLOCK( _T("synch teardown") );
		// synchronisation OFF
		m_hr = m_oTTLLive->put_SyncSendPeriodic( FALSE );
		m_hr = m_oTTLLive->put_SyncSendPeriodicInterval( -1 );
		m_hr = m_oTTLLive->put_SyncProcessPeriodic( FALSE );
		m_hr = m_oTTLLive->put_SyncSendTrigger( FALSE );
		m_hr = m_oTTLLive->put_SyncWaitTrigger( FALSE );
		// gap filling OFF
		m_hr = m_oTTLLive->put_GapEncoderOfflineFill( FALSE );
		m_hr = m_oTTLLive->put_GapEncoderFill( FALSE );
		m_hr = m_oTTLLive->put_GapChannelFill( FALSE );
	}
#endif

	SimpleTimerStop();

	m_oRecorder.OnStop();

	m_wndGraphs.OnStop();

	m_bRunning = false;

	UIUpdate();
}

void CMainFrame::OnSimpleTimer( void )
{
	int cActiveChannels = m_aActiveChannels.GetSize();
	ATLASSERT( 0 < cActiveChannels );

	static long s_nTock = 0;
	++s_nTock;

	// check for samples available:
	long nSamplesAvailable = -1;
#ifdef USE_TTLLIVE_2
	m_hr = m_oTTLLive->get_MinTicksAvailable( &nSamplesAvailable );
#else
	for ( int j = 0; j < cActiveChannels; ++j )
	{
		long nSamples = 0;
		m_hr = m_oTTLLive->get_SamplesAvailable( m_aActiveChannels[j], &nSamples );
		if ( 0 <= nSamplesAvailable && nSamplesAvailable <= nSamples ) continue;
		nSamplesAvailable = nSamples;
	}
#endif
	if ( nSamplesAvailable <= 0 ) return;

	// we want data in whole frames:
	long nFrames = nSamplesAvailable / FRAME_SIZE;

	if ( FRAMES_HI*2 < nFrames )
		TTL_TRACE( _T("tock %ld *** %ld frames (%ld samples) far from %d ***\n")
			, s_nTock
			, nFrames, nSamplesAvailable
			, DESIRED_FRAMES
			);

	// try to keep a steady pace, but if we fall behind, catch up:
	if ( nFrames <= FRAMES_LO ) return; // we are too fast
	if ( nFrames <= FRAMES_HI ) nFrames = DESIRED_FRAMES; // steady pace
	if ( MAX_FRAMES < nFrames ) nFrames = MAX_FRAMES;
	long nMaxSamples = nFrames * FRAME_SIZE;

	bool bPartialRead = false;

	m_wndGraphs.OnDataBegin( nMaxSamples );

	m_oRecorder.OnDataBegin( nMaxSamples );

	for ( int i = 0; i < cActiveChannels; ++i )
	{
		data_t s_aBuffer[ MAX_DATA_COUNT ] = {0,};
		long hChannel = m_aActiveChannels[ i ];
		long nSamplesRead = 0;

		while ( nSamplesRead < nMaxSamples )
		{
			long cRead = nMaxSamples - nSamplesRead;
			long bEvent = 0;
			m_hr = m_oTTLLive->ReadChannelData( hChannel
				, s_aBuffer + nSamplesRead
				, &cRead
				, &bEvent
				);
			nSamplesRead += cRead;

		#ifdef USE_TTLLIVE_2
			if ( bEvent )
			{
				CComPtr<ITTLChannel> spChannel;
				m_hr = m_oTTLLive->get_Channel( hChannel, &spChannel );
				ATLASSERT( NULL != spChannel.p );
				TTLAPI_EVENT_TYPES eType = (TTLAPI_EVENT_TYPES)( 0 );
				m_hr = spChannel->get_EventType( &eType );
				long nTicks = 0;
				m_hr = spChannel->get_EventTicks( &nTicks );
				TTL_TRACE( _T("tock %ld *** Channel %d Read %ld (of %ld) Tick %ld EVENT = %s ***\n")
					, s_nTock
					, i, nSamplesRead, nMaxSamples
					, nTicks, TtlToStr( eType )
					);
			}
			else
			if ( 0 == cRead )
				break; // otherwise, possible infinite loop
		#else
			if ( bEvent )
			{
				TTL_TRACE( _T("tock %ld *** Channel %d Read %ld (of %ld) UNEXPECTED RESULT = %ld ***\n")
					, s_nTock
					, i, nSamplesRead, nMaxSamples
					, bEvent
					);
			}
			if ( 0 == cRead )
				break; // otherwise, possible infinite loop
		#endif
		}

		if ( nSamplesRead < nMaxSamples )
		{
			TTL_TRACE( _T("tock %ld *** Channel %d Read %ld (of %ld) PARTIAL READ ***\n")
				, s_nTock
				, i, nSamplesRead, nMaxSamples
				);
			bPartialRead = true;
		}

		if ( ! m_mapForced.Lookup( hChannel ) ) m_oFilter.Filter( i, s_aBuffer, nMaxSamples );

		m_wndGraphs.OnData( i, s_aBuffer, nMaxSamples );

		m_oRecorder.OnData( i, s_aBuffer, nMaxSamples );
	}

	m_oRecorder.OnDataEnd();

	m_wndGraphs.OnDataEnd();

	if ( bPartialRead ) ::Beep( 880, 10 );
}

/////////////////////////////////////////////////////////////////////////////
//

enum
{
	MAX_TEST_CHANNELS = 10
,	POLLING_INTERVAL = ( 2 * MAX_FRAMES * DESIRED_PERIOD )
,	SEARCH_PORTS = 0
		| TTLAPI_OCCMD_TTUSB0
		| TTLAPI_OCCMD_TTUSB1
		| TTLAPI_OCCMD_TTUSB2
		| TTLAPI_OCCMD_TTUSB3
,	PROTOCOL = TTLAPI_PROT_INFINITY_FLEX
#ifdef USE_TTLLIVE_2
,	FORCED_PROTOCOL = TTLAPI_OCCMD_FORCE_INFINITI_FLEX
#endif
,	NOTIFICATIONS = 0
		| TTLAPI_ON_CHANNEL_DATA_ACTIVE
		| TTLAPI_ON_EVENT_SWITCH_ACTIVE
		| TTLAPI_ON_SENSOR_STATE_CHANGE_ACTIVE
		| TTLAPI_ON_ENCODER_STATE_CHANGE_ACTIVE
		| TTLAPI_ON_DATA_OVERFLOW_ACTIVE
		| TTLAPI_ON_DATA_ERROR_ACTIVE
		| TTLAPI_ON_DATA_TIMEOUT_ACTIVE
		| TTLAPI_ON_IMP_CHECK_RESULTS_ACTIVE
#ifdef USE_TTLLIVE_2
		| TTLAPI_ON_SYNC_STATE_CHANGE
		| TTLAPI_ON_EVENT_SWITCH_OFF
		| TTLAPI_ON_DATA_GAP
		| TTLAPI_ON_TICKSAHEAD_UPDATE
		| TTLAPI_ON_EVENT_OUTPUT_FAILURE
,	EVENT_INSERTIONS = 0
	#if 1
		| TTLAPI_ON_CHANNEL_DATA_ACTIVE
		| TTLAPI_ON_EVENT_SWITCH_ACTIVE
		| TTLAPI_ON_SENSOR_STATE_CHANGE_ACTIVE
		| TTLAPI_ON_ENCODER_STATE_CHANGE_ACTIVE
		| TTLAPI_ON_DATA_OVERFLOW_ACTIVE
		| TTLAPI_ON_DATA_ERROR_ACTIVE
		| TTLAPI_ON_DATA_TIMEOUT_ACTIVE
		| TTLAPI_ON_IMP_CHECK_RESULTS_ACTIVE
		| TTLAPI_ON_SYNC_STATE_CHANGE
		| TTLAPI_ON_EVENT_SWITCH_OFF
	#if 0
		| TTLAPI_ON_DATA_GAP
	#endif
		| TTLAPI_ON_TICKSAHEAD_UPDATE
		| TTLAPI_ON_EVENT_OUTPUT_FAILURE
	#endif
#endif
};

void CMainFrame::OnFileClose( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	m_hr = m_oTTLLive->put_NotificationMask( 0 );
	m_hr = m_oTTLLive->CloseConnections();
	m_nConnectTime = 0;
	m_mapForced.RemoveAll();
	m_aActiveChannels.RemoveAll();
	m_wndConfig.ResetTree( m_oTTLLive );
	m_bConnected = false;

	UIUpdate();
}

void CMainFrame::OnFileNew( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	{
		CWaitCursor hourglass;

		m_hr = m_oTTLLive->put_NotificationMask( 0 );

		long nCommand = m_bAutoDetect ? TTLAPI_OCCMD_AUTODETECT : SEARCH_PORTS;
		if ( m_bKeepExisting )
			nCommand |= TTLAPI_OCCMD_KEEPEXISTING;
		else
			m_hr = m_oTTLLive->CloseConnections();

	#ifdef USE_TTLLIVE_2
		m_hr = m_oTTLLive->put_ForcedProtocol( m_bForceProtocol ? PROTOCOL : 0 );
	#elif defined( USE_TTLLIVE_2 ) // another way to do it:
		nCommand |= m_bForceProtocol ? FORCED_PROTOCOL : 0;
	#else
		nCommand |= m_bForceProtocol ? TTLAPI_OCCMD_FORCE_PROT( PROTOCOL ) : 0;
	#endif

		m_hr = m_oTTLLive->put_NotificationMask( NOTIFICATIONS );

		/***/
		double s0 = TTL_SECONDS();
		m_hr = m_oTTLLive->OpenConnections( nCommand, POLLING_INTERVAL, NULL, NULL );
		double s1 = TTL_SECONDS();
		m_nConnectTime = s1 - s0;
		/***/

		if ( m_bAutoSetup )
			m_hr = m_oTTLLive->AutoSetupChannels();
		else
			m_hr = m_oTTLLive->DropChannels();
	}

	m_mapForced.RemoveAll();
	m_aActiveChannels.RemoveAll();

	if ( ! m_bAutoSetup )
	{
		long hEncoder = -1;
		for ( m_hr = m_oTTLLive->GetFirstEncoderHND( &hEncoder )
			; 0 <= hEncoder
			; m_hr = m_oTTLLive->GetNextEncoderHND( &hEncoder )
			)
		{
			long nChannelCount = 0;
			m_hr = m_oTTLLive->get_EncoderChannelCount( hEncoder, &nChannelCount );
			for ( int i = 0; i < MAX_TEST_CHANNELS && i < nChannelCount ; ++i )
			{
				long hChannel = -1;
				m_hr = m_oTTLLive->AddChannel( hEncoder, i, &hChannel );
			}
		}
	}

	long hChannel = -1;
	for ( m_hr = m_oTTLLive->GetFirstChannelHND( &hChannel )
		; 0 <= hChannel
		; m_hr = m_oTTLLive->GetNextChannelHND( &hChannel )
		)
	{
		long nSensorID = 0;
		m_hr = m_oTTLLive->get_SensorID( hChannel, &nSensorID );
		if ( m_bForceAll && nSensorID <= TTLAPI_SENS_TYPE_UNCONNECTED )
			nSensorID = TTLAPI_SENS_TYPE_MYOSCAN;

		if ( m_bSetSensorType )
			m_hr = m_oTTLLive->put_SensorType( hChannel, nSensorID );
		if ( TTLAPI_UT_DEFAULT != m_nUnitType )
			m_hr = m_oTTLLive->put_UnitType( hChannel, m_nUnitType );

		long nSensorConnected = 0;
		m_hr = m_oTTLLive->get_SensorConnected( hChannel, &nSensorConnected );
		if ( FALSE == nSensorConnected || nSensorID <= TTLAPI_SENS_TYPE_UNCONNECTED )
		{
			if ( m_bForceAll )
				m_hr = m_oTTLLive->put_ForceSensor( hChannel, TRUE );
			else
			if ( m_bDisactivate )
				m_hr = m_oTTLLive->put_ChannelActive( hChannel, FALSE );
		}

		long nChannelActive = 0;
		m_hr = m_oTTLLive->get_ChannelActive( hChannel, &nChannelActive );
		if ( TRUE == nChannelActive ) m_aActiveChannels.Add( hChannel );
		m_mapForced.Add( hChannel, m_bForceAll && ( FALSE == nSensorConnected ) );

	#ifdef USE_TTLLIVE_2
		if ( m_bShortReads )
		{
			CComPtr<ITTLChannel> spChannel;
			m_hr = m_oTTLLive->get_Channel( hChannel, &spChannel );
			ATLASSERT( NULL != spChannel.p );
			m_hr = spChannel->put_EventInsertionMask( EVENT_INSERTIONS );
			m_hr = spChannel->put_ShortReadMask( EVENT_INSERTIONS );
		}
	#endif
	}

	m_wndConfig.ResetTree( m_oTTLLive );

	m_bConnected = 0 < m_aActiveChannels.GetSize();

	UIUpdate();
}


void CMainFrame::OnTest1( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	if ( ! m_bTest1 ) return;
	static long bBlockReEntry = FALSE;
	if ( ::InterlockedExchange( &bBlockReEntry, TRUE ) ) return;

	::InterlockedExchange( &bBlockReEntry, FALSE );
}

void CMainFrame::OnTest2( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	if ( ! m_bTest2 ) return;
	static long bBlockReEntry = FALSE;
	if ( ::InterlockedExchange( &bBlockReEntry, TRUE ) ) return;

	::InterlockedExchange( &bBlockReEntry, FALSE );
}

void CMainFrame::OnTest3( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	if ( ! m_bTest3 ) return;
	static long bBlockReEntry = FALSE;
	if ( ::InterlockedExchange( &bBlockReEntry, TRUE ) ) return;

	::InterlockedExchange( &bBlockReEntry, FALSE );
}

void CMainFrame::OnTest4( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ )
{
	if ( ! m_bTest4 ) return;
	static long bBlockReEntry = FALSE; // protect against multiple clicks (we call ::DispatchMessage)
	if ( ::InterlockedExchange( &bBlockReEntry, TRUE ) ) return;

	::InterlockedExchange( &bBlockReEntry, FALSE );
}

LRESULT CMainFrame::OnTvSelChanged( LPNMHDR lpNmHdr )
{
	m_wndConfig.ResetList( (LPNMTREEVIEW)( lpNmHdr ), m_oTTLLive, m_nConnectTime );
	return 0;
}
