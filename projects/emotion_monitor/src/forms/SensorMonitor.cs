using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace emophyz
{
	public partial class m_frmEmotionMonitor : Form, ISensorListener
	{
		private SensorProvider m_provider;

		public m_frmEmotionMonitor()
		{
			InitializeComponent();

			m_provider = SensorProvider.getSingleton();
			m_provider.AddListener(this);
			if (m_provider.Connected)
				OnConnect();

			updateSpeedLabel();

			m_backgroundWorker.RunWorkerAsync();

			m_plotGSR.TimerInterval = 1000;
			m_plotGSR.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotHR.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotEKGFrown.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotEKGSmile.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotArousal.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotValence.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotFun.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotExcitement.ScaleMode = SpPerfChart.ScaleMode.Relative;
			m_plotBoredom.ScaleMode = SpPerfChart.ScaleMode.Relative;
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
					button_connect.Text = "Connecting";
					break;
				case Message.Connected:
					OnConnect();
					break;
				case Message.Disconnected:
					button_connect.Text = "Connect";
					break;
				case Message.SensorData:
					break;
				case Message.Arousal:
					break;
				case Message.Valence:
					break;
			}
		}

		void OnConnect()
		{
			button_connect.Text = "Disconnect";
		}

		private void bgWorkerDoWork(object sender, DoWorkEventArgs e)
		{
			System.Threading.Thread.Sleep(Convert.ToInt32(e.Argument));
		}

		private void bgWorkerWorkComplete(object sender, RunWorkerCompletedEventArgs e)
		{
			updatePlots();
			m_backgroundWorker.RunWorkerAsync(m_trcbWorkerWait.Value);
		}

		private void updatePlots()
		{
			m_plotGSR.AddValue(m_provider.GSR);
			m_plotHR.AddValue(m_provider.HR);
			m_plotEKGFrown.AddValue(m_provider.EKGFrown); 
			m_plotEKGSmile.AddValue(m_provider.EKGSmile); 
			m_plotArousal.AddValue(m_provider.Arousal); 
			m_plotValence.AddValue(m_provider.Valence); 
			m_plotFun.AddValue(m_provider.Fun);
			m_plotExcitement.AddValue(m_provider.Excitement); 
			m_plotBoredom.AddValue(m_provider.Boredom); 
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
