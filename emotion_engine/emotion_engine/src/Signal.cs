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
			EMGSmile,
			EMGFrown,
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
				case SensorProvider.SensorType.EMGSmile:
					return SignalType.EMGSmile;
				case SensorProvider.SensorType.EMGFrown:
					return SignalType.EMGFrown;
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
		public double NormalizeMinimum = 0.0;
		public double NormalizeMaximum = 100.0;
        private Log m_log;

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
		public bool EnableSmoothe = false;

		public int SmootheWindow = 4;
		private System.Collections.Queue m_history = new System.Collections.Queue();

		private double m_current = 0.0;
		private double m_previous = 0.0;
		private double m_transformed = 0.0;
        private double m_transformed_previous = 0.0;
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
				if (m_history.Count == SmootheWindow)
					m_history.Dequeue();
				m_history.Enqueue(m_current);
                m_transformed_previous = m_transformed;
				Transform();
                m_log.CSV(Log.Details.Short, Log.Priority.Information, m_current.ToString(), m_transformed.ToString());
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

		public double PreviousTransformed
		{
			get
			{
                return m_transformed_previous;
			}
		}
        
		public Signal(string name = "Unknown", SignalType type = SignalType.Unknown)
		{
			Name = name;
			Type = type;
            m_log = new Log(DateTime.Now.ToString(@"yyyy-MM-dd h-mm") +  "_" + name.ToLower() + ".csv");
            m_log.CSV(Log.Details.Raw, Log.Priority.Information, "time", "raw", "transformed");
		}

		private void doCalibrate()
		{
            /*/
            Minimum = -0.700f;
            Maximum = -0.600f;
            /*/
            if (m_transformed < Minimum)
				Minimum = m_transformed;
			if (m_transformed > Maximum)
				Maximum = m_transformed;
            //*/
		}

		private void doNormalize()
		{
			if (Maximum - Minimum > 0.00001) //first time, max would be equal to min
			{
				m_transformed = (m_transformed - Minimum) / (Maximum - Minimum);
				m_transformed = m_transformed * (NormalizeMaximum - NormalizeMinimum) + NormalizeMinimum;
			}
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

		private void doSmoothe()
		{
			m_transformed = 0;
			for (System.Collections.IEnumerator itr = m_history.GetEnumerator(); itr.MoveNext(); )
				m_transformed += (double)itr.Current;
			m_transformed /= m_history.Count;
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

			if (EnableSmoothe)
				doSmoothe();

			if (EnableNormalize)
				doNormalize();
		}

		public static string SerializationFormat()
		{
			return "Name\tCurrent\tTransformed\tMinimum\tMaximum\tShift";
		}

		public string Serialize()
		{
			string ret =
				String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}"
					, this.Name
					, this.Current
					, this.Transformed
					, this.Minimum
					, this.Maximum
					, this.Shift);

			return ret;
		}
	}
}
