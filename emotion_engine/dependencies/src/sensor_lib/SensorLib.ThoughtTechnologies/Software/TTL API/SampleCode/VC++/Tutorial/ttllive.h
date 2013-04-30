
// Set to '1' so that TTLLive wrapper classes get re-created.

#if 1

#ifdef USING_WRAPPER_CLASS
#import "..\..\..\TTLLiveCtrl.dll" no_namespace named_guids
#else

#import "..\..\..\..\TTLLiveCtrl.dll" no_namespace, raw_interfaces_only,\
                                      raw_native_types, named_guids
#endif

#else

#include "TTLLiveCtrl.tlh"

#endif

typedef enum _APP_MESSAGES 
{
	MSG_CO_INITIALIZE=0,
	MSG_CREATING_INSTANCE,
	MSG_QUERYING_TTLLIVE2,
	MSG_GETTING_VERSION,
  MSG_AUTODETECTING,
	MSG_CLOSE_CONNECTIONS,
	MSG_SETTING_UP_CHANNELS,
	MSG_RELEASING_INSTANCES,
	MSG_CO_UNINITIALIZE,
} APP_MESSAGES;

LPCTSTR Message( int nMessage )
{
	switch( nMessage ){
		case MSG_CO_INITIALIZE:
			return "Initializing COM libraries";

	  case MSG_CREATING_INSTANCE: 
		  return "Creating an instance of TTLLive object";

		case MSG_QUERYING_TTLLIVE2:
			return "Querying for the ITTLLive2 interface";

		case MSG_GETTING_VERSION:
			return "Getting TTLLiveCtrl.dll version";

		case MSG_AUTODETECTING:
			return "Auto-Detecting encoders";

		case MSG_CLOSE_CONNECTIONS:
			return "Closing all connections";

		case MSG_SETTING_UP_CHANNELS:
			return "Setting-Up Channels";

		case MSG_RELEASING_INSTANCES:
			return "Releasing interface instances";

		case MSG_CO_UNINITIALIZE:
			return "UnInitializing COM libraries";

		default:
			return NULL;
	}
}

void CheckHRESULT(HRESULT hr, char* str = NULL)
{
	if( SUCCEEDED(hr) ){
		if( str )printf("%s\n\r",str);
		else printf("Success!\n\r");
	}
	else printf("Failed...(0x%08X)\n\r",hr);
}

void PrintLine( const char* szFormat	, ...)
{
  char s[8*1024];

	int i,k,nWidth = 45;

  va_list arglist;

  va_start(arglist, szFormat);
  vsprintf( s, szFormat, arglist );

	k = strlen(s);

	for(i=k;i<nWidth;i++){
		s[i]='.';
	}
	s[i]=0;

	printf("%s",s);
}

void PrintLine( int nMessage )
{
	PrintLine(Message(nMessage));
}

typedef union T_Version {
  DWORD dwVersion;
  LONG liVersion;
  struct {
    WORD woBuild;
    BYTE byMinor;
    BYTE byMajor;
  };
} T_Version, *T_LPVersion;
