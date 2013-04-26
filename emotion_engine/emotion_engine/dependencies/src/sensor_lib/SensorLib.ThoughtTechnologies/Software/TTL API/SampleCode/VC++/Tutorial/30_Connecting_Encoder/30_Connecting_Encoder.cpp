// 30_Connecting_Encoder.cpp : 

#include <stdio.h>

#define USING_WRAPPER_CLASS
#include "..\ttllive.h"

int main(int argc, char* argv[])
{	
	HRESULT hr;

	LONG liEncoderHND;

	ITTLLivePtr TTLLive;
	ITTLLive2Ptr TTLLive2;
	
	printf("\n\rSimple TTLLive Console Test Client\n\r\n\r");
	
  PrintLine(MSG_CO_INITIALIZE);	
	hr = CoInitialize(NULL);
	if( SUCCEEDED(hr)){

		CheckHRESULT(hr);

		PrintLine(MSG_CREATING_INSTANCE);

		// Declare and create an interface class object of ITTLLive type.
		// Unlike the "ITTLLivePtr TTLLive(CLSID_TTLLive)" usage, using
		// the CreateInstance requires later to call Release. Also using
		// CreateInstance does not generate an exception if it fails to 
		// create the desired object.	
		hr = TTLLive.CreateInstance(CLSID_TTLLive);
		CheckHRESULT(hr);
		if( SUCCEEDED(hr)){		
			try {
				T_Version sV;

				PrintLine(MSG_GETTING_VERSION);
				sV.liVersion = TTLLive->Version;
				printf("%d.%d.%d\n\r",sV.byMajor, sV.byMinor, sV.woBuild);
								
				// Let's 'query' for ITTLLive2 availability
				PrintLine(MSG_QUERYING_TTLLIVE2);
				// This is the same as calling QueryInterface
				TTLLive2 = TTLLive;
				
				(TTLLive2 != NULL)?printf("Available\n\r"):printf("Not available\n\r");

				LONG liEncoderCount;

				// Lets attempt auto-detecting any encoder that might be present
				PrintLine(MSG_AUTODETECTING);
				liEncoderHND = TTLLive->OpenConnection(L"USB:0",1000);
				//TTLLive->OpenConnections(TTLAPI_OCCMD_AUTODETECT,1000,NULL,NULL);
				
				liEncoderCount = TTLLive->EncoderCount;
				if( liEncoderCount > 0 ){
					printf("Found %d encoder%s.\n\r",liEncoderCount,(liEncoderCount>1)?"s":"");

					// If at least one encoder was detected we will print out
					// Serial Number, Battery Level, Model Type and protocol
					// for each one.					
					if( TTLLive2 ){
						
						ITTLEncoderPtr TTLEncoder;

						TTLEncoder = TTLLive2->GetFirstEncoder();
						while( TTLEncoder ){

							PrintLine("  Encoder HND");printf("%d\n\r",TTLEncoder->HND);
							PrintLine("  Connection Name");printf("%s\n\r",(char*)TTLEncoder->ConnectionName);							
							PrintLine("  Serial Number");printf("%s\n\r",(char*)TTLEncoder->SerialNumber);
							PrintLine("  Battery Level Volt");printf("%6.3f\n\r",TTLEncoder->BatteryLevelVolt);
							PrintLine("  Model Name");printf("(%d)%s\n\r",TTLEncoder->ModelType,(char*)TTLEncoder->ModelName);
							PrintLine("  Protocol");printf("(%d)%s\n\r",TTLEncoder->ProtocolType,(char*)TTLEncoder->ProtocolName);
							PrintLine("  EventInputState");printf("%s\n\r",(TTLEncoder->EventInputState==1)?"Active":"Inactive");
							PrintLine("  StreamQuality");printf("%6.3f\n\r",TTLEncoder->StreamQuality);
							PrintLine("  TicksAhead");printf("%6.3f\n\r",TTLEncoder->TicksAhead);

							TTLEncoder = TTLLive2->GetNextEncoder();
						}
					} else {

						LONG liEncoderHND;

						liEncoderHND = TTLLive->GetFirstEncoderHND();
						while( liEncoderHND >= 0 ){
							PrintLine("  Encoder HND");printf("%d\n\r",liEncoderHND);
							PrintLine("  Connection Name");printf("%s\n\r",(char*)TTLLive->ConnectionName[liEncoderHND]);
							PrintLine("  Serial Number");printf("%s\n\r",(char*)TTLLive->SerialNumber[liEncoderHND]);
							PrintLine("  Battery Level Volt");printf("%6.3f\n\r",TTLLive->BatteryLevelVolt[liEncoderHND]);
							PrintLine("  Model Name");printf("(%d)%s\n\r",TTLLive->EncoderModelType[liEncoderHND],(char*)TTLLive->EncoderModelName[liEncoderHND]);
							PrintLine("  Protocol");printf("(%d)%s\n\r",TTLLive->ProtocolType[liEncoderHND],(char*)TTLLive->ProtocolName[liEncoderHND]);
							PrintLine("  EventInputState");printf("Unavailable in ITTLLive\n\r");
							PrintLine("  StreamQuality");printf("Unavailable in ITTLLive\n\r");
							PrintLine("  TicksAhead");printf("Unavailable in ITTLLive\n\r");
							liEncoderHND = TTLLive->GetNextEncoderHND();
						}
					}
					PrintLine("Closing all connections");
					TTLLive->CloseConnections();printf("Succeeded!\n\r");
					
				} else {
				 printf("No encoder detected.\n\r");
				}

				// Release instances by decremented references count.
				// After the CoUninitialize call if reference count is zero
				// the COM environment will unload the .dll			
				PrintLine(MSG_RELEASING_INSTANCES);
				if( TTLLive )TTLLive.Release();
				if( TTLLive2 )TTLLive2.Release();
				printf("Done!\n\r");
				
			} catch( _com_error &e){
				printf("(0x%08X) %s\n\r",e.Error(),e.ErrorMessage());
			}			
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
