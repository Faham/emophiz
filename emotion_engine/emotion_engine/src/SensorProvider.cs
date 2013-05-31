using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace emophiz
{
	public class SensorProvider
	{
        private static SensorProvider ms_instance;
        public static SensorProvider Instance
        {
            get
            {
                if (ms_instance == null)
                    throw new System.InvalidOperationException("No instance created!");
                else
                    return ms_instance;
            }
        }


        public string test() { return "this is a test"; }

		private bool m_connected = false;
		private SensorLib.ThoughtTechnologies.ITtlEncoder m_encoder = null;
		private Dictionary<string, SensorLib.ThoughtTechnologies.ITtlSensor> m_sensors = new Dictionary<string, SensorLib.ThoughtTechnologies.ITtlSensor>();
		private List<ISensorListener> m_listeners = new List<ISensorListener>();
		private Dictionary<string, Signal> m_signals = new Dictionary<string, Signal>();

		//private Signal m_arousal, m_valence;
		//private Signal m_fun, m_boredom, m_excitement;

		public SensorLib.ThoughtTechnologies.ITtlEncoder Encoder { get { return m_encoder; } }

		public Signal GSR { get { return m_signals[sensorTypeToStr(SensorType.GSR)]; } }
		//public Signal HR { get { return m_signals[sensorTypeToStr(SensorType.HR)]; } }
		//public Signal BVP { get { return m_signals[sensorTypeToStr(SensorType.BVP)]; } }
		//public Signal EMGFrown { get { return m_signals[sensorTypeToStr(SensorType.EMGFrown)]; } }
		//public Signal EMGSmile { get { return m_signals[sensorTypeToStr(SensorType.EMGSmile)]; } }

		//public Signal Arousal { get { return m_arousal; } }
		//public Signal Valence { get { return m_valence; } }
		//public Signal Fun { get { return m_fun; } }
		//public Signal Boredom { get { return m_boredom; } }
		//public Signal Excitement { get { return m_excitement; } }

		//fuzzy variables
		//DotFuzzy.FuzzyEngine m_fuzzyEngineArousal = new DotFuzzy.FuzzyEngine();
		//DotFuzzy.FuzzyEngine m_fuzzyEngineValence = new DotFuzzy.FuzzyEngine();
		//DotFuzzy.FuzzyEngine m_fuzzyEngineFun = new DotFuzzy.FuzzyEngine();
		//DotFuzzy.FuzzyEngine m_fuzzyEngineBoredom = new DotFuzzy.FuzzyEngine();
		//DotFuzzy.FuzzyEngine m_fuzzyEngineExcitement = new DotFuzzy.FuzzyEngine();

		//double m_last_heartbeat_time = 0;
		//double m_current_heartbeat_time = 0;
		//double m_last_rise;
		//bool m_derivative_change = false;
		//Signal m_bvp;

		public enum SensorType
		{
			Unknown = 0,
			BVP,
			HR,
			GSR,
			EMGSmile,
			EMGFrown,
			Count
		}

		private Log m_log;
        //private Log m_log_signals;
        private Log m_log_game;

        public SensorProvider()
        {
            ms_instance = this;
            init();
        }

        public SensorProvider(Log log, String fuzzy_resources)
        {
            ms_instance = this;
            init(log, fuzzy_resources);
        }

        private void init(Log log = null, String fuzzy_resources = "resources/")
        {
            if (log == null)
                m_log = new Log("sensor_provider.log");
            else
                m_log = log;

            //m_log_signals = new Log("sensor_provider.csv");
            m_log_game = new Log("game_events.csv");
            //m_log_signals.CSV(Log.Details.Raw,
            //    Log.Priority.Information,
            //    "time",
            //    "gsr_raw",
            //    "gsr_transformed",
            //    "hr_raw",
            //    "hr_transformed",
            //    "bvp_raw",
            //    "bvp_transformed",
            //    "emgfrown_raw",
            //    "emgfrown_transformed",
            //    "emgsmile_raw",
            //    "emgsmile_transformed",
            //    "arousal_raw",
            //    "arousal_transformed",
            //    "valence_raw",
            //    "valence_transformed",
            //    "fun_raw",
            //    "fun_transformed",
            //    "excitement_raw",
            //    "excitement_transformed",
            //    "boredom_raw",
            //    "boredom_transformed"
            //);


			//m_fuzzyResources = fuzzy_resources;
			//InitFuzzyEngines();
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

        public bool IsConnected() {
            return m_connected;
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
            string sensor_type;

			m_log.Message("Creating signals");
            /*
			sensor_type = sensorTypeToStr(SensorType.BVP);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.A, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.BVP);
			m_signals[sensor_type].EnableNormalize = true;
            m_signals[sensor_type].EnableSmoothe = true;
            m_signals[sensor_type].SmootheWindow = 256 * 1;
            m_bvp = m_signals[sensor_type];

			sensor_type = sensorTypeToStr(SensorType.HR);
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.HR);
			m_signals[sensor_type].EnableNormalize = true;
			m_signals[sensor_type].EnableSmoothe = true;
            m_signals[sensor_type].SmootheWindow = 256 * 5;

			////////how to work with filters.
			SensorLib.Filters.FilterOrderSpec spec = new SensorLib.Filters.FilterOrderSpec();
			spec.BandType = SensorLib.Filters.BandType.HighPass;
			spec.FilterType = SensorLib.Filters.IirFilterType.Butterworth;
			spec.CornerFreqs = new SensorLib.Util.Pair<double, double>(System.Math.PI / 16.0, 0.0);
			spec.Order = 8;
			m_filterBaselineRemover = SensorLib.Filters.FilterCreation.FilterFactory.CreateIirFilter(spec);
			/////////
            */
			sensor_type = sensorTypeToStr(SensorType.GSR);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.C, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.GSR);
			m_signals[sensor_type].EnableNormalize = true;
			m_signals[sensor_type].EnableSmoothe = true;
			m_signals[sensor_type].SmootheWindow = 32 * 1; //frequecy * second
            /*
			sensor_type = sensorTypeToStr(SensorType.EMGSmile);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.D, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type].Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.EMGSmile);
			m_signals[sensor_type].EnableNormalize = true;
			m_signals[sensor_type].EnableSmoothe = true;
			//m_signals[sensor_type].SmootheWindow = 32 * 4;

			sensor_type = sensorTypeToStr(SensorType.EMGFrown);
			m_sensors[sensor_type] = m_encoder.CreateSensor(sensor_type, SensorLib.ThoughtTechnologies.SensorType.Raw, SensorLib.ThoughtTechnologies.Channel.E, false);
			m_sensors[sensor_type].DataAvailable += new SensorLib.Sensors.DataAvailableHandler<float>(sensor_DataAvailable);
			m_sensors[sensor_type] .Start();
			m_signals[sensor_type] = new Signal(sensor_type, Signal.SignalType.EMGFrown);
			m_signals[sensor_type].EnableNormalize = true;
			m_signals[sensor_type].EnableSmoothe = true;
			//m_signals[sensor_type].SmootheWindow = 32 * 4;

			m_arousal = new Signal("Arousal");
			m_arousal.Minimum = 0;
			m_arousal.Maximum = 100;
			m_valence = new Signal("Valence");
			m_valence.Minimum = 0;
			m_valence.Maximum = 100;
			m_fun = new Signal("Fun");
			m_fun.Minimum = 0;
			m_fun.Maximum = 100;
			//m_fun.EnableNormalize = true;
			m_excitement = new Signal("Excitement");
			m_excitement.Minimum = 0;
			m_excitement.Maximum = 100;
			//m_excitement.EnableNormalize = true;
			m_boredom = new Signal("Boredom");
			m_boredom.Minimum = 0;
			m_boredom.Maximum = 100;
			//m_boredom.EnableNormalize = true;
            */
			m_log.Message("Signals created.");
			m_log.Message("Signal serialization format: " + Signal.SerializationFormat());

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
        /*
		private string m_fuzzyResources;

		private void InitFuzzyEngines()
		{
			m_log.Message("Initializing fuzzy engines");
			m_fuzzyEngineArousal.Load(m_fuzzyResources + "fuzzy-engine-arousal.xml");
			m_fuzzyEngineValence.Load(m_fuzzyResources + "fuzzy-engine-valence.xml");
			m_fuzzyEngineFun.Load(m_fuzzyResources + "fuzzy-engine-fun.xml");
			m_fuzzyEngineExcitement.Load(m_fuzzyResources + "fuzzy-engine-excitement.xml");
			m_fuzzyEngineBoredom.Load(m_fuzzyResources + "fuzzy-engine-boredom.xml");
			m_log.Message("Fuzzy engines initialized");
		}

		private void UpdateHR()
		{
			double _current_rise = m_bvp.Transformed - m_bvp.PreviousTransformed;
            
            if (_current_rise == 0)
                return;

            if (m_last_rise == 0) {
                m_last_rise = _current_rise;
                return;
            }

            if (m_last_rise * _current_rise < 0 && !m_derivative_change)
			{
                m_derivative_change = true;
                m_last_rise = _current_rise;
            }
            else if (m_last_rise * _current_rise > 0 && m_derivative_change)
            {
                //One HeartBeat!
                double now = System.TimeSpan.FromTicks(System.DateTime.Now.Ticks).TotalMilliseconds;
                if (m_last_heartbeat_time == 0 && m_current_heartbeat_time == 0)
                {
                    m_last_heartbeat_time = now;
                    m_current_heartbeat_time = now;
                }
                else
                {
                    double delta_time = now - m_last_heartbeat_time;
                    double temp_beat_rate = 60000.0 / delta_time;

                    //if (temp_beat_rate < 110 && temp_beat_rate > 40)
                    {
                        m_derivative_change = false;
                        m_last_heartbeat_time = m_current_heartbeat_time;
                        m_current_heartbeat_time = now;
                        m_signals[sensorTypeToStr(SensorType.HR)].Current = temp_beat_rate;
                        m_last_rise = _current_rise;
                    }
                }
            }
		}
        */
		private void sensor_DataAvailable(SensorLib.Sensors.ISensor<float> sensor, float[] data)
		{
			try
			{
				Signal signal;
				if (!m_signals.TryGetValue(sensor.Name, out signal))
					throw new Exception("This type of sensor isn't supported: " + sensor.Name);

                signal.Current = sensor.CurrentValue;
                /*

                if (sensorStrToType(sensor.Name) == SensorType.BVP)
                    UpdateHR();

                DotFuzzy.LinguisticVariable var = m_fuzzyEngineArousal.LinguisticVariableCollection.Find(signal.Name);
                if (var != null)
                    var.InputValue = signal.Transformed;

                var = m_fuzzyEngineValence.LinguisticVariableCollection.Find(signal.Name);
                if (var != null)
                    var.InputValue = signal.Transformed;

                // Phase 1
                m_valence.Current = m_fuzzyEngineValence.Defuzzify();
                m_arousal.Current = m_fuzzyEngineArousal.Defuzzify();

                if (Double.IsNaN(m_valence.Current))
                    m_valence.Current = 0;
                if (Double.IsNaN(m_arousal.Current))
                    m_arousal.Current = 0;

                // Phase 2
                m_fuzzyEngineFun.LinguisticVariableCollection.Find("Valence").InputValue = m_valence.Current;
                m_fuzzyEngineFun.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal.Current;
                m_fun.Current = m_fuzzyEngineFun.Defuzzify();

                if (Double.IsNaN(m_fun.Current))
                    m_fun.Current = 0;

                m_fuzzyEngineExcitement.LinguisticVariableCollection.Find("Valence").InputValue = m_valence.Current;
                m_fuzzyEngineExcitement.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal.Current;
                m_excitement.Current = m_fuzzyEngineExcitement.Defuzzify();

                if (Double.IsNaN(m_excitement.Current))
                    m_excitement.Current = 0;

                m_fuzzyEngineBoredom.LinguisticVariableCollection.Find("Valence").InputValue = m_valence.Current;
                m_fuzzyEngineBoredom.LinguisticVariableCollection.Find("Arousal").InputValue = m_arousal.Current;
                m_boredom.Current = m_fuzzyEngineBoredom.Defuzzify();

                if (Double.IsNaN(m_boredom.Current))
                    m_boredom.Current = 0;
                */
                //m_log_signals.CSV(Log.Details.Short,
                //    Log.Priority.Information,
                //    GSR.Current.ToString(),
                //    GSR.Transformed.ToString(),
                //    HR.Current.ToString(),
                //    HR.Transformed.ToString(),
                //    BVP.Current.ToString(),
                //    BVP.Transformed.ToString(),
                //    EMGFrown.Current.ToString(),
                //    EMGFrown.Transformed.ToString(),
                //    EMGSmile.Current.ToString(),
                //    EMGSmile.Transformed.ToString(),
                //    m_arousal.Current.ToString(),
                //    m_arousal.Transformed.ToString(),
                //    m_valence.Current.ToString(),
                //    m_valence.Transformed.ToString(),
                //    m_fun.Current.ToString(),
                //    m_fun.Transformed.ToString(),
                //    m_excitement.Current.ToString(),
                //    m_excitement.Transformed.ToString(),
                //    m_boredom.Current.ToString(),
                //    m_boredom.Transformed.ToString()
                //);
			}
			catch (Exception e)
			{
				System.Windows.Forms.MessageBox.Show(e.Message);
			}
		}
        /*
		public void scaleEmotions()
		{
			m_fun.EnableNormalize = true;
			m_fun.NormalizeMinimum = 20.0;
			m_fun.NormalizeMaximum = 80.0;

			m_excitement.EnableNormalize = true;
			m_excitement.NormalizeMinimum = 20.0;
			m_excitement.NormalizeMaximum = 80.0;

			m_boredom.EnableNormalize = true;
			m_boredom.NormalizeMinimum = 20.0;
			m_boredom.NormalizeMaximum = 80.0;
		}
        */

        public void logGameEvent(int optcode, params float[] values)
        {
            string[] values_str = new string[values.Length + 1];

            values_str[0] = optcode.ToString();
            for (int i = 0; i < values.Length; ++i)
                values_str[i + 1] = values[i].ToString();

            m_log_game.CSV(Log.Details.Short, Log.Priority.Information, values_str);
        }
	}
}
