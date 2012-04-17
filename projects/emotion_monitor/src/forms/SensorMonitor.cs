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
				m_plotGSR.AddValue(m_provider.GSR.Transformed);
				m_plotGSR.Messages[0] = "GSR: " + m_provider.GSR.Transformed.ToString();
				m_plotGSR.Messages[1] = "Current: " + m_provider.GSR.Current.ToString();
				m_plotGSR.Messages[2] = "Min: " + m_provider.GSR.Minimum.ToString();
				m_plotGSR.Messages[3] = "Max: " + m_provider.GSR.Maximum.ToString();

				m_plotHR.AddValue(m_provider.HR.Transformed);
				m_plotHR.Messages[0] = "HR: " + m_provider.HR.Transformed.ToString();
				m_plotHR.Messages[1] = "Current: " + m_provider.HR.Current.ToString();
				m_plotHR.Messages[2] = "Min: " + m_provider.HR.Minimum.ToString();
				m_plotHR.Messages[3] = "Max: " + m_provider.HR.Maximum.ToString();

				m_plotEKGFrown.AddValue(m_provider.EKGFrown.Transformed);
				m_plotEKGFrown.Messages[0] = "EKGFrown: " + m_provider.EKGFrown.Transformed.ToString();
				m_plotEKGFrown.Messages[1] = "Current: " + m_provider.EKGFrown.Current.ToString();
				m_plotEKGFrown.Messages[2] = "Min: " + m_provider.EKGFrown.Minimum.ToString();
				m_plotEKGFrown.Messages[3] = "Max: " + m_provider.EKGFrown.Maximum.ToString();
	
				m_plotEKGSmile.AddValue(m_provider.EKGSmile.Transformed);
				m_plotEKGSmile.Messages[0] = "EKGSmile: " + m_provider.EKGSmile.Transformed.ToString();
				m_plotEKGSmile.Messages[1] = "Current: " + m_provider.EKGSmile.Current.ToString();
				m_plotEKGSmile.Messages[2] = "Min: " + m_provider.EKGSmile.Minimum.ToString();
				m_plotEKGSmile.Messages[3] = "Max: " + m_provider.EKGSmile.Maximum.ToString();
				
				m_plotArousal.AddValue(m_provider.Arousal);
				m_plotArousal.Messages[0] = "Arousal: " + m_provider.Arousal.ToString();

				m_plotValence.AddValue(m_provider.Valence);
				m_plotValence.Messages[0] = "Valence: " + m_provider.Valence.ToString();
				
				m_plotFun.AddValue(m_provider.Fun);
				m_plotFun.Messages[0] = "Fun: " + m_provider.Fun.ToString();
				
				m_plotExcitement.AddValue(m_provider.Excitement);
				m_plotExcitement.Messages[0] = "Excitement: " + m_provider.Excitement.ToString();
				
				m_plotBoredom.AddValue(m_provider.Boredom);
				m_plotBoredom.Messages[0] = "Boredom: " + m_provider.Boredom.ToString();
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
	}
}
