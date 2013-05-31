
//==============================================================================

#include <CEmotionEngine.h>

#include <vcclr.h>
#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>
#include <windows.h>

#include <string>
#include <iostream>

#using <System.dll>
#using <System.Windows.Forms.dll>

#pragma comment(lib, "mscoree.lib")

//==============================================================================

using namespace System;
using namespace msclr::interop;
using namespace System::Diagnostics;
using namespace System::Windows::Forms;
using namespace System::Threading;

// for str conversion: marshal_as<std::string>(.Net String);

//==============================================================================

namespace emophiz {

//------------------------------------------------------------------------------

	void messageLoop() {
		EmotionMonitor^ _emotion_monitor = gcnew EmotionMonitor();
		gcroot<EmotionMonitor^> *_ptr_emotion_monitor = new gcroot<EmotionMonitor^>(_emotion_monitor);
		void *m_emotion_monitor = (void*)_ptr_emotion_monitor;
		((EmotionMonitor^)*_ptr_emotion_monitor)->Show();
		
		SensorProvider^ _sensor_provider = SensorProvider::Instance;
		gcroot<SensorProvider^> *_ptr_sensor_provider = new gcroot<SensorProvider^>(_sensor_provider);
		void *m_sensor_provider = (void*)_ptr_sensor_provider;
		//*/

		Application::Run(((EmotionMonitor^)*_ptr_emotion_monitor));
	}

//------------------------------------------------------------------------------

	CEmotionEngine::CEmotionEngine():
		m_sensor_provider(NULL) {

		ThreadStart^ trdDlgt = gcnew ThreadStart(&emophiz::messageLoop);
		Thread^ _back_thread = gcnew Thread(trdDlgt);
		gcroot<Thread^> *_ptr_back_thread = new gcroot<Thread^>(_back_thread);
		m_background_thread = (void*)_ptr_back_thread;
		((Thread^)*_ptr_back_thread)->IsBackground = true;
	}

//------------------------------------------------------------------------------

	CEmotionEngine::~CEmotionEngine() {
		if (m_sensor_provider) {
			gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
			m_sensor_provider = NULL;
			delete pp;
		}

		if (m_emotion_monitor) {
			gcroot<EmotionMonitor^> *pp = reinterpret_cast<gcroot<EmotionMonitor^>*>(m_emotion_monitor);
			m_emotion_monitor = NULL;
			delete pp;
		}
	}

//------------------------------------------------------------------------------

	bool CEmotionEngine::start() {
		gcroot<Thread^> *pp = reinterpret_cast<gcroot<Thread^>*>(m_background_thread);
		((Thread^)*pp)->Start();

		return true;
	}

//------------------------------------------------------------------------------

	bool CEmotionEngine::connect() {
		SensorProvider^ _sensor_provider = SensorProvider::Instance;
		gcroot<SensorProvider^> *_ptr_sensor_provider = new gcroot<SensorProvider^>(_sensor_provider);
		m_sensor_provider = (void*)_ptr_sensor_provider;

		return true;
	}

//------------------------------------------------------------------------------

	bool CEmotionEngine::isConnected () {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
		return ((SensorProvider^)*pp)->IsConnected();
	}

//------------------------------------------------------------------------------

	void CEmotionEngine::calibrateGSR(bool b) {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
		((SensorProvider^)*pp)->GSR->EnableCalibrate = b;
	}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readArousal(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->Arousal->Current;
	//	else
	//		return ((SensorProvider^)*pp)->Arousal->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readValence(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->Valence->Current;
	//	else
	//		return ((SensorProvider^)*pp)->Valence->Transformed;
	//}

//------------------------------------------------------------------------------

	double CEmotionEngine::readGSR(bool raw /*= false*/) {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);

		if (raw)
			return ((SensorProvider^)*pp)->GSR->Current;
		else
			return ((SensorProvider^)*pp)->GSR->Transformed;
	}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readHR(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->HR->Current;
	//	else
	//		return ((SensorProvider^)*pp)->HR->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readBVP(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->BVP->Current;
	//	else
	//		return ((SensorProvider^)*pp)->BVP->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readEMGFrown(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->EMGFrown->Current;
	//	else
	//		return ((SensorProvider^)*pp)->EMGFrown->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readEMGSmile(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->EMGFrown->Current;
	//	else
	//		return ((SensorProvider^)*pp)->EMGFrown->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readFun(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->Fun->Current;
	//	else
	//		return ((SensorProvider^)*pp)->Fun->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readBoredom(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->Boredom->Current;
	//	else
	//		return ((SensorProvider^)*pp)->Boredom->Transformed;
	//}

//------------------------------------------------------------------------------

	//double CEmotionEngine::readExcitement(bool raw /*= false*/) {
	//	gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
	//
	//	if (raw)
	//		return ((SensorProvider^)*pp)->Excitement->Current;
	//	else
	//		return ((SensorProvider^)*pp)->Excitement->Transformed;
	//}

//------------------------------------------------------------------------------

	void CEmotionEngine::logGameEvent(int optcode) {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
		((SensorProvider^)*pp)->logGameEvent(optcode);
	}

	void CEmotionEngine::logGameEvent(int optcode, float v) {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
		((SensorProvider^)*pp)->logGameEvent(optcode, v);
	}

	void CEmotionEngine::logGameEvent(int optcode, float v1, float v2) {
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);
		((SensorProvider^)*pp)->logGameEvent(optcode, v1, v2);
	}

//------------------------------------------------------------------------------

	/*void CEmotionEngine::logGameEvent(int optcode) {
	
		int n = 0;
		switch(optcode) {
		case 2:
		case 7:
		case 8:
			n = 0;
		case 3:
			n = 2;
		default:
			n = 1;
		}
		
		gcroot<SensorProvider^> *pp = reinterpret_cast<gcroot<SensorProvider^>*>(m_sensor_provider);

		if (n > 0) {
			float[] values = new float[n];

			va_list argptr;
			va_start(argptr, optcode);
			for (int i = 0; i < n; ++i)
				values[i] = va_arg(argptr, float);
			va_end(argptr);

			((SensorProvider^)*pp)->logGameEvent(optcode, values);

			delete [] values;
		} else {
			((SensorProvider^)*pp)->logGameEvent(optcode);
		}
	}*/

//------------------------------------------------------------------------------

}

//==============================================================================

/*
void main() {
	emophiz::CEmotionEngine sens;
	sens.start();

	int i = 1000000;
	while (i > 0)
		Sleep(1000);
}

//*/

//==============================================================================
