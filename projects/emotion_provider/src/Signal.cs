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

		public enum Operation : byte
		{
			None = 0,
			Normalize = 1,
			Shift = 1 << 1,
			Lowpass = 1 << 2,
			Highpass = 1 << 3,
		}

		public string Name = "Unknown";
		public double Maximum = double.MinValue;
		public double Minimum = double.MaxValue;
		public double Highpass = double.MinValue;
		public double Lowpass = double.MaxValue;
		public double Shift = 0.0;

		public byte Operations;
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

		private void doNormalize()
		{
			if (m_transformed < Minimum)
				Minimum = m_transformed;
			if (m_transformed > Maximum)
				Maximum = m_transformed;

			if (Maximum - Minimum > 0.00001) //first time, max would be equal to min
				m_transformed = (m_transformed - Minimum) * 100 / (Maximum - Minimum);
			else
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

			if ((Operations & (byte)Operation.Shift) != 0)
				doShift();

			if ((Operations & (byte)Operation.Highpass) != 0)
				doHighpass();

			if ((Operations & (byte)Operation.Lowpass) != 0)
				doLowpass();

			if ((Operations & (byte)Operation.Normalize) != 0)
				doNormalize();
		}
	}
}
