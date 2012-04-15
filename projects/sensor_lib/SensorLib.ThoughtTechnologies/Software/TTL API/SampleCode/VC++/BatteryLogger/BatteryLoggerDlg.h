// BatteryLoggerDlg.h : header file
//
//{{AFX_INCLUDES()
#include "LogFile.h"
#include "ttllive.h"
//}}AFX_INCLUDES

#include "COMDEF.H" // mainly for _com_error

#if !defined(AFX_BatteryLoggerDLG_H__21F86CFE_9E28_440F_BC0D_AA6E8664186B__INCLUDED_)
#define AFX_BatteryLoggerDLG_H__21F86CFE_9E28_440F_BC0D_AA6E8664186B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CBatteryLoggerDlg dialog

#define TIMER_CONTROL_UPDATE    101
#define TIMER_BATTERY_POLL      102

class CBatteryLoggerDlg : public CDialog
{
// Construction
public:  
	CBatteryLoggerDlg(CWnd* pParent = NULL);	// standard constructor
// Dialog Data
	//{{AFX_DATA(CBatteryLoggerDlg)
	enum { IDD = IDD_BatteryLogger_DIALOG };
	CComboBox	m_cbPorts;
	CEdit	m_ebBatteryLevel;
	CListBox	m_lbLogWindow;
	CTTLLive	m_TTLLive;
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBatteryLoggerDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CBatteryLoggerDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnClose();
	afx_msg void OnDestroy();
	virtual void OnOK();
	virtual void OnCancel();	
	afx_msg void OnTimer(UINT nIDEvent);
	afx_msg void OnButtonStopLogging();
	afx_msg void OnButtonStartLog();
  afx_msg void OnEncoderStateChangeTtllive(long, long);
	afx_msg void OnRadioVolts();
	afx_msg void OnRadioPct();
	afx_msg void OnOk();
	DECLARE_EVENTSINK_MAP()
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
private:
  SYSTEMTIME m_systemtimeStart;
  BOOL  m_bLogging;
  BOOL  m_bKillTimer;
  BOOL  m_bBatteryLevelInVolts; 
	CLogFile* m_pLogFile;
	//CTimeStamp* m_pTimeStamp;


  void UpdateStatus(){
    if( m_bLogging ){
      SetDlgItemText(IDC_EDIT_STATUS, _T("Logging"));
    } else {
      SetDlgItemText(IDC_EDIT_STATUS, _T("Stopped"));
    }
  };

  void DumpCOMError(_com_error e);
	int m_iCountDown;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_BatteryLoggerDLG_H__21F86CFE_9E28_440F_BC0D_AA6E8664186B__INCLUDED_)
