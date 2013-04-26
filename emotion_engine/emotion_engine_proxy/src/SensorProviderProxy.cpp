
#include <SensorProviderProxy.h>
#include <iostream>
#include <metahost.h>

#using <System.dll>
#using <System.Windows.Forms.dll>

#pragma comment(lib, "mscoree.lib")

using namespace System;
using namespace msclr::interop;
using namespace System::Diagnostics;
using namespace System::Windows::Forms;

namespace emophiz {

	SensorProviderProxy::SensorProviderProxy() {
		m_emotion_monitor = gcnew EmotionMonitor();
		//m_emotion_monitor->Show();
		m_sensor_provider = SensorProvider::Instance;
		Application::Run(m_emotion_monitor);
	}
	
	std::string SensorProviderProxy::test() {
		return marshal_as<std::string>(m_sensor_provider->test());
	}

}

void main() {
	emophiz::SensorProviderProxy sens;
	std::cout << sens.test();
}
