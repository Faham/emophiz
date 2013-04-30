// 11_Simplest.cpp : 

#include <stdio.h>

#define USING_WRAPPER_CLASS
#include "..\ttllive.h"

int main(int argc, char* argv[])
{	
	HRESULT hr;

	printf("\n\rSimple TTLLive Console Test Client\n\r\n\r");
	
  PrintLine(MSG_CO_INITIALIZE);	
	hr = CoInitialize(NULL);
	if( SUCCEEDED(hr)){

		CheckHRESULT(hr);

		try {		
			
			PrintLine(MSG_CREATING_INSTANCE);

			// Declare and create an interface class object of ITTLLive type.
			//
			// Note that using the following delclaration type will generate
			// a COM exception if the object or interface cannot be created.		
			ITTLLivePtr TTLLive(CLSID_TTLLive);
			
			// Only declare an interface class object of ITTLLive2 type. We shall
			// see whether ITTLive2 interface is available later on.
			ITTLLive2Ptr TTLLive2;

			// If we get here, the ITTLLive interface pointer was loaded properly.			
			printf("Success!\n\r");

			// Let's get version.
			LONG liVersion;
			PrintLine(MSG_GETTING_VERSION);
			liVersion = TTLLive->Version;
			printf("0x%08X\n\r",liVersion);

			// Let's 'query' for ITTLLive2 availability
			PrintLine(MSG_QUERYING_TTLLIVE2);
			// This is the same as calling QueryInterface
			TTLLive2 = TTLLive;
			
			(TTLLive2 != NULL)?printf("Available\n\r"):printf("Not available\n\r");

		} catch( _com_error &e){
			printf("(0x%08X) %s\n\r",e.Error(),e.ErrorMessage());
		}

		// We much balance a successful call to CoInitialize
		// by a call to CoUninitialize.
    PrintLine(MSG_CO_UNINITIALIZE);
    CoUninitialize();
		printf("Done!\n\r");
	} else {
		printf("Failed!\n\r");
	}

	printf("\n\rExiting application...\n\r");
	return 0;
}
