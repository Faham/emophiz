// main.cpp : Defines the entry point for the console application.
//

#include <stdio.h>
#include <conio.h> 

#define USING_WRAPPER_CLASS
#include "ttllive.h"

int main(int argc, char* argv[])
{	
  HRESULT hr;

	ULONG ulCount;

	ITTLLive*		pTTLLive	= NULL;

  printf("\n\rSimple Plain C++ TTLLive Console Test Client\n\r\n\r");

  PrintLine("Initializing COM libraries...");
	hr = CoInitialize(NULL);
	CheckHRESULT(hr);

	if( SUCCEEDED(hr)){

    PrintLine("Creating an instance of TTLLive object...");

		// First we try creating a TTLLive object.
		hr = CoCreateInstance(
			__uuidof(TTLLive),    // Class identifier (CLSID) of the object.
			NULL,                 // Pointer to controlling IUnknown.
			CLSCTX_INPROC_SERVER, // Context for running executable code.
			__uuidof(ITTLLive),   // Reference to the identifier of the interface.
			(void**)&pTTLLive     // Address of output variable that receives 
													  // the interface pointer requested in riid.
		);
		CheckHRESULT(hr);
		if( SUCCEEDED(hr)){
			// At this point we should have either an ITTLLive2 or ITTLLive pointer
			// or else we won't be doing much.
			if( pTTLLive ){
				T_Version v;
				LONG liEncoderCount = 0;
				LONG liEncoderHND = 0;
				
				PrintLine("Getting TTLLiveCtrl.dll version...");
				hr = pTTLLive->get_Version(&v.liVersion);
				if( FAILED(hr))CheckHRESULT(hr);
				else printf("%d.%d.%d\n\r",v.byMajor, v.byMinor, v.woBuild);

				// Lets attempt auto-detecting any encoder that might be present
				PrintLine("Attempting encoder autodetection");
				//hr = pTTLLive->OpenConnection(L"USB:0",1000,&liEncoderHND);
				hr = pTTLLive->OpenConnections(TTLAPI_OCCMD_AUTODETECT,1000,NULL,NULL);
				if( FAILED(hr))CheckHRESULT(hr);
				else {					
					hr = pTTLLive->get_EncoderCount(&liEncoderCount);
					if( FAILED(hr))printf("ITTLLive::get_EncoderCount failed...(0x%08X)\n\r",hr);
					else {
						printf("%d encoder(s) detected\n\r",liEncoderCount);
					}
				}

				if( liEncoderCount > 0 ){
					PrintLine("Creating data channels");
					hr = pTTLLive->AutoSetupChannels();
					if( FAILED(hr))CheckHRESULT(hr);
					else {
						LONG liChannelCount;
						hr = pTTLLive->get_ChannelCount(&liChannelCount);
						if( FAILED(hr))printf("ITTLLive::get_ChannelCount failed...(0x%08X)\n\r",hr);
						else {
							printf("%d channel(s) created\n\r",liChannelCount);
						}
					}
				}

			  PrintLine("Closing all connections");
			  hr = pTTLLive->CloseConnections();
			  CheckHRESULT(hr);
			}	

			// Release instances by decremented references count.
			// After the CoUninitialize call if reference count is zero
			// the COM environment will unload the .dll			
			PrintLine("Releasing interface instances");
			if( pTTLLive )ulCount = pTTLLive->Release();
			CheckHRESULT(0,"Done!");

		} else {
			// A TTLLive could not be created, was it registered on the system ?
		}

		// We much balance a successful call to CoInitialize
		// by a call to CoUninitialize.
    PrintLine("UnInitializing COM libraries");
    CoUninitialize();
		CheckHRESULT(0,"Done!");

	} else {
		// CoInitialize failed..
	}
  
	printf("\n\rExiting application...\n\r");
	return 0;	
}

