using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophyz
{
	public enum Message
	{
		Connecting,
		Connected,
		Disconnected,
		SensorData,
		GSR,
		HR,
		EMGFrown,
		EMGSmile,
		Arousal,
		Valence
	}

	public interface ISensorListener
	{
		void OnMessage(Message msg, Object value);
	}
}
