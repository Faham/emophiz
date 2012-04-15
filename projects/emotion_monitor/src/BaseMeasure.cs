using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophyz
{
	public enum SensorState
	{
		Unknown = 0,
		VeryLow,
		Low,
		MediumLow,
		Medium,
		MediumHigh,
		High,
		VeryHigh,
		Neutral
	}

	public class BaseMeasure
	{
		private SensorState m_state = SensorState.Unknown;
		public SensorState State
		{
			get { return m_state; }
			set { m_state = value; }
		}

		private float m_value;
		private float m_max_value;
		public float Value
		{
			get { return m_value; }
			set
			{
				if (value > m_max_value) m_max_value = value;
				m_value = value / m_max_value;
			}
		}
	}

	public class BaseSensorMeasure : BaseMeasure
	{
		public void updateState(SensorProvider.SensorType st)
		{
			switch (st)
			{
			case SensorProvider.SensorType.HR:
				if (Value > 75) State = SensorState.High;
				else if (Value >= 35 && Value <= 75) State = SensorState.Medium;
				else if (Value < 35) State = SensorState.Low;
				break;
			case SensorProvider.SensorType.GSR:
				if (Value > 75) State = SensorState.High;
				else if (Value >= 35 && Value <= 75) State = SensorState.Medium;
				else if (Value < 35) State = SensorState.Low;
				break;
			case SensorProvider.SensorType.EMGSmile:
				break;
			case SensorProvider.SensorType.EMGFrown:
				break;
			}
		}
	}
}
