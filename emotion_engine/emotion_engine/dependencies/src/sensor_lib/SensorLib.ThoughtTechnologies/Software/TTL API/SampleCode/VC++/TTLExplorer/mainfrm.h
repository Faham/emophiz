// MainFrm.h : interface of the CMainFrame class
//
/////////////////////////////////////////////////////////////////////////////

#if !defined(AFX_MAINFRM_H__A19A3886_8A46_4E72_900D_EC9D3B9735F8__INCLUDED_)
#define AFX_MAINFRM_H__A19A3886_8A46_4E72_900D_EC9D3B9735F8__INCLUDED_

#if _MSC_VER >= 1000
#pragma once
#endif // _MSC_VER >= 1000

#include ".\TTLLive.h"

#include "viewgraph.h"
#include "viewconfig.h"
#include "SimpleFilter.h"
#include "SimpleRecorder.h"
#include "SimpleTimer.h"

class CMainFrame
	: public CFrameWindowImpl<CMainFrame>
	, public CUpdateUI<CMainFrame>
	, public CMessageFilter
	, public CIdleHandler
	, public CTTLLive<CMainFrame>
#if 0
	, public CTTLLiveMessageSink<CMainFrame>
#else
	, public CTTLLiveDispatchSink<CMainFrame>
#endif
	, public CTTLLivePnPEventsSink<CMainFrame>
	, public CSimpleTimer<CMainFrame>
{
public:

	typedef CFrameWindowImpl<CMainFrame> CFrameWindowImpl_type;
	typedef CUpdateUI<CMainFrame> CUpdateUI_type;

	DECLARE_FRAME_WND_CLASS( _T("WtlSample:TTLExplorer:MainFrame"), IDR_MAINFRAME )

	CMainFrame()
		: m_nConnectTime( 0 )
		, m_bKeepExisting( false )
		, m_bAutoDetect( false )
		, m_bForceProtocol( true )
		, m_bAutoSetup( false )
		, m_bForceAll( false )
		, m_bDisactivate( true )
		, m_bSetSensorType( true )
	#ifdef USE_TTLLIVE_2
		, m_bFillGaps( true )
		, m_bShortReads( true )
	#else
		, m_bFillGaps( false )
		, m_bShortReads( false )
	#endif
		, m_bSynchronize( false )
		, m_bSynchInternal( false )
		, m_bSynchPeriodic( false )
		, m_bRecord( false )
		, m_idUnitType( ID_OPTIONS_UNITTYPE_ENCC )
		, m_nUnitType( TTLAPI_UT_COUNT )
		, m_nScale( 8192 )
		, m_idFilter( ID_OPTIONS_FILTER_NONE )
		, m_bConnected( false )
		, m_bRunning( false )
		, m_bTest1( false )
		, m_bTest2( false )
		, m_bTest3( false )
		, m_bTest4( false )
	{
	}

	BEGIN_UPDATE_UI_MAP( CMainFrame )
		UPDATE_ELEMENT( ID_VIEW_TOOLBAR          , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_VIEW_STATUS_BAR       , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_KEEPEXISTING  , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_AUTODETECT    , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FORCEPROTOCOL , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILLGAPS      , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_AUTOSETUP     , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FORCEALL      , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_DISACTIVATE   , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_SETSENSORTYPE , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_SHORTREADS    , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_SYNCHRONIZE   , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_SYNCH_INTERNAL, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_SYNCH_PERIODIC, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_UNITTYPE_NONE , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_UNITTYPE_SENV , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_UNITTYPE_ENCV , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_UNITTYPE_ENCC , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILTER_NONE   , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILTER_FILTER1, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILTER_FILTER2, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILTER_FILTER3, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_OPTIONS_FILTER_FILTER4, UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_FILE_CLOSE            , UPDUI_MENUPOPUP )
		UPDATE_ELEMENT( ID_FILE_NEW              , UPDUI_MENUPOPUP | UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_DAC_START             , UPDUI_MENUPOPUP | UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_DAC_STOP              , UPDUI_MENUPOPUP | UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_DAC_RECORD            , UPDUI_MENUPOPUP | UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_TEST1                 , UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_TEST2                 , UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_TEST3                 , UPDUI_TOOLBAR )
		UPDATE_ELEMENT( ID_TEST4                 , UPDUI_TOOLBAR )
	END_UPDATE_UI_MAP()

	void UIUpdate( void )
	{
		UIEnable( ID_FILE_CLOSE, ! m_bRunning );
		UIEnable( ID_FILE_NEW, ! m_bRunning );
		UIEnable( ID_DAC_START, m_bConnected && ! m_bRunning );
		UIEnable( ID_DAC_STOP, m_bRunning );
		UIEnable( ID_DAC_RECORD, m_bConnected && ! m_bRunning );
		UIEnable( ID_TEST1, m_bTest1 );
		UIEnable( ID_TEST2, m_bTest2 );
		UIEnable( ID_TEST3, m_bTest3 );
		UIEnable( ID_TEST4, m_bTest4 );
	}

protected:

	typedef TTL_API_DATA_T data_t;

	CCommandBarCtrl m_CmdBar;

	CSplitterWindow m_wndVSplitter;
	CViewGraph m_wndGraphs;
	CViewConfig m_wndConfig;

	CSimpleValArray<long> m_aActiveChannels;
	CSimpleMap<long,bool> m_mapForced;
	double m_nConnectTime;

	bool m_bKeepExisting; /* connections */
	bool m_bAutoDetect; /* connections */
	bool m_bForceProtocol;
	bool m_bFillGaps;
	bool m_bAutoSetup; /* channels */
	bool m_bForceAll; /* channels (implies don't disactivate) */
	bool m_bDisactivate; /* unconnected channels */
	bool m_bSetSensorType; /* of unknown sensors to MyoScan Pro */
	bool m_bShortReads;
	bool m_bSynchronize; /* multiple encoders */
	bool m_bSynchInternal; /* send internal synch trigger */
	bool m_bSynchPeriodic; /* send internal periodic synch. switches */
	bool m_bRecord;
	int m_idUnitType; // command id of current unit
	int m_idFilter; // command id of current filter
	TTLAPI_UNIT_TYPES m_nUnitType;
	double m_nScale;
	CSimpleFilter m_oFilter;
	CSimpleRecorder m_oRecorder;
	bool m_bConnected;
	bool m_bRunning;
	bool m_bTest1;
	bool m_bTest2;
	bool m_bTest3;
	bool m_bTest4;

	virtual BOOL PreTranslateMessage( MSG* pMsg );
	virtual BOOL OnIdle();

	BEGIN_MSG_MAP_EX( CMainFrame )
		CHAIN_MSG_MAP( CSimpleTimer_type )
		MSG_WM_CREATE( OnCreate )
		MSG_WM_CLOSE( OnClose )
		CHAIN_MSG_MAP( CUpdateUI_type )
		COMMAND_ID_HANDLER_EX( ID_APP_EXIT, OnFileExit )
		COMMAND_ID_HANDLER_EX( ID_FILE_CLOSE, OnFileClose )
		COMMAND_ID_HANDLER_EX( ID_FILE_NEW, OnFileNew )
		COMMAND_ID_HANDLER_EX( ID_VIEW_TOOLBAR, OnViewToolBar )
		COMMAND_ID_HANDLER_EX( ID_VIEW_STATUS_BAR, OnViewStatusBar )
		COMMAND_ID_HANDLER_EX( ID_APP_ABOUT, OnAppAbout )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_KEEPEXISTING  , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_AUTODETECT    , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FORCEPROTOCOL , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILLGAPS      , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_AUTOSETUP     , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FORCEALL      , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_DISACTIVATE   , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_SETSENSORTYPE , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_SHORTREADS    , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_SYNCHRONIZE   , OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_SYNCH_INTERNAL, OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_SYNCH_PERIODIC, OnOptions )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_UNITTYPE_NONE , OnOptionsUnitType )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_UNITTYPE_SENV , OnOptionsUnitType )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_UNITTYPE_ENCV , OnOptionsUnitType )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_UNITTYPE_ENCC , OnOptionsUnitType )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILTER_NONE   , OnOptionsFilter )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILTER_FILTER1, OnOptionsFilter )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILTER_FILTER2, OnOptionsFilter )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILTER_FILTER3, OnOptionsFilter )
		COMMAND_ID_HANDLER_EX( ID_OPTIONS_FILTER_FILTER4, OnOptionsFilter )
		COMMAND_ID_HANDLER_EX( ID_DAC_RECORD, OnOptions )
		COMMAND_ID_HANDLER_EX( ID_DAC_START, OnDacStart )
		COMMAND_ID_HANDLER_EX( ID_DAC_STOP, OnDacStop )
		COMMAND_ID_HANDLER_EX( ID_TEST1, OnTest1 )
		COMMAND_ID_HANDLER_EX( ID_TEST2, OnTest2 )
		COMMAND_ID_HANDLER_EX( ID_TEST3, OnTest3 )
		COMMAND_ID_HANDLER_EX( ID_TEST4, OnTest4 )
		NOTIFY_CODE_HANDLER_EX( TVN_SELCHANGED, OnTvSelChanged )
		CHAIN_MSG_MAP( CTTLLivePnPEventsSink_type )
		CHAIN_MSG_MAP( CFrameWindowImpl_type )
	END_MSG_MAP()

	LRESULT OnCreate( LPCREATESTRUCT /*lpCreateStruct*/ );
	void OnClose( void );
	void OnFileExit( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnFileClose( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnFileNew( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnViewToolBar( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnViewStatusBar( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnAppAbout( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnOptions( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnOptionsUnitType( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnOptionsFilter( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnDacStart( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnDacStop( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnSimpleTimer( void );
	void OnTest1( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnTest2( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnTest3( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	void OnTest4( UINT /*uNotifyCode*/, int /*nID*/, CWindow /*wndCtl*/ );
	LRESULT OnTvSelChanged( LPNMHDR /*lpNmHdr*/ );
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_MAINFRM_H__A19A3886_8A46_4E72_900D_EC9D3B9735F8__INCLUDED_)
