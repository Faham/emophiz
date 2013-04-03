#include "cbase.h"

#include <windows.h>
#include <mscoree.h>
#include <assert.h>
#include <stdio.h>
#include <tchar.h>
#include <math.h>

/*
// Import mscorlib typelib. Using 1.0 for maximum backwards compatibility
#import "C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.tlb" auto_rename
// Link with mscoree.dll import lib.
#pragma comment(lib,"mscoree.lib") 

using namespace mscorlib;
*/
class CDirector : public CLogicalEntity
{
public:
	DECLARE_CLASS( CDirector, CLogicalEntity );

	DECLARE_DATADESC();
 
	// Constructor
	CDirector () 
		//:
		//m_pDefaultDomain(NULL),
		//m_pCLRHost(NULL)
	{
		m_nCounter = 0;
		m_nAlive = 0;
		m_nRound = 1;
		//InitCLR();
		//InitEmophiz();
	}
 
	// Input function
	void InputTickZombie( inputdata_t &inputData );
	void InputTickZombieDied( inputdata_t &inputData );

private:
	int m_nThreshold; // Count at which to fire our output
	float m_nIncreasePower; // Increase Power
	int m_nCounter;   // Internal counter
	int m_nAlive; // number of alive enemies
	int m_nRound; // round number
	//_AppDomainPtr m_pDefaultDomain;
	//ICorRuntimeHost *m_pCLRHost; 
 
	COutputEvent m_OnNextRound;	// Output event when the counter reaches the threshold
	COutputEvent m_OnContinue;

	//int InitCLR();
	//int InitEmophiz();
};

LINK_ENTITY_TO_CLASS( director, CDirector );

// Start of our data description for the class
BEGIN_DATADESC( CDirector )
 
	// For save/load
	DEFINE_FIELD( m_nCounter, FIELD_INTEGER ),
 
	// As above, and also links our member variable to a Hammer keyvalue
	DEFINE_KEYFIELD( m_nThreshold, FIELD_INTEGER, "threshold" ),
 	DEFINE_KEYFIELD( m_nIncreasePower, FIELD_FLOAT, "increase_power" ),
 
	// Links our input name from Hammer to our input member function
	DEFINE_INPUTFUNC( FIELD_VOID, "TickZombie", InputTickZombie ),
	DEFINE_INPUTFUNC( FIELD_VOID, "TickZombieDied", InputTickZombieDied ),
 
	// Links our output member variable to the output name used by Hammer
	DEFINE_OUTPUT( m_OnNextRound, "OnNextRound" ),
	DEFINE_OUTPUT( m_OnContinue, "OnContinue" ),
 
END_DATADESC()

//-----------------------------------------------------------------------------
// Purpose: Handle a tick input from another entity
//-----------------------------------------------------------------------------
void CDirector::InputTickZombie( inputdata_t &inputData )
{
	++m_nCounter;
	++m_nAlive;
}

//-----------------------------------------------------------------------------
// Purpose: Handle a tick input from another entity
//-----------------------------------------------------------------------------
void CDirector::InputTickZombieDied( inputdata_t &inputData )
{
	--m_nAlive;
	if ( m_nCounter < m_nThreshold )
	{
		int _min = min(int(m_nThreshold * 0.6), m_nThreshold - m_nCounter);
		for (int i = 0; i < _min; ++i)
			m_OnContinue.FireOutput( inputData.pActivator, this );
	}
	else if ( m_nCounter == m_nThreshold && m_nAlive == 0)
	{
		m_nCounter = 0;
		++m_nRound;
		m_nThreshold = int(powf(m_nThreshold, m_nIncreasePower));
		m_OnNextRound.FireOutput( inputData.pActivator, this );
	}
}

