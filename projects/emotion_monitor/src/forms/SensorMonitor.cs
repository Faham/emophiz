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

			m_plotGSR.Messages.Add("GSR");
			m_plotGSR.Messages.Add("Current");
			m_plotGSR.Messages.Add("Min");
			m_plotGSR.Messages.Add("Max");
			m_plotHR.Messages.Add("HR");
			m_plotHR.Messages.Add("Current");
			m_plotHR.Messages.Add("Min");
			m_plotHR.Messages.Add("Max");
			m_plotEKGFrown.Messages.Add("EKGFrown");
			m_plotEKGFrown.Messages.Add("Current");
			m_plotEKGFrown.Messages.Add("Min");
			m_plotEKGFrown.Messages.Add("Max");
			m_plotEKGSmile.Messages.Add("EKGSmile");
			m_plotEKGSmile.Messages.Add("Current");
			m_plotEKGSmile.Messages.Add("Min");
			m_plotEKGSmile.Messages.Add("Max");
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
			m_btnConnect.Text = "&Connect";
			m_plot_emotion = false;
		}

		void OnConnect()
		{
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

				m_plotHR.AddValue(m_provider.HR.Transformed);
				m_plotHR.Messages[0] = "HR: " + String.Format(double_formats, m_provider.HR.Transformed);
				m_plotHR.Messages[1] = "Current: " + String.Format(double_formats, m_provider.HR.Current);
				m_plotHR.Messages[2] = "Min: " + String.Format(double_formats, m_provider.HR.Minimum);
				m_plotHR.Messages[3] = "Max: " + String.Format(double_formats, m_provider.HR.Maximum);

				m_plotEKGFrown.AddValue(m_provider.EKGFrown.Transformed);
				m_plotEKGFrown.Messages[0] = "EKGFrown: " + String.Format(double_formats, m_provider.EKGFrown.Transformed);
				m_plotEKGFrown.Messages[1] = "Current: " + String.Format(double_formats, m_provider.EKGFrown.Current);
				m_plotEKGFrown.Messages[2] = "Min: " + String.Format(double_formats, m_provider.EKGFrown.Minimum);
				m_plotEKGFrown.Messages[3] = "Max: " + String.Format(double_formats, m_provider.EKGFrown.Maximum);
	
				m_plotEKGSmile.AddValue(m_provider.EKGSmile.Transformed);
				m_plotEKGSmile.Messages[0] = "EKGSmile: " + String.Format(double_formats, m_provider.EKGSmile.Transformed);
				m_plotEKGSmile.Messages[1] = "Current: " + String.Format(double_formats, m_provider.EKGSmile.Current);
				m_plotEKGSmile.Messages[2] = "Min: " + String.Format(double_formats, m_provider.EKGSmile.Minimum);
				m_plotEKGSmile.Messages[3] = "Max: " + String.Format(double_formats, m_provider.EKGSmile.Maximum);
				
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
			double spd = 1000 / m_trcbWorkerWait.Value;
			m_lblPlotSpeed.Text = "Speed: " + spd.ToString() + " fps";
		}

		private void m_trcbWorkerWait_ValueChanged(object sender, EventArgs e)
		{
			updateSpeedLabel();
		}

		private SpPerfChart.PerfChart m_selectedPlot = null;
		private System.Drawing.Color m_defaultPlotBGColor = System.Drawing.SystemColors.ControlDark;
		private System.Drawing.Color m_selectedPlotBGColor = System.Drawing.Color.DarkMagenta;

		private void unselectPlot()
		{
			if (m_selectedPlot == null)
				return;

			Signal signal = getSignal(m_selectedPlot);
			signal.EnableCalibrate = false;
			m_selectedPlot.PerfChartStyle.BackgroundColorBottom = m_defaultPlotBGColor;
			m_selectedPlot.PerfChartStyle.BackgroundColorTop = m_defaultPlotBGColor;
			m_selectedPlot.Invalidate();
			m_selectedPlot = null;
			m_chbxCalibrate.Enabled = false;
		}

		private void selectPlot(SpPerfChart.PerfChart plot)
		{
			if (!m_provider.Connected)
				return;

			m_selectedPlot = plot;
			m_selectedPlot.PerfChartStyle.BackgroundColorBottom = m_selectedPlotBGColor;
			m_selectedPlot.PerfChartStyle.BackgroundColorTop = m_selectedPlotBGColor;
			m_selectedPlot.Invalidate();
			m_chbxCalibrate.Enabled = true;
		}

		private void sensorPlotClick(object sender, EventArgs e)
		{
			if (!m_provider.Connected)
				return;

			SpPerfChart.PerfChart plot = (SpPerfChart.PerfChart)sender;

			if (m_selectedPlot == plot)
			{
				unselectPlot();
				return;
			}

			if (plot == m_plotGSR || plot == m_plotHR || plot == m_plotEKGSmile || plot == m_plotEKGFrown)
			{
				unselectPlot();
				selectPlot(plot);
			}
		}

		private Signal getSignal(SpPerfChart.PerfChart plot)
		{
			if (plot == m_plotGSR)
				return m_provider.GSR;
			else if (plot == m_plotHR)
				return m_provider.HR;
			else if (plot == m_plotEKGSmile)
				return m_provider.EKGSmile;
			else if (plot == m_plotEKGFrown)
				return m_provider.EKGFrown;

			return null;
		}

		private void chbxCalibrateCheckedChanged(object sender, EventArgs e)
		{
			if(m_selectedPlot == null)
				return;

			Signal signal = getSignal(m_selectedPlot);
			if (m_chbxCalibrate.Checked)
				signal.EnableCalibrate = true;
			else
				signal.EnableCalibrate = false;
		}
	}
}
