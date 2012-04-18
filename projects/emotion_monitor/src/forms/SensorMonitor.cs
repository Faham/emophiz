using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace emophiz
{
	public partial class m_frmEmotionMonitor : Form, ISensorListener
	{
		private SensorProvider m_provider;
		private bool m_plot_emotion = false;
		private Log m_log = new Log();
		Dictionary<string, SpPerfChart.PerfChart> m_sensorPlots = new Dictionary<string, SpPerfChart.PerfChart>();

		public m_frmEmotionMonitor()
		{
			System.IO.StreamWriter testfile = new System.IO.StreamWriter("test.log", false);
			testfile.WriteLine("test2");
			testfile.Close();

			m_log.Message("Initializing EmotionMonitor");
			InitializeComponent();

			m_provider = new SensorProvider(m_log);
			m_provider.AddListener(this);
			if (m_provider.Connected)
				OnConnect();

			updateSpeedLabel();

			m_sensorPlots["HR"] = m_plotHR;
			m_sensorPlots["GSR"] = m_plotGSR;
			m_sensorPlots["BVP"] = m_plotBVP;
			m_sensorPlots["EMGSmile"] = m_plotEMGSmile;
			m_sensorPlots["EMGFrown"] = m_plotEMGFrown;

			foreach (KeyValuePair<string, SpPerfChart.PerfChart> item in m_sensorPlots)
			{
				SpPerfChart.PerfChart plot = (SpPerfChart.PerfChart)item.Value;
				plot.Messages.Add(item.Key);
				plot.Messages.Add("Current");
				plot.Messages.Add("Min");
				plot.Messages.Add("Max");
			}

			//m_plotGSR.Messages.Add("GSR");
			//m_plotGSR.Messages.Add("Current");
			//m_plotGSR.Messages.Add("Min");
			//m_plotGSR.Messages.Add("Max");
			//m_plotHR.Messages.Add("HR");
			//m_plotHR.Messages.Add("Current");
			//m_plotHR.Messages.Add("Min");
			//m_plotHR.Messages.Add("Max");
			//m_plotEMGFrown.Messages.Add("EMGFrown");
			//m_plotEMGFrown.Messages.Add("Current");
			//m_plotEMGFrown.Messages.Add("Min");
			//m_plotEMGFrown.Messages.Add("Max");
			//m_plotEMGSmile.Messages.Add("EMGSmile");
			//m_plotEMGSmile.Messages.Add("Current");
			//m_plotEMGSmile.Messages.Add("Min");
			//m_plotEMGSmile.Messages.Add("Max");
			m_plotArousal.Messages.Add("Arousal");
			m_plotValence.Messages.Add("Valence");
			m_plotFun.Messages.Add("Fun");
			m_plotExcitement.Messages.Add("Excitement");
			m_plotBoredom.Messages.Add("Boredom");
			m_log.Message("EmotionMonitor initialized");
		}

		private void button_connect_Click(object sender, EventArgs e)
		{
			if (!m_provider.Connected)
				m_provider.Connect();
			else
				m_provider.Disconnect();
		}

		void ISensorListener.OnMessage(Message msg, Object value)
		{
			switch (msg)
			{
				case Message.Connecting:
					m_btnConnect.Text = "Connecting";
					break;
				case Message.Connected:
					OnConnect();
					break;
				case Message.Disconnected:
					OnDisconnect();
					break;
				case Message.SensorData:
					break;
				case Message.Arousal:
					break;
				case Message.Valence:
					break;
			}
		}

		void OnDisconnect()
		{
			m_prgbrBatteryLevel.Value = 0;
			m_lblBatteryLevel.Text = "No Connected";

			m_btnConnect.Text = "&Connect";
			m_plot_emotion = false;
		}

		void OnConnect()
		{
			m_prgbrBatteryLevel.Value = (int)m_provider.Encoder.GetBatteryPercentage();
			m_lblBatteryLevel.Text = String.Format("Battery level: {0}%", m_prgbrBatteryLevel.Value);

			m_btnConnect.Text = "&Disconnect";
			m_plot_emotion = true;
			m_backgroundWorker.RunWorkerAsync();
		}

		private void bgWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			System.Threading.Thread.Sleep(Convert.ToInt32(e.Argument));
		}

		private void bgWorkerWorkComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			updatePlots();
			if (m_plot_emotion)
				m_backgroundWorker.RunWorkerAsync(m_trcbWorkerWait.Value);
		}

		private void updatePlots()
		{
			try
			{
				string double_formats = "{0:0.##}";
				m_plotGSR.AddValue(m_provider.GSR.Transformed);
				m_plotGSR.Messages[0] = "GSR: " + String.Format(double_formats, m_provider.GSR.Transformed);
				m_plotGSR.Messages[1] = "Current: " + String.Format(double_formats, m_provider.GSR.Current);
				m_plotGSR.Messages[2] = "Min: " + String.Format(double_formats, m_provider.GSR.Minimum);
				m_plotGSR.Messages[3] = "Max: " + String.Format(double_formats, m_provider.GSR.Maximum);
				
				m_plotBVP.AddValue(m_provider.BVP.Transformed);
				m_plotBVP.Messages[0] = "BVP: " + String.Format(double_formats, m_provider.BVP.Transformed);
				m_plotBVP.Messages[1] = "Current: " + String.Format(double_formats, m_provider.BVP.Current);
				m_plotBVP.Messages[2] = "Min: " + String.Format(double_formats, m_provider.BVP.Minimum);
				m_plotBVP.Messages[3] = "Max: " + String.Format(double_formats, m_provider.BVP.Maximum);
				
				m_plotHR.AddValue(m_provider.HR.Transformed);
				m_plotHR.Messages[0] = "HR: " + String.Format(double_formats, m_provider.HR.Transformed);
				m_plotHR.Messages[1] = "Current: " + String.Format(double_formats, m_provider.HR.Current);
				m_plotHR.Messages[2] = "Min: " + String.Format(double_formats, m_provider.HR.Minimum);
				m_plotHR.Messages[3] = "Max: " + String.Format(double_formats, m_provider.HR.Maximum);
				
				m_plotEMGFrown.AddValue(m_provider.EMGFrown.Transformed);
				m_plotEMGFrown.Messages[0] = "EMGFrown: " + String.Format(double_formats, m_provider.EMGFrown.Transformed);
				m_plotEMGFrown.Messages[1] = "Current: " + String.Format(double_formats, m_provider.EMGFrown.Current);
				m_plotEMGFrown.Messages[2] = "Min: " + String.Format(double_formats, m_provider.EMGFrown.Minimum);
				m_plotEMGFrown.Messages[3] = "Max: " + String.Format(double_formats, m_provider.EMGFrown.Maximum);
	
				m_plotEMGSmile.AddValue(m_provider.EMGSmile.Transformed);
				m_plotEMGSmile.Messages[0] = "EMGSmile: " + String.Format(double_formats, m_provider.EMGSmile.Transformed);
				m_plotEMGSmile.Messages[1] = "Current: " + String.Format(double_formats, m_provider.EMGSmile.Current);
				m_plotEMGSmile.Messages[2] = "Min: " + String.Format(double_formats, m_provider.EMGSmile.Minimum);
				m_plotEMGSmile.Messages[3] = "Max: " + String.Format(double_formats, m_provider.EMGSmile.Maximum);
				
				m_plotArousal.AddValue(m_provider.Arousal);
				m_plotArousal.Messages[0] = "Arousal: " + String.Format(double_formats, m_provider.Arousal);

				m_plotValence.AddValue(m_provider.Valence);
				m_plotValence.Messages[0] = "Valence: " + String.Format(double_formats, m_provider.Valence);
				
				m_plotFun.AddValue(m_provider.Fun);
				m_plotFun.Messages[0] = "Fun: " + String.Format(double_formats, m_provider.Fun);
				
				m_plotExcitement.AddValue(m_provider.Excitement);
				m_plotExcitement.Messages[0] = "Excitement: " + String.Format(double_formats, m_provider.Excitement);
				
				m_plotBoredom.AddValue(m_provider.Boredom);
				m_plotBoredom.Messages[0] = "Boredom: " + String.Format(double_formats, m_provider.Boredom);
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
			}
		}

		private void updateSpeedLabel()
		{
			m_lblPlotSpeed.Text = "Speed: " + m_trcbWorkerWait.Value + " milisecond";
		}

		private void m_trcbWorkerWait_ValueChanged(object sender, EventArgs e)
		{
			updateSpeedLabel();
		}

		private void sensorPlotClick(object sender, EventArgs e)
		{
			if (!m_provider.Connected || !m_chbxCalibrate.Checked)
				return;

			SpPerfChart.PerfChart plot = (SpPerfChart.PerfChart)sender;

			if (getSignal(plot) != null)
				enableCalibrate(plot, !plot.Highlighted);
		}

		private Signal getSignal(SpPerfChart.PerfChart plot)
		{
			if (plot == m_plotGSR)
				return m_provider.GSR;
			else if (plot == m_plotHR)
				return m_provider.HR;
			else if (plot == m_plotBVP)
				return m_provider.BVP;
			else if (plot == m_plotEMGSmile)
				return m_provider.EMGSmile;
			else if (plot == m_plotEMGFrown)
				return m_provider.EMGFrown;

			return null;
		}

		private void enableCalibrate(SpPerfChart.PerfChart plot, bool calibrate)
		{
			plot.Highlighted = calibrate;
			getSignal(plot).EnableCalibrate = calibrate;
			plot.Invalidate();
		}

		private void m_chbxCalibrate_CheckedChanged(object sender, EventArgs e)
		{
			if (!m_provider.Connected)
				return;
			enableCalibrate(m_plotGSR, false);
			enableCalibrate(m_plotHR, false);
			enableCalibrate(m_plotEMGSmile, false);
			enableCalibrate(m_plotEMGFrown, false);
		}
	}
}
