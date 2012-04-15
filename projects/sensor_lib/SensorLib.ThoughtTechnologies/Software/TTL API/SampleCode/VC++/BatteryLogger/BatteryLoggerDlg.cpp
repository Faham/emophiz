// BatteryLoggerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "BatteryLogger.h"
#include "BatteryLoggerDlg.h"

// The usage of import or include for TTLLiveCtrl here is solely to get 
// TTLLiveCtrl typelib enums.
#if 0
  // (re-)generate header for TTLLive 
  // Note: while in VS, clicking anywhere on the import line will
  // regenerate the .tlh file in the intermediate folder...!
	#import "..\..\..\TTLLiveCtrl.dll" \
		no_namespace \
		raw_interfaces_only, raw_native_types, named_guids
#else
  // copied into project's root folder we simply include generated header.
	#include "TTLLiveCtrl.tlh"
#endif

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CAboutDlg dialog used for App About

class CAboutDlg : public CDialog
{
public:
	CAboutDlg();

// Dialog Data
	//{{AFX_DATA(CAboutDlg)
	enum { IDD = IDD_ABOUTBOX };
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CAboutDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	//{{AFX_MSG(CAboutDlg)
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialog(CAboutDlg::IDD)
{
	//{{AFX_DATA_INIT(CAboutDlg)
	//}}AFX_DATA_INIT
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CAboutDlg)
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialog)
	//{{AFX_MSG_MAP(CAboutDlg)
		// No message handlers
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

void CBatteryLoggerDlg::DumpCOMError(_com_error e)
{
  TCHAR szStr[64];
	TCHAR szStr2[256];

	_stprintf(szStr2,_T("COM Error \t: %08lx"),e.Error());	
	_tcscat(szStr2,_T("\r\n"));
	_stprintf(szStr,_T("Source     \t: %s"),static_cast<LPCSTR>(e.Source()));
	_tcscat(szStr2,szStr);
	_tcscat(szStr2,_T("\r\n"));
	_stprintf(szStr,_T("Description\t: %s"),static_cast<LPCSTR>(e.Description()));
	_tcscat(szStr2,szStr);
	_tcscat(szStr2,_T("\r\n"));
	_stprintf(szStr,_T("Message    \t: %s"),e.ErrorMessage());
	_tcscat(szStr2,szStr);

	m_pLogFile->Add(szStr2,TRUE);
  m_lbLogWindow.AddString(szStr2);	
}

/////////////////////////////////////////////////////////////////////////////
// CBatteryLoggerDlg dialog

CBatteryLoggerDlg::CBatteryLoggerDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CBatteryLoggerDlg::IDD, pParent)
{
	//{{AFX_DATA_INIT(CBatteryLoggerDlg)
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);	
  m_bBatteryLevelInVolts = TRUE;
  m_bKillTimer = FALSE;
	m_pLogFile = NULL;
}

void CBatteryLoggerDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CBatteryLoggerDlg)
	DDX_Control(pDX, IDC_COMBO_PORTS, m_cbPorts);
	DDX_Control(pDX, IDC_EDIT_BATTERY_LEVEL, m_ebBatteryLevel);
	DDX_Control(pDX, IDC_LIST_LOG_WINDOW, m_lbLogWindow);
	DDX_Control(pDX, IDC_TTLLIVE, m_TTLLive);
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(CBatteryLoggerDlg, CDialog)
	//{{AFX_MSG_MAP(CBatteryLoggerDlg)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_WM_CLOSE()
	ON_WM_DESTROY()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDC_BUTTON_STOP_LOGGING, OnButtonStopLogging)
	ON_BN_CLICKED(IDC_BUTTON_START_LOG, OnButtonStartLog)
	ON_BN_CLICKED(IDC_RADIO_VOLTS, OnRadioVolts)
	ON_BN_CLICKED(IDC_RADIO_PCT, OnRadioPct)
	ON_BN_CLICKED(IDC_BUTTON_CANCEL, OnCancel)
	ON_BN_CLICKED(IDC_BUTTON_OK, OnOK)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CBatteryLoggerDlg message handlers

