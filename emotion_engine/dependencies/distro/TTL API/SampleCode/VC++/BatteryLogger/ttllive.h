#if !defined(AFX_TTLLIVE_H__2E611266_DD2D_4FF2_A0F9_55290BAA3C8E__INCLUDED_)
#define AFX_TTLLIVE_H__2E611266_DD2D_4FF2_A0F9_55290BAA3C8E__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// Machine generated IDispatch wrapper class(es) created by Microsoft Visual C++

// NOTE: Do not modify the contents of this file.  If this class is regenerated by
//  Microsoft Visual C++, your modifications will be overwritten.

/////////////////////////////////////////////////////////////////////////////
// CTTLLive wrapper class

class CTTLLive : public CWnd
{
protected:
	DECLARE_DYNCREATE(CTTLLive)
public:
	CLSID const& GetClsid()
	{
		static CLSID const clsid
			= { 0x897b372a, 0xf588, 0x4143, { 0xaa, 0x5d, 0x34, 0x14, 0xd1, 0xdb, 0xc4, 0x2e } };
		return clsid;
	}
	virtual BOOL Create(LPCTSTR lpszClassName,
		LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect,
		CWnd* pParentWnd, UINT nID,
		CCreateContext* pContext = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID); }

    BOOL Create(LPCTSTR lpszWindowName, DWORD dwStyle,
		const RECT& rect, CWnd* pParentWnd, UINT nID,
		CFile* pPersist = NULL, BOOL bStorage = FALSE,
		BSTR bstrLicKey = NULL)
	{ return CreateControl(GetClsid(), lpszWindowName, dwStyle, rect, pParentWnd, nID,
		pPersist, bStorage, bstrLicKey); }

// Attributes
public:

// Operations
public:
	long GetEncoderCount();
	long GetChannelCount();
	long GetVersion();
	float GetBatteryLevelPct(long liEncoderHND);
	CString GetConnectionName(long liEncoderHND);
	long GetConnectionType(long liEncoderHND);
	long GetEncoderChannelCount(long liEncoderHND);
	CString GetEncoderModelName(long liEncoderHND);
	long GetEncoderModelType(long liEncoderHND);
	long GetFirmwareVersion(long liEncoderHND);
	long GetHardwareVersion(long liEncoderHND);
	long GetProtocolType(long liEncoderHND);
	long GetSensorStatus(long liEncoderHND);
	CString GetSerialNumber(long liEncoderHND);
	float GetAutoZero(long liChannelHND);
	void SetAutoZero(long liChannelHND, float newValue);
	long GetChannelActive(long liChannelHND);
	void SetChannelActive(long liChannelHND, long nNewValue);
	long GetChannelEncoderHND(long liChannelHND);
	long GetChannelPhysicalIndex(long liChannelHND);
	float GetNominalSampleRate(long liChannelHND);
	long GetNotification(long liChannelHND);
	void SetNotification(long liChannelHND, long nNewValue);
	long GetSamplesAvailable(long liChannelHND);
	long GetSensorConnected(long liChannelHND);
	long GetUnitType(long liChannelHND);
	void SetUnitType(long liChannelHND, long nNewValue);
	void RegisterClientThreadId(long liThreadHND);
	void AssignEncoderHND(LPCTSTR bstrSerialNumber, long liNewEncoderHND);
	void CloseConnection(long liEncoderHND);
	void CloseConnections();
	long GetFirstEncoderHND();
	long GetNextEncoderHND();
	long OpenConnection(LPCTSTR bstrDeviceName, long liPollingInterval);
	void OpenConnections(long libmCommand, long liPollingInterval, long* plibmScanned, long* plibmDetected);
	void AddChannel(long liEncoderHND, long liPhysicalChannelIndex, long* pliChannelHND);
	void AutoSetupChannels();
	void DropChannel(long liChannelHND);
	void DropChannels();
	long GetFirstChannelHND();
	long GetNextChannelHND();
	long ReadChannelData(long liChannelHND, float* pfBuffer, long* pliMaxSamples);
	VARIANT ReadChannelDataVT(long liChannelHND, long liMaxSamples);
	void StartChannels();
	void StopChannels();
	long GetFirstChannelHND(long liEncoderHND);
	void SetFirstChannelHND(long liEncoderHND, long nNewValue);
	long GetEncoderID(long liEncoderHND);
	long GetNotificationMask();
	void SetNotificationMask(long nNewValue);
	long GetEventInsertionMask(long liChannelHND);
	void SetEventInsertionMask(long liChannelHND, long nNewValue);
	long GetSensorID(long liChannelHND);
	long GetSensorState(long liChannelHND);
	long GetEncoderState(long liEncoderHND);
	long GetForceSensor(long liChannelHND);
	void SetForceSensor(long liChannelHND, long nNewValue);
	float GetChannelOffset(long liChannelHND);
	void SetChannelOffset(long liChannelHND, float newValue);
	long GetSensorType(long liChannelHND);
	void SetSensorType(long liChannelHND, long nNewValue);
	long GetImpCheckType();
	void SetImpCheckType(long nNewValue);
	float GetImpCheckAge(long liChannelHND);
	float GetImpCheckResults(long liChannelHND, long liResultIndex);
	void StartRecord(long liEncoderHND, LPCTSTR bstrFilename);
	void StopRecord(long liEncoderHND);
	DATE GetEncoderTime(long liEncoderHND);
	DATE GetEncoderTimeAdjustment(long liEncoderHND);
	long GetPlaybackMode(long liEncoderHND);
	void SetPlaybackMode(long liEncoderHND, long nNewValue);
	long GetPlaybackStart(long liEncoderHND);
	void SetPlaybackStart(long liEncoderHND, long nNewValue);
	long GetPlaybackEnd(long liEncoderHND);
	void SetPlaybackEnd(long liEncoderHND, long nNewValue);
	long GetPlaybackPosition(long liEncoderHND);
	void SetPlaybackPosition(long liEncoderHND, long nNewValue);
	float GetPlaybackSpeed(long liEncoderHND);
	void SetPlaybackSpeed(long liEncoderHND, float newValue);
	long GetSwitchState(long liEncoderHND);
	void SetSwitchState(long liEncoderHND, long nNewValue);
	float GetBatteryLevelVolt(long liEncoderHND);
	float GetBatteryTimeLeft(long liEncoderHND);
	float GetChannelScale(long liChannelHND);
	void SetChannelScale(long liChannelHND, float newValue);
	long GetEventInputState(long liEncoderHND);
	float GetTickRate(long liEncoderHND);
	long GetDecryptionState(long liEncoderHND);
	void UnlockDecryption(long liEncoderHND, long liUnlockValue);
	void SetEncoderModelType(long liEncoderHND, long nNewValue);
	CString GetProtocolName(long liEncoderHND);
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TTLLIVE_H__2E611266_DD2D_4FF2_A0F9_55290BAA3C8E__INCLUDED_)
