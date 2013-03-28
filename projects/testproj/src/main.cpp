
#include "nativeAdapter/NativeProxy.h"
#include "nativeAdapter/NativeProxyStatic.h"
#include "nativeTools/anyType.h"

#include <iostream>

class SensorProviderNative
{
public:
    SensorProviderNative() : wrapper_(_T("EmotionProvider.dll"), _T("emophiz.SensorProvider")) {}
	std::string test() { wrapper_(_T("test")); }

private:
	nativeAdapter::NativeProxy wrapper_;
};

void main() {
	SensorProviderNative spn;
	std::cout << "test";
}
