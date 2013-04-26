using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophiz
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
