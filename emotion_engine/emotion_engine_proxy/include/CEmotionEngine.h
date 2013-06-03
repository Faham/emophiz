
//==============================================================================

#ifndef EMOTIONENGINE__H_
#define EMOTIONENGINE__H_

//==============================================================================

#ifdef NATIVEDLL_EXPORTS
   #define NATIVEDLL_API __declspec(dllexport)
#else
   #define NATIVEDLL_API __declspec(dllimport)
#endif

//==============================================================================

namespace emophiz {

	class NATIVEDLL_API CEmotionEngine
	{
	public:
		CEmotionEngine();
		~CEmotionEngine();

		bool start();
		bool connect();
		bool isConnected();
		void calibrateGSR(bool b);
		//double readArousal   (bool raw = false);
		//double readValence   (bool raw = false);
		double readGSR       (bool raw = false);
		//double readHR        (bool raw = false);
		//double readBVP       (bool raw = false);
		//double readEMGFrown  (bool raw = false);
		//double readEMGSmile  (bool raw = false);
		//double readFun       (bool raw = false);
		//double readBoredom   (bool raw = false);
		//double readExcitement(bool raw = false);
		void logGameEvent(int optcode);
		void logGameEvent(int optcode, float v);
		void logGameEvent(int optcode, float v1, float v2);

	private:
		void* m_sensor_provider;
		void* m_emotion_monitor;
		void* m_background_thread;
	};
}

//==============================================================================

#endif // EMOTIONENGINE__H_

//==============================================================================

