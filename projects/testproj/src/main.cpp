
//#include "nativeAdapter/NativeProxy.h"
//#include "nativeAdapter/NativeProxyStatic.h"
//#include "nativeTools/anyType.h"
//
//#include <iostream>
//
//class SensorProviderNative
//{
//public:
//    SensorProviderNative() : wrapper_(_T("EmotionProvider.dll"), _T("emophiz.SensorProvider")) {}
//	std::string test() { wrapper_(_T("test")); }
//
//private:
//	nativeAdapter::NativeProxy wrapper_;
//};
//
//void main() {
//	SensorProviderNative spn;
//	std::cout << "test";
//}

/*
    CheckCLR.cpp - A Native Code CLR Host Sample 

    CheckCLR is a native console application which queries the registry to 
    determine if a specific version of the .NET runtime is installed on the
    machine. It then loads the specific CLR required. 

    It will then load a managed console app and execute the Main method.
    It then loads a managed assembly and executes a method.
*/

#include <windows.h>
#include <mscoree.h>
#include <assert.h>
#include <stdio.h>
#include <tchar.h> 

// Import mscorlib typelib. Using 1.0 for maximum backwards compatibility
#import "C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.tlb" auto_rename
// Link with mscoree.dll import lib.
#pragma comment(lib,"mscoree.lib") 

using namespace mscorlib; 

int _tmain(int argc, _TCHAR* argv[])
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
    ICorRuntimeHost *pHost = NULL; 

    HRESULT hr = CorBindToRuntimeEx( pszVer,       
                                                       pszFlavor,    
                                                       STARTUP_LOADER_OPTIMIZATION_SINGLE_DOMAIN | STARTUP_CONCURRENT_GC, 
                                                       CLSID_CorRuntimeHost, 
                                                       IID_ICorRuntimeHost, 
                                                       (void **)&pHost); 

    if (!SUCCEEDED(hr)) {
        _tprintf(TEXT("CorBindToRuntimeEx failed 0x%x\n"),hr);
        return 1;
    }
 
    _putts(TEXT("Loaded version 2.0.50727 of the CLR\n"));
 
    pHost->Start(); // Start the CLR 

    //
    // Get a pointer to the default domain in the CLR
    //
    _AppDomainPtr pDefaultDomain = NULL;
    IUnknownPtr   pAppDomainPunk = NULL; 

    hr = pHost->GetDefaultDomain(&pAppDomainPunk);
    assert(pAppDomainPunk); 
 
    hr = pAppDomainPunk->QueryInterface(__uuidof(_AppDomain),(void**) &pDefaultDomain);
    assert(pDefaultDomain); 

    //
    // Load an Exe Assembly and call Main()
    //
    _bstr_t bstrExeName = L"D:\\faham\\emophiz\\emophiz\\projects\\emotion_game\\Minigames\\Minigames\\bin\\x86\\Release\\Minigames.exe";
    try {
        hr = pDefaultDomain->ExecuteAssembly_2(bstrExeName);
    }
    catch(_com_error& error) {
        _tprintf(TEXT("ERROR: %s\n"),(_TCHAR*)error.Description());
        goto exit;
    }

    /*
        Load a type from a DLL Assembly and call it 

        Doing the same thing from native code as this C# code: 

            System.Runtime.Remoting.ObjectHandle objptr;
            objptr = AppDomain.CurrentDomain.CreateInstanceFrom("ClassLibrary1.dll",
                                                                                           "ClassLibrary1.Class1");
            object obj = objptr.Unwrap();
            Type t = obj.GetType();
            t.InvokeMember("Test",
                                   BindingFlags.InvokeMethod,
                                   null,
                                   obj,
                                   new object[0]);
    */
    //try {
    //    _ObjectHandlePtr pObjectHandle; 
    //    _ObjectPtr pObject; 
    //    _TypePtr pType;
    //    SAFEARRAY* psa; 
	//
    //    // Create an instance of a type from an assembly
    //    pObjectHandle = pDefaultDomain->CreateInstanceFrom(
	//		L"D:\\faham\\emophiz\\emophiz\\projects\\emotion_game\\Minigames\\Minigames\\bin\\x86\\Release\\ClassLibrary1.dll", // no path -- local directory
	//		L"ClassLibrary1.Class1");
  	//
    //    variant_t vtobj = pObjectHandle->Unwrap();                          // Get an _Object (as variant) from the _ObjectHandle
    //    vtobj.pdispVal->QueryInterface(__uuidof(_Object),(void**)&pObject); // QI the variant for the Object iface
    //    pType = pObject->GetType();                                         // Get the _Type iface
    //    psa = SafeArrayCreateVector(VT_VARIANT,0,0);                        // Create a safearray (0 length)
    //    pType->InvokeMember_3("Test",                                       // Invoke "Test" method on pType
    //                                        BindingFlags_InvokeMethod,
    //                                        NULL,
    //                                        vtobj,
    //                                        psa );
    //    SafeArrayDestroy(psa);                                                                   // Destroy safearray
    //}
    //catch(_com_error& error) {
    //    _tprintf(TEXT("ERROR: %s\n"),(_TCHAR*)error.Description());
    //    goto exit;
    //} 

exit:
    pHost->Stop();
    pHost->Release(); 

    return 0;
}

/*** End of file CheckCLR.cpp ***/
