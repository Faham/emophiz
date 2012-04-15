// 10_Simplest.cpp : 

#if 1
#import "..\..\..\..\TTLLiveCtrl.dll" no_namespace named_guids
#else
#include "TTLLiveCtrl.tlh"
#endif

int main(int argc, char* argv[])
{	
	HRESULT hr;

	LONG liVersion;
	LONG liEncoderCount;
	
	hr = CoInitialize(NULL);
	if( SUCCEEDED(hr)){
		try {		
			
			// Declare and create an interface class object of ITTLLive type.
			//
			// Note that using the following delclaration type will generate
			// a COM exception if the object or interface cannot be created.		
			ITTLLivePtr TTLLive(CLSID_TTLLive);

			// If we get here, the ITTLLive interface pointer was loaded properly.			
			// Let's get version.
			liVersion = TTLLive->Version;

			// The EncoderCount property will return zero as no connection was established.
			liEncoderCount = TTLLive->EncoderCount;

		} catch( _com_error &e){
      HRESULT hr = e.Error();
			// ...
			// The e.Error() function returns the actual HRESULT.
			// The e.ErrorMessage() function returns a corresponding message.
			//..
		}

		// We much balance a successful call to CoInitialize
		// by a call to CoUninitialize.
    CoUninitialize();
	} else {
		// CoInitialize Failed!...
	}

	return 0;
}
