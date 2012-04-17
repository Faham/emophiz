using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophiz
{
	public class Signal
	{
		public enum SignalType
		{
			Unknown,
			GSR,
			EKGSmile,
			EKGFrown,
			HR,
			BVP,
		}

		public static SignalType SensorType2SignalType(SensorProvider.SensorType sensor)
		{
			switch (sensor)
			{
				case SensorProvider.SensorType.HR:
					return SignalType.HR;
				case SensorProvider.SensorType.GSR:
					return SignalType.GSR;
				case SensorProvider.SensorType.EKGSmile:
					return SignalType.EKGSmile;
				case SensorProvider.SensorType.EKGFrown:
					return SignalType.EKGFrown;
				default:
					return SignalType.Unknown;
			}
		}

		public string Name = "Unknown";
		public double Maximum = double.MinValue;
		public double Minimum = double.MaxValue;
		public double Highpass = double.MinValue;
		public double Lowpass = double.MaxValue;
		public double Shift = 0.0;

		private bool m_enableCalibrate = false;
		public bool EnableCalibrate 
		{
			get
			{
				return m_enableCalibrate;
			}
			set
			{
				m_enableCalibrate = value;
				if (m_enableCalibrate)
					resetMinMax();
			}
		}
		
		private void resetMinMax()
		{
			Maximum = double.MinValue;
			Minimum = double.MaxValue;
		}

		public bool EnableNormalize = false;
		public bool EnableShift = false;
		public bool EnableLowpass = false;
		public bool EnableHighpass = false;
		private double m_current = 0.0;
		private double m_previous = 0.0;
		private double m_transformed = 0.0;
		private SignalType m_type = SignalType.Unknown;

		public SignalType Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
				//TODO: you might want to reinitialize it.
			}
		}

		public double Previous { get { return m_previous; } }

		public double Current
		{
			set
			{
				m_previous = m_current;
				m_current = value;
				Transform();
			}
			get
			{
				return m_current;
			}
		}

		public double Transformed
		{
			get
			{
				return m_transformed;
			}
		}

		public Signal(string name = "Unknown", SignalType type = SignalType.Unknown)
		{
			Name = name;
			Type = type;
		}

		private void doCalibrate()
		{
			if (m_transformed < Minimum)
				Minimum = m_transformed;
			if (m_transformed > Maximum)
				Maximum = m_transformed;
		}

		private void doNormalize()
		{
			if (Maximum - Minimum > 0.00001) //first time, max would be equal to min
				m_transformed = (m_transformed - Minimum) * 100 / (Maximum - Minimum);
			else
				m_transformed = 0.0;

			if (m_transformed > 100.0)
				m_transformed = 100.0;
			else if (m_transformed < 0.0)
				m_transformed = 0.0;
		}

		private void doShift()
		{
			m_transformed += Shift;
		}

		private void doHighpass()
		{
			if (m_transformed <= Highpass)
				m_transformed = Highpass;
		}

		private void doLowpass()
		{
			if (m_transformed >= Lowpass)
				m_transformed = Lowpass;
		}

		public void Transform()
		{
			m_transformed = m_current;

			if (EnableCalibrate)
				doCalibrate();

			if (EnableShift)
				doShift();

			if (EnableHighpass)
				doHighpass();

			if (EnableLowpass)
				doLowpass();

			if (EnableNormalize)
				doNormalize();
		}
	}
}
