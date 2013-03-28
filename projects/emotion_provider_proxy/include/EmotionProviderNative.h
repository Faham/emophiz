// EmotionProviderProxy.h

#pragma once

#include "nativeAdapter\NativeProxy.h"
#include <string>

class SensorProviderNative
{
public:
	std::string test() { wrapper_(_T("test")); }

	SensorProviderNative(): wrapper_(_T("EmotionProvider.dll"), _T("emophiz.SensorProvider")) {}
	bool IsConnected() { wrapper_(_T("IsConnected")); }
	bool Connect() { wrapper_(_T("Connect")); }
	void Disconnect() { wrapper_(_T("Disconnect")); }
	//void AddListener(ISensorListener listener) { wrapper_("SensorProvider"); }
	void scaleEmotions() { wrapper_(_T("scaleEmotions")); }
private:
	nativeAdapter::NativeProxy wrapper_;
};
