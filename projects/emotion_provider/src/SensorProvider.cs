using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorLib.ThoughtTechnologies;

namespace emophiz
{
	public class SensorProvider
	{
		private bool m_connected = false;
		ITtlEncoder m_encoder = null;
		Dictionary<string, ITtlSensor> m_sensors = new Dictionary<string, ITtlSensor>();
		List<ISensorListener> m_listeners = new List<ISensorListener>();

		private double m_hr, m_gsr, m_ekg_frown, m_ekg_smile;
		private double m_hr_cur, m_gsr_cur, m_ekg_frown_cur, m_ekg_smile_cur;
		private double m_hr_min, m_gsr_min, m_ekg_frown_min, m_ekg_smile_min;
		private double m_hr_max, m_gsr_max, m_ekg_frown_max, m_ekg_smile_max;
		private double m_arousal, m_valence;
		private double m_fun, m_boredom, m_excitement;

		public decimal CurGSR { get { return (decimal)m_gsr_cur; } }
		public decimal MinGSR { get { return (decimal)m_gsr_min; } }
		public decimal MaxGSR { get { return (decimal)m_gsr_max; } }

		public decimal CurHR { get { return (decimal)m_hr_cur; } }
		public decimal MinHR { get { return (decimal)m_hr_min; } }
		public decimal MaxHR { get { return (decimal)m_hr_max; } }

		public decimal CurEKGFrown { get { return (decimal)m_ekg_frown_cur; } }
		public decimal MinEKGFrown { get { return (decimal)m_ekg_frown_min; } }
		public decimal MaxEKGFrown { get { return (decimal)m_ekg_frown_max; } }

		public decimal CurEKGSmile { get { return (decimal)m_ekg_smile_cur; } }
		public decimal MinEKGSmile { get { return (decimal)m_ekg_smile_min; } }
		public decimal MaxEKGSmile { get { return (decimal)m_ekg_smile_max; } }

		public decimal GSR { get { return (decimal)m_gsr; } }
		public decimal HR { get { return (decimal)m_hr; } }
		public decimal EKGFrown { get { return (decimal)m_ekg_frown; } }
		public decimal EKGSmile { get { return (decimal)m_ekg_smile; } }
		public decimal Arousal { get { return (decimal)m_arousal; } }
		public decimal Valence { get { return (decimal)m_valence; } }
		public decimal Fun { get { return (decimal)m_fun; } }
		public decimal Boredom { get { return (decimal)m_boredom; } }
		public decimal Excitement { get { return (decimal)m_excitement; } }

		//fuzzy variables
		DotFuzzy.FuzzyEngine m_fuzzyEngineArousal = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineValence = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineFun = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineBoredom = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineExcitement = new DotFuzzy.FuzzyEngine();

		public enum SensorType
		{
			Unknown = 0,
			HR,
			GSR,
			EMGSmile,
			EMGFrown,
			Count
		}

		private Log m_log;

		public SensorProvider(Log _log = null)
		{
			if (_log == null)
				m_log = new Log();
			else
				m_log = _log;

			m_hr_min = m_gsr_min = m_ekg_frown_min = m_ekg_smile_min = float.MaxValue;
			m_hr_max = m_gsr_max = m_ekg_frown_max = m_ekg_smile_max = float.MinValue;

			InitFuzzyEngines();
		}

		private static string[] SensorTypeStr = Enum.GetNames(typeof(SensorType));

		public static string sensorTypeToStr(SensorType st)
		{
			if (st >= SensorType.Count || st <= SensorType.Unknown)
				return SensorTypeStr[0];
			return SensorTypeStr[(uint)(st)];
		}

		public static SensorType sensorStrToType(string ss)
		{
			for (uint i = 1; i < (uint)SensorType.Count; ++i)
				if (SensorTypeStr[i] == ss)
					return (SensorType)i;
			return SensorType.Unknown;
		}

		public bool Connected
		{
			get
			{
				return m_connected;
			}
		}

		public bool Connect()
		{
			InformListeners(Message.Connecting, null);

			ITtlEncoderInfo[] encoderInfos = TtlEncoder.GetEncoders();

			if (encoderInfos.Count() < 1)
			{
				m_connected = false;
				InformListeners(Message.Disconnected, null);
				return false;
			}

			m_encoder = TtlEncoder.Connect(encoderInfos.First());
			m_connected = true;
			InformListeners(Message.Connected, null);

			string sensor_type = sensorTypeToStr(SensorType.HR);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, Channel.B, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();

			//SensorLib.Filters.RealTime.Normalizer m_filter_normalizer = new SensorLib.Filters.RealTime.Normalizer();

			sensor_type = sensorTypeToStr(SensorType.GSR);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, Channel.C, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			sensor_type = sensorTypeToStr(SensorType.EMGSmile);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, Channel.D, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			sensor_type = sensorTypeToStr(SensorType.EMGFrown);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, Channel.E, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type] .Start();
			
			return true;
		}

