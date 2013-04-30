// 42_Reading_VARIANT_Data_MFC.cpp :

#include "stdafx.h"
#include "42_Reading_VARIANT_Data_MFC.h"

#include <stdio.h>
#include <conio.h>			// for kbhit() and getch()

#define USING_WRAPPER_CLASS
#include "..\ttllive.h"

ITTLLive2Ptr g_TTLLive;

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// The one and only application object

CWinApp theApp;

using namespace std;

void show_error(_com_error &e)
{
	printf("(0x%08X)\n\r",e.Error());
	PrintLine("");
	printf("%s\n\r",(char*)e.Description());
}

void read_data(void)
{
	COleSafeArray array;

	int col = 0;
  LONG liSamplesAvailable, liChannelHND;
  	
	liChannelHND = g_TTLLive->GetFirstChannelHND();
	while( liChannelHND > -1 ){	

		if( col == 0 )printf("\n\rData ");

		liSamplesAvailable = g_TTLLive->SamplesAvailable[liChannelHND];

    if( liSamplesAvailable ){
      array = g_TTLLive->ReadChannelDataVT(liChannelHND, liSamplesAvailable );
    }

		// If any samples available, we simply print out value of the first one
		array.GetUBound(1,&liSamplesAvailable );
		if( liSamplesAvailable ){
			LONG liIndice = 0;
			FLOAT fData;

			array.GetElement(&liIndice,&fData);

			printf("%c:%6.3f, ",(char)('A'+liChannelHND), fData);

		} else {
			printf("%c:NO DATA, ",(char)('A'+liChannelHND));
		}

		col++;
		if( col >= 5)col = 0;

		liChannelHND = g_TTLLive->GetNextChannelHND();
	}
	printf("\n\r");
}

void channel_setup(BOOL bForceSensors)
{
	LONG liChannelHND;

	LONG liChannelCount;

	PrintLine(MSG_SETTING_UP_CHANNELS);
	g_TTLLive->AutoSetupChannels();

	liChannelHND = g_TTLLive->GetFirstChannelHND();
	while( liChannelHND > -1 ){									
		// Setting up channels with arbitrary configuration, turning-off channel 
		// notification, pluggin-in sensor ID into sensor type and set channels
		// to output raw COUNTS.
		g_TTLLive->Notification[liChannelHND]=0;		
		g_TTLLive->ForceSensor[liChannelHND]=bForceSensors;
		g_TTLLive->SensorType[liChannelHND] = g_TTLLive->SensorID[liChannelHND];		
		g_TTLLive->UnitType[liChannelHND]=TTLAPI_UT_COUNT;
		liChannelHND = g_TTLLive->GetNextChannelHND();
	}	
	// if we get here it means there was no exceptions.
	liChannelCount = g_TTLLive->ChannelCount;
	printf("Created %d channel%s.\n\r",liChannelCount,(liChannelCount>1)?"s":"");
}

void force_channels(BOOL bForceSensors )
{
	LONG liChannelHND;

	printf("Force Channels = ");
	if( bForceSensors ){
		printf("TRUE, also acquiring data from unconnected channels.\n\r");
	} else {
		printf("FALSE, only acquiring data only from connected channels.\n\r");
	}

	liChannelHND = g_TTLLive->GetFirstChannelHND();
	while( liChannelHND > -1 ){
		g_TTLLive->ForceSensor[liChannelHND]=bForceSensors;
		liChannelHND = g_TTLLive->GetNextChannelHND();
	}	
}

void show_menu(void)
{
	printf("\n\r***** Main Menu *****\n\r\n\r");
	printf("press 'O' to attempt connecting to encoder.\n\r");
	printf("press 'C' to close all connections.\n\r");
	printf("press 'S' to setup channels.\n\r");
	printf("press 'D' to toggle acquiring data.\n\r");
	printf("press 'F' to toggle between [force channels/only use connected channels].\n\r");
	printf("press 'X' to exit.\n\r\n\r");
}

