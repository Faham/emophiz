﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophiz
{
	public class SensorProvider
	{
		private bool m_connected = false;
		private SensorLib.ThoughtTechnologies.ITtlEncoder m_encoder = null;
		private Dictionary<string, SensorLib.ThoughtTechnologies.ITtlSensor> m_sensors = new Dictionary<string, SensorLib.ThoughtTechnologies.ITtlSensor>();
		private List<ISensorListener> m_listeners = new List<ISensorListener>();
		private Dictionary<string, Signal> m_signals = new Dictionary<string, Signal>();

		private double m_arousal, m_valence;
		private double m_fun, m_boredom, m_excitement;

		public Signal GSR { get { return m_signals[sensorTypeToStr(SensorType.GSR)]; } }
		public Signal HR { get { return m_signals[sensorTypeToStr(SensorType.HR)]; } }
		public Signal BVP { get { return m_signals[sensorTypeToStr(SensorType.BVP)]; } }
		public Signal EKGFrown { get { return m_signals[sensorTypeToStr(SensorType.EKGFrown)]; } }
		public Signal EKGSmile { get { return m_signals[sensorTypeToStr(SensorType.EKGSmile)]; } }

		public double Arousal { get { return m_arousal; } }
		public double Valence { get { return m_valence; } }
		public double Fun { get { return m_fun; } }
		public double Boredom { get { return m_boredom; } }
		public double Excitement { get { return m_excitement; } }

		//fuzzy variables
		DotFuzzy.FuzzyEngine m_fuzzyEngineArousal = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineValence = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineFun = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineBoredom = new DotFuzzy.FuzzyEngine();
		DotFuzzy.FuzzyEngine m_fuzzyEngineExcitement = new DotFuzzy.FuzzyEngine();

		System.DateTime m_last_heartbeat_time = System.DateTime.Now;
		System.DateTime m_current_heartbeat_time = System.DateTime.Now;
		double m_last_rise;
		bool m_derivative_change = false;
		Signal m_bvp;

		public enum SensorType
		{
			Unknown = 0,
			BVP,
			HR,
			GSR,
			EKGSmile,
			EKGFrown,
			Count
		}

		private Log m_log;

		public SensorProvider(Log _log = null)
		{
			if (_log == null)
				m_log = new Log();
			else
				m_log = _log;

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

		private SensorLib.Filters.RealTime.LinearFilter m_filterBaselineRemover;

		public bool Connect()
		{
			InformListeners(Message.Connecting, null);

			SensorLib.ThoughtTechnologies.ITtlEncoderInfo[] encoderInfos = SensorLib.ThoughtTechnologies.TtlEncoder.GetEncoders();

			if (encoderInfos.Count() < 1)
			{
				m_connected = false;
				InformListeners(Message.Disconnected, null);
				return false;
			}

			m_encoder = SensorLib.ThoughtTechnologies.TtlEncoder.Connect(encoderInfos.First());
			m_connected = true;
			InformListeners(Message.Connected, null);

			string sensor_type = sensorTypeToStr(SensorType.BVP);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.B, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.BVP);
			m_signals[sensor_type].EnableNormalize = true;

			sensor_type = sensorTypeToStr(SensorType.HR);
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.HR);
			m_signals[sensor_type].EnableNormalize = true;

			m_bvp = m_signals[sensorTypeToStr(SensorType.BVP)];

			////////how to work with filters.
			SensorLib.Filters.FilterOrderSpec spec = new SensorLib.Filters.FilterOrderSpec();
			spec.BandType = SensorLib.Filters.BandType.HighPass;
			spec.FilterType = SensorLib.Filters.IirFilterType.Butterworth;
			spec.CornerFreqs = new SensorLib.Util.Pair<double, double>(System.Math.PI / 16.0, 0.0);
			spec.Order = 8;
			m_filterBaselineRemover = SensorLib.Filters.FilterCreation.FilterFactory.CreateIirFilter(spec);
			/////////

			sensor_type = sensorTypeToStr(SensorType.GSR);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.C, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.GSR);
			m_signals[sensor_type].EnableNormalize = true;

			sensor_type = sensorTypeToStr(SensorType.EKGSmile);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.D, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.EKGSmile);
			m_signals[sensor_type].EnableNormalize = true;

			sensor_type = sensorTypeToStr(SensorType.EKGFrown);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.E, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type] .Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.EKGFrown);
			m_signals[sensor_type].EnableNormalize = true;
			
			return true;
		}

		public void Disconnect()
		{
			if (null == m_encoder)
				return;

			for (Dictionary<string, SensorLib.ThoughtTechnologies.ITtlSensor>.Enumerator itr = m_sensors.GetEnumerator(); itr.MoveNext(); )
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

		private void UpdateHR()
		{
			double _current_rise = m_bvp.Current - m_bvp.Previous;
			if (m_last_rise * _current_rise < 0)
			{
				if (!m_derivative_change)
					m_derivative_change = true;
				else
				{
					//One HeartBeat!
					m_derivative_change = false;
					m_last_heartbeat_time = m_current_heartbeat_time;
					m_current_heartbeat_time = System.DateTime.Now;
					System.TimeSpan delta_time = m_current_heartbeat_time - m_last_heartbeat_time;
					m_signals[sensorTypeToStr(SensorType.HR)].Current = 60000.0 / delta_time.TotalMilliseconds;
				}
			}
			m_last_rise = _current_rise;
		}

		private void sensor_DataAvailable(SensorLib.Sensors.ISensor<float> sensor, float[] data)
		{
			try
			{
				Signal signal;
				if (!m_signals.TryGetValue(sensor.Name, out signal))
					throw new Exception("This type of sensor isn't supported: " + sensor.Name);
				signal.Current = sensor.CurrentValue;

				if (sensorStrToType(sensor.Name) == SensorType.BVP)
					UpdateHR();

				DotFuzzy.LinguisticVariable var = m_fuzzyEngineArousal.LinguisticVariableCollection.Find(signal.Name);
				if (var != null)
					var.InputValue = signal.Transformed;

				var = m_fuzzyEngineValence.LinguisticVariableCollection.Find(signal.Name);
				if (var != null)
					var.InputValue = signal.Transformed;

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