		public void Disconnect()
		{
			if (null == m_encoder)
				return;

			for (Dictionary<string, ITtlSensor>.Enumerator itr = m_sensors.GetEnumerator(); itr.MoveNext(); )
				itr.Current.Value.Stop();

			m_encoder.Dispose();
			m_sensors.Clear();
			m_encoder = null;
			m_connected = false;
			InformListeners(Message.Disconnected, null);
		}

		public void AddListener(ISensorListener listener)
		{
			m_listeners.Add(listener);
		}

		private void InformListeners(Message msg, Object value)
		{
			foreach (ISensorListener lsn in m_listeners)
				lsn.OnMessage(msg, value);
		}

		private void InitFuzzyEngines()
		{
			m_log.Message("Initializing fuzzy engines");
			m_fuzzyEngineArousal.Load("../../resources/fuzzy-engine-arousal.xml");
			m_fuzzyEngineValence.Load("../../resources/fuzzy-engine-valence.xml");
			m_fuzzyEngineFun.Load("../../resources/fuzzy-engine-fun.xml");
			m_fuzzyEngineExcitement.Load("../../resources/fuzzy-engine-excitement.xml");
			m_fuzzyEngineBoredom.Load("../../resources/fuzzy-engine-boredom.xml");
			m_log.Message("Fuzzy engines initialized");
		}

		private double normalizeGSR(double val)
		{
			m_gsr_cur = val;
			if (val < m_gsr_min)
				m_gsr_min = val;
			if (val > m_gsr_max)
				m_gsr_max = val;

			if (m_gsr_max - m_gsr_min < 0.001f)
				return 0.0f;
			else
				return (val - m_gsr_min) * 100 / (m_gsr_max - m_gsr_min);
		}

		private double normalizeHR(double val)
		{
			m_hr_cur = val;
			if (val < m_hr_min)
				m_hr_min = val;
			if (val > m_hr_max)
				m_hr_max = val;

			if (m_hr_max - m_hr_min < 0.001f)
				return 0.0f;
			else
				return (val - m_hr_min) * 100 / (m_hr_max - m_hr_min);
		}

		private double normalizeEKGFrown(double val)
		{
			m_ekg_frown_cur = val;
			if (val < m_ekg_frown_min)
				m_ekg_frown_min = val;
			if (val > m_ekg_frown_max)
				m_ekg_frown_max = val;

			if (m_ekg_frown_max - m_ekg_frown_min < 0.001f)
				return 0.0f;
			else
				return (val - m_ekg_frown_min) * 100 / (m_ekg_frown_max - m_ekg_frown_min);
		}

		private double normalizeEKGSmile(double val)
		{
			m_ekg_smile_cur = val;
			if (val < m_ekg_smile_min)
				m_ekg_smile_min = val;
			if (val > m_ekg_smile_max)
				m_ekg_smile_max = val;

			if (m_ekg_smile_max - m_ekg_smile_min < 0.001f)
				return 0.0f;
			else
				return (val - m_ekg_smile_min) * 100 / (m_ekg_smile_max - m_ekg_smile_min);
		}

		private void sensor_DataAvailable(SensorLib.Sensors.ISensor<float> sensor, float[] data)
		{
			try
			{
				switch (sensorStrToType(sensor.Name))
				{
				case SensorType.HR:
					m_hr = normalizeHR(sensor.CurrentValue);
					m_fuzzyEngineArousal.LinguisticVariableCollection.Find("HR").InputValue = m_hr;
					m_fuzzyEngineValence.LinguisticVariableCollection.Find("HR").InputValue = m_hr;
					break;
				case SensorType.GSR:
					m_gsr = normalizeGSR(sensor.CurrentValue);
					m_fuzzyEngineArousal.LinguisticVariableCollection.Find("GSR").InputValue = m_gsr;
					break;
				case SensorType.EMGSmile:
					m_ekg_smile = normalizeEKGSmile(sensor.CurrentValue);
					m_fuzzyEngineValence.LinguisticVariableCollection.Find("EMGsmile").InputValue = m_ekg_smile;
					break;
				case SensorType.EMGFrown:
					m_ekg_frown = normalizeEKGFrown(sensor.CurrentValue);
					m_fuzzyEngineValence.LinguisticVariableCollection.Find("EMGfrown").InputValue = m_ekg_frown;
					break;
				}

				// Phase 1
				m_valence = m_fuzzyEngineValence.Defuzzify();
				m_arousal = m_fuzzyEngineArousal.Defuzzify();

				// Phase 2
				m_fuzzyEngineFun.LinguisticVariableCollection.Find("Valence").InputValue = m_valence;
				m_fuzzyEngineFun.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal;
				m_fun = m_fuzzyEngineFun.Defuzzify();

				m_fuzzyEngineExcitement.LinguisticVariableCollection.Find("Valence").InputValue = m_valence;
				m_fuzzyEngineExcitement.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal;
				m_excitement = m_fuzzyEngineExcitement.Defuzzify();

				m_fuzzyEngineBoredom.LinguisticVariableCollection.Find("Valence").InputValue = m_valence;
				m_fuzzyEngineBoredom.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal;
				m_boredom = m_fuzzyEngineBoredom.Defuzzify();
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.Message);
			}
		}

	}
}