int my_main(void)
{
	LONG liEncoderCount;
	UINT_PTR puiTimer;
  HRESULT hr;
	MSG msg;
  int c;  

	BOOL bReadingData = FALSE;
	BOOL bForceSensors = TRUE;

	printf("\n\rSimple TTLLive Console Test Client Reading Actual Sample Data\n\r\n\r");
	
  PrintLine(MSG_CO_INITIALIZE);	

  hr = CoInitializeEx(NULL,COINIT_APARTMENTTHREADED);
	CheckHRESULT(hr);

  if( SUCCEEDED(hr)){
		PrintLine(MSG_CREATING_INSTANCE);

		// This will attempt creating an TTLLive object 
    hr = g_TTLLive.CreateInstance(CLSID_TTLLive);
		CheckHRESULT(hr);
    if( SUCCEEDED(hr)){
			try {

				T_Version sV;

				PrintLine(MSG_GETTING_VERSION);
				sV.liVersion = g_TTLLive->Version;
				printf("%d.%d.%d\n\r",sV.byMajor, sV.byMinor, sV.woBuild);
	    } catch ( _com_error &e ) {
		    show_error(e);
	    }  

			show_menu();
    
			do {
				try {
					if( kbhit()){
						c = toupper(getch());

						switch( c ){
							case 'O' :
								bReadingData = FALSE;
								PrintLine(MSG_AUTODETECTING);
								g_TTLLive->OpenConnections(TTLAPI_OCCMD_AUTODETECT,1000,NULL,NULL);
								//g_TTLLive->OpenConnection("USB:0",1000);
								liEncoderCount = g_TTLLive->EncoderCount;
								printf("Found %d encoder%s.\n\r",liEncoderCount,(liEncoderCount>1)?"s":"");
								break;

							case 'C' :
								PrintLine(MSG_CLOSE_CONNECTIONS);
								g_TTLLive->CloseConnections();
								liEncoderCount = g_TTLLive->EncoderCount;
								printf("EncoderCount = %d.\n\r",liEncoderCount);
								bReadingData = FALSE;
								::KillTimer(NULL,puiTimer);
								break;

							case 'S' :
								channel_setup(bForceSensors);
								break;

							case 'D' :
								bReadingData^=TRUE;
								if( bReadingData ){
									if( g_TTLLive->EncoderCount > 0 ){
										PrintLine("Starting data");
										g_TTLLive->StartChannels();
										CheckHRESULT(S_OK);
										// Setting windows timer to get a WM_TIMER messages 
										// about 5 times per second.
										puiTimer = ::SetTimer(NULL,0,200,NULL);
									} else {
										printf("No encoder created yet.\n\r");
									}
								} else {
									PrintLine("Stopping data");
									g_TTLLive->StartChannels();
									CheckHRESULT(S_OK);
									::KillTimer(NULL,puiTimer);
								}
								break;

							case 'F':
								bForceSensors^=TRUE;
								force_channels(bForceSensors);
								break;

							case 'X':
							  break;

							default:
								show_menu();
						} 
					}
     
					while( PeekMessage(&msg, NULL, 0, 0, PM_REMOVE)){
						switch( msg.message ){
							case WM_TIMER:
								if( msg.wParam == puiTimer ){
									if( bReadingData ){
										read_data();
									}
								}
								break;
						}
						DispatchMessage(&msg);
					}

					Sleep(10);
				} catch ( _com_error &e ) {
					show_error(e);
				} 

			} while( c !='X');
    
      // Releasing instance since we used 'g_TTLLive.CreateInstance'
			PrintLine(MSG_RELEASING_INSTANCES);
			if( g_TTLLive )g_TTLLive.Release();
			printf("Done!\n\r");

    }

		// We much balance a successful call to CoInitialize
		// by a call to CoUninitialize.
    PrintLine(MSG_CO_UNINITIALIZE);
    CoUninitialize();
		printf("Done!\n\r");
  } 

	::KillTimer(NULL,puiTimer);

	printf("\n\rExiting application...\n\r");
	return 0;
}

int _tmain(int argc, TCHAR* argv[], TCHAR* envp[])
{
	int nRetCode = 0;

	// initialize MFC and print and error on failure
	if (!AfxWinInit(::GetModuleHandle(NULL), NULL, ::GetCommandLine(), 0))
	{
		// TODO: change error code to suit your needs
		cerr << _T("Fatal Error: MFC initialization failed") << endl;
		nRetCode = 1;
	}
	else
	{
		nRetCode = my_main();
	}

	return nRetCode;
}