/*
//-----------------------------------------------------------------------------
// Purpose: Initialize the .net clr
//-----------------------------------------------------------------------------
int CDirector::InitCLR()
{
	//
	// Query 'HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v3.5\Install' DWORD value
	// See http://support.microsoft.com/kb/318785/ for more information on .NET runtime versioning information
	//
	HKEY key = NULL;
	DWORD lastError = 0;
	lastError = RegOpenKeyEx(HKEY_LOCAL_MACHINE,TEXT("SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.5"),0,KEY_QUERY_VALUE,&key);
	if(lastError!=ERROR_SUCCESS) {
		_putts(TEXT("Error opening HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.5"));
		return 1;
	} 

	DWORD type;
	BYTE data[4];
	DWORD len = sizeof(data);
	lastError = RegQueryValueEx(key,TEXT("Install"),NULL,&type,data,&len);
 
	if(lastError!=ERROR_SUCCESS) {
		RegCloseKey(key);
		_putts(TEXT("Error querying HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v3.5\\Install"));
		return 2;
	} 

	RegCloseKey(key); 

	// Was Install DWORD key value == 1 ??
	if(data[0]==1)
		_putts(TEXT(".NET Framework 3.5 is installed"));
	else {
		_putts(TEXT(".NET Framework 3.5 is NOT installed"));
		return 3;
	} 

	// 
	// Load the runtime the 3.5 Runtime (CLR version 2.0)
	//
	LPWSTR pszVer = L"v2.0.50727";  // .NET Fx 3.5 needs CLR 2.0
	LPWSTR pszFlavor = L"wks";

	HRESULT hr = CorBindToRuntimeEx(pszVer,	   
		pszFlavor,
		STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN | STARTUP_CONCURRENT_GC,
		CLSID_CorRuntimeHost,
		IID_ICorRuntimeHost,
		(void **)&m_pCLRHost
	); 

	if (!SUCCEEDED(hr)) {
		_tprintf(TEXT("CorBindToRuntimeEx failed 0x%x\n"),hr);
		return 1;
	}
 
	_putts(TEXT("Loaded version 2.0.50727 of the CLR\n"));
 
	m_pCLRHost->Start(); // Start the CLR 

	//
	// Get a pointer to the default domain in the CLR
	//
	IUnknownPtr   pAppDomainPunk = NULL; 

	hr = m_pCLRHost->GetDefaultDomain(&pAppDomainPunk);
	assert(pAppDomainPunk); 
 
	hr = pAppDomainPunk->QueryInterface(__uuidof(_AppDomain),(void**) &m_pDefaultDomain);
	assert(m_pDefaultDomain); 
}

//-----------------------------------------------------------------------------
// Purpose: Initialize the .net clr
//-----------------------------------------------------------------------------
int CDirector::InitEmophiz()
{
	try {
		_ObjectHandlePtr pObjectHandle; 
		_ObjectPtr pObject; 
		_TypePtr pType;
		SAFEARRAY* psa; 

		// Create an instance of a type from an assembly, no path -- local directory
		pObjectHandle = m_pDefaultDomain->CreateInstanceFrom(L"ClassLibrary1.dll", L"ClassLibrary1.Class1");
  
		variant_t vtobj = pObjectHandle->Unwrap();                                    // Get an _Object (as variant) from the _ObjectHandle
		vtobj.pdispVal->QueryInterface(__uuidof(_Object),(void**)&pObject);           // QI the variant for the Object iface
		pType = pObject->GetType();                                                   // Get the _Type iface
		psa = SafeArrayCreateVector(VT_VARIANT,0,0);                                  // Create a safearray (0 length)
		pType->InvokeMember_3("Test", BindingFlags_InvokeMethod, NULL, vtobj, psa );  // Invoke "Test" method on pType
		SafeArrayDestroy(psa);                                                        // Destroy safearray
	}
	catch(_com_error& error) {
		_tprintf(TEXT("ERROR: %s\n"),(_TCHAR*)error.Description());
		goto exit;
	}

exit:
	m_pCLRHost->Stop();
	m_pCLRHost->Release(); 

	return 0;
}

*/