BOOL CBatteryLoggerDlg::OnInitDialog()
{
  CWinApp* pApp = AfxGetApp();

	CDialog::OnInitDialog();

	// Add "About..." menu item to system menu.  
	// IDM_ABOUTBOX must be in the system command range.
  /*
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);
  
	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		CString strAboutMenu;
		strAboutMenu.LoadString(IDS_ABOUTBOX);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	} 
  */ 
	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	// TODO: Add extra initialization here  
  //OnChangeEditBatteryPollTimer();
  m_pLogFile = NULL;


  m_cbPorts.SetCurSel(pApp->GetProfileInt( _T("Config"), _T("Port"),8));
  SetDlgItemInt( IDC_EDIT_BATTERY_POLL_TIMER, pApp->GetProfileInt( _T("Config"), _T("PollTimer"),60));
  m_bBatteryLevelInVolts = pApp->GetProfileInt( _T("Config"), _T("BatteryLevelMode"),0);

  CButton *p1,*p2;
  p1 = (CButton*)GetDlgItem(IDC_RADIO_VOLTS);
  p2 = (CButton*)GetDlgItem(IDC_RADIO_PCT);

  if( m_bBatteryLevelInVolts ){
    p1->SetCheck(TRUE);
    p2->SetCheck(FALSE);
  } else {
    p1->SetCheck(FALSE);
    p2->SetCheck(TRUE);
  }
  m_bLogging = FALSE;
  UpdateStatus();
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

void CBatteryLoggerDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
/*
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialog::OnSysCommand(nID, lParam);
	}
*/
  CDialog::OnSysCommand(nID, lParam);
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CBatteryLoggerDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CBatteryLoggerDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}

void CBatteryLoggerDlg::OnClose() 
{
	// TODO: Add your message handler code here and/or call default
	
	CDialog::OnClose();
}

void CBatteryLoggerDlg::OnDestroy() 
{  
  m_bKillTimer = KillTimer(TIMER_BATTERY_POLL);

  delete m_pLogFile;
  
	CDialog::OnDestroy();
}

void CBatteryLoggerDlg::OnOK(void) 
{
  CWinApp* pApp = AfxGetApp();

  pApp->WriteProfileInt( _T("Config"), _T("BatteryLevelMode"),m_bBatteryLevelInVolts);

  pApp->WriteProfileInt( _T("Config"), _T("Port"),m_cbPorts.GetCurSel());

  pApp->WriteProfileInt( _T("Config"), _T("PollTimer"),GetDlgItemInt( IDC_EDIT_BATTERY_POLL_TIMER));

	CDialog::OnOK();
}

void CBatteryLoggerDlg::OnCancel() 
{
	// TODO: Add extra cleanup here
	//if( m_pLogFile )delete m_pLogFile;		
	CDialog::OnCancel();
}

BEGIN_EVENTSINK_MAP(CBatteryLoggerDlg, CDialog)
    //{{AFX_EVENTSINK_MAP(CBatteryLoggerDlg)
	ON_EVENT(CBatteryLoggerDlg, IDC_TTLLIVE, 4 /* EncoderStateChange */, OnEncoderStateChangeTtllive, VTS_I4 VTS_I4)
	//}}AFX_EVENTSINK_MAP
END_EVENTSINK_MAP()

void CBatteryLoggerDlg::OnTimer(UINT nIDEvent) 
{
	_TCHAR str[80];
  FLOAT fBatteryLevel;
  SYSTEMTIME systemtime;
  __int64 i64timeStart,i64timeDelta;
  int iHours, iMin, iSec;
	
	CDialog::OnTimer(nIDEvent);

  if( m_bKillTimer )return;

  switch( nIDEvent ){
    case TIMER_BATTERY_POLL :
      SetDlgItemInt( IDC_EDIT_BATTERY_POLL_COUNTDOWN, m_iCountDown);
      UpdateStatus();
      GetSystemTime(&systemtime);
      SystemTimeToFileTime(&systemtime,(LPFILETIME)&i64timeDelta);
      SystemTimeToFileTime(&m_systemtimeStart,(LPFILETIME)&i64timeStart);

      // 100-nanosecond intervals
      i64timeDelta-=i64timeStart;

      i64timeDelta /= 10000000L;
      iHours = static_cast<int>(i64timeDelta / 3600);
      i64timeDelta %= 3600;

      iMin   = static_cast<int>(i64timeDelta / 60);
      i64timeDelta %= 60;

      iSec   = static_cast<int>(i64timeDelta);

	    _stprintf(str,_T("%d:%02d:%02d"),iHours,iMin,iSec);
      SetDlgItemText(IDC_EDIT_ELAPSED_TIME, str);

      
      try {        
        if( m_bBatteryLevelInVolts ){
          fBatteryLevel = m_TTLLive.GetBatteryLevelVolt(m_TTLLive.GetFirstEncoderHND());
	        _stprintf(str,_T("%5.3f volts%"),fBatteryLevel);
        } else {
          fBatteryLevel = m_TTLLive.GetBatteryLevelPct(m_TTLLive.GetFirstEncoderHND());
	        _stprintf(str,_T("%6.3f%%"),fBatteryLevel);
        }
	      m_ebBatteryLevel.SetWindowText(str);
        m_iCountDown--;
        if( m_iCountDown < 0 ){
          m_iCountDown = GetDlgItemInt(IDC_EDIT_BATTERY_POLL_TIMER);  
		      m_pLogFile->Add(str,TRUE);
        }      
      } catch ( _com_error e ){
		    DumpCOMError(e);
	    }	
    break;
  }
}

void CBatteryLoggerDlg::OnButtonStopLogging() 
{
	// TODO: Add your control notification handler code here
  if( m_pLogFile ){
    //m_pLogFile->Close();
    delete m_pLogFile;
    m_pLogFile = NULL;
  }
  
  m_bLogging = FALSE;
  UpdateStatus();
  KillTimer(TIMER_BATTERY_POLL);
	m_TTLLive.CloseConnections();	
	m_lbLogWindow.AddString(_T("Logging stopped."));	  
}

void CBatteryLoggerDlg::OnButtonStartLog() 
{
	_TCHAR str[80];
	_TCHAR szDeviceStr[16];
	
	m_cbPorts.GetWindowText(szDeviceStr,10);

	try {
		m_TTLLive.OpenConnection(szDeviceStr, 1000 );

    if( m_TTLLive.GetEncoderCount() > 0 ){
			m_TTLLive.SetNotificationMask(TTLAPI_ON_ENCODER_STATE_CHANGE_ACTIVE);
      m_pLogFile = new CLogFile2(_T("BatteryLog.txt"));
 		  _stprintf(str,_T("Logging started on %s port."),szDeviceStr);
      GetSystemTime(&m_systemtimeStart);
      OnTimer(TIMER_BATTERY_POLL);
      SetTimer(TIMER_BATTERY_POLL,1000,NULL);
      m_iCountDown = 0; 
      m_bLogging = TRUE;
      UpdateStatus();     
    } else {
 		  _stprintf(str,_T("No encoder found on %s port."),szDeviceStr);
    }
		m_lbLogWindow.AddString(str);    
	} catch ( _com_error &e ) {
    DumpCOMError(e);
	} 
}

void CBatteryLoggerDlg::OnRadioVolts() 
{
	m_bBatteryLevelInVolts = TRUE;
	
}

void CBatteryLoggerDlg::OnRadioPct() 
{
  m_bBatteryLevelInVolts = FALSE;
	
}

void CBatteryLoggerDlg::OnEncoderStateChangeTtllive(long liEncoderHND, long liNewState) 
{
  if( liEncoderHND == m_TTLLive.GetFirstEncoderHND()){

    if( liNewState != TTLAPI_ENC_ST_ONLINE ){
      OnButtonStopLogging();
    }      
  }
}
