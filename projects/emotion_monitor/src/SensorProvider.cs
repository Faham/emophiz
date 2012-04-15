using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SensorLib.ThoughtTechnologies;

namespace emophyz
{
	public class SensorProvider
	{
		private bool m_connected = false;
		private static SensorProvider m_self = null;
		ITtlEncoder m_encoder = null;
		Dictionary<string, ITtlSensor> m_sensors = new Dictionary<string, ITtlSensor>();
		List<ISensorListener> m_listeners = new List<ISensorListener>();

		private double m_hr, m_gsr, m_ekg_frown, m_ekg_smile;
		private double m_arousal, m_valence;
		private double m_fun, m_boredom, m_excitement;

		public decimal GSR { get { return (decimal)m_gsr; } }
		public decimal HR { get { return (decimal)m_hr; } }
		public decimal EKGFrown { get { return (decimal)m_ekg_frown; } }
		public decimal EKGSmile { get { return (decimal)m_ekg_smile; } }
		public decimal Arousal { get { return (decimal)m_gsr; } }
		public decimal Valence { get { return (decimal)m_gsr; } }
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

		public SensorProvider()
		{
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

		public static SensorProvider getSingleton()
		{
			if (null == m_self)
				m_self = new SensorProvider();

			return m_self;
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
			m_fuzzyEngineArousal.Load("../../resources/fuzzy-engine-arousal.xml");
			m_fuzzyEngineValence.Load("../../resources/fuzzy-engine-valence.xml");
		}

		private double normalizeGSR(double val)
		{
			return val * 100;
		}

		private double normalizeHR(double val)
		{

			return val * 100;
		}

		private double normalizeEKGFrown(double val)
		{

			return val * 100;
		}

		private double normalizeEKGSmile(double val)
		{

			return val * 100;
		}

		private void sensor_DataAvailable(SensorLib.Sensors.ISensor<float> sensor, float[] data)
		{
			try
			{
				switch (sensorStrToType(sensor.Name))
				{
				case SensorType.HR:
					m_hr = normalizeGSR(sensor.CurrentValue);
					m_fuzzyEngineArousal.LinguisticVariableCollection.Find("HR").InputValue = m_hr;
					m_fuzzyEngineValence.LinguisticVariableCollection.Find("HR").InputValue = m_hr;
					break;
				case SensorType.GSR:
					m_gsr = normalizeHR(sensor.CurrentValue);
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

				m_valence = m_fuzzyEngineValence.Defuzzify();
				m_arousal = m_fuzzyEngineValence.Defuzzify();

				//InformListeners(Message.SensorData, sensor.CurrentValue);
				//InformListeners(Message.Valence, m_valence);
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.Message);
			}
		}

	}
}
