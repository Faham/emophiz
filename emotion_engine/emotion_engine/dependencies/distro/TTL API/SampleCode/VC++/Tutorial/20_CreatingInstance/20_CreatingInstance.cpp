// 20_CreatingInstance.cpp :

#include <stdio.h>

#undef USING_WRAPPER_CLASS
#include "..\ttllive.h"

int main(int argc, char* argv[])
{	
  HRESULT hr;

	ULONG ulCount;

	T_Version v;

	ITTLLive*		pTTLLive	= NULL;
	ITTLLive2*  pTTLLive2 = NULL;

  printf("\n\rSimple TTLLive Console Client not using wrapper class code or smart pointers.\n\r\n\r");

  PrintLine(MSG_CO_INITIALIZE);
	hr = CoInitialize(NULL);
	CheckHRESULT(hr);

	if( SUCCEEDED(hr)){

    PrintLine(MSG_CREATING_INSTANCE);
		
		// First we try creating a TTLLive object.
		hr = CoCreateInstance(
			__uuidof(TTLLive),    // Class identifier (CLSID) of the object.
			NULL,                 // Pointer to controlling IUnknown.
			CLSCTX_INPROC_SERVER, // Context for running executable code.
			__uuidof(ITTLLive),   // Reference to the identifier of the interface.
			(void**)&pTTLLive);   // Address of output variable that receives 
													  // the interface pointer requested in riid.
		CheckHRESULT(hr);
		if( SUCCEEDED(hr)){
			// Good, we now have an instance of a TTLLive object and also
			// a pointer to its ITTLLive interface. Now lets see if the object
			// supports the newer ITTLLive2 interface.

			PrintLine(MSG_QUERYING_TTLLIVE2);
			// Returns a pointer to a specified interface on an object to which a 
			// client currently holds an interface pointer. 			
			hr = pTTLLive->QueryInterface(__uuidof(ITTLLive2),(void**)&pTTLLive2);
			CheckHRESULT(hr);
			// It is required to AddRef() on a pointer returned by a call to QI()			
			if( SUCCEEDED(hr))ulCount = pTTLLive2->AddRef();

			// At this point we should have either an ITTLLive2 or ITTLLive pointer
			// or else we won't be doing much.
			if( pTTLLive ){
								
				PrintLine(MSG_GETTING_VERSION);
				hr = pTTLLive->get_Version(&v.liVersion);
				if( FAILED(hr))CheckHRESULT(hr);
				else printf("%d.%d.%d\n\r",v.byMajor, v.byMinor, v.woBuild);

				// Now, if available, lets try out some of the ITTLLive2 capabilities
				if( pTTLLive2 ){
					LONG liGlobalTickrate,liSyncTimeout;

				  // Getting global tickrate without any encoders yet it will return
					// 1. Getting sync timeout at this point should return TTLLLive
					// default value.
					PrintLine("Getting the GlobalTickRate property value");
					hr = pTTLLive2->get_GlobalTickRate(&liGlobalTickrate);
					if( FAILED(hr))CheckHRESULT(hr);
					else printf("%d\n\r",liGlobalTickrate);

					PrintLine("Getting the SyncTimeout property value");
					hr = pTTLLive2->get_SyncTimeout(&liSyncTimeout);
					if( FAILED(hr))CheckHRESULT(hr);
					else printf("%d\n\r",liSyncTimeout);
				}				
			}

			// Release instances by decremented references count.
			// After the CoUninitialize call if reference count is zero
			// the COM environment will unload the .dll			
			PrintLine(MSG_RELEASING_INSTANCES);
			if( pTTLLive )ulCount = pTTLLive->Release();
			if( pTTLLive2 )ulCount = pTTLLive2->Release();
			printf("Done!\n\r");
		}

		// We much balance a successful call to CoInitialize
		// by a call to CoUninitialize.
    PrintLine(MSG_CO_UNINITIALIZE);
    CoUninitialize();
		printf("Done!\n\r");

	} else {
		// CoInitialize failed..
		printf("Failed!\n\r");
	}
  
	printf("\n\rExiting application...\n\r");
	return 0;	
}