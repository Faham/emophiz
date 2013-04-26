
#ifndef SENSOR_PROVIDER_PROXY__H_
#define SENSOR_PROVIDER_PROXY__H_

#include <vcclr.h>
#include <string>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

namespace emophiz {
	//__declspec(dllexport)
	class SensorProviderProxy
	{
	public:
		SensorProviderProxy();
		std::string test();

	private:
		gcroot<SensorProvider^> m_sensor_provider;
		gcroot<EmotionMonitor^> m_emotion_monitor;
	};
}

#endif // SENSOR_PROVIDER_PROXY__H_