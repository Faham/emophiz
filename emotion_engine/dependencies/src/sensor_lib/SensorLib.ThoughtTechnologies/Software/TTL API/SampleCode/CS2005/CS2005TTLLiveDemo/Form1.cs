using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CS2005TTLLiveDemo
{
    public partial class Form1 : Form
    {
        const int BUFSIZE = 4096;
        float[] m_data;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_data = new float[BUFSIZE];  // arbitrary picked
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                int scanned = 0;
                int detected = 0;
                Cursor = Cursors.WaitCursor;
                axTTLLive1.OpenConnections(
                      (int)TTLLiveCtrlLib.TTLAPI_OPENCONNECTIONS_CMD_BITS.TTLAPI_OCCMD_AUTODETECT
                    , 1000
                    , ref scanned
                    , ref detected
                    );
                Cursor = Cursors.Default;
                if (axTTLLive1.EncoderCount > 0)
                {
                    setupChannels();
                    if (axTTLLive1.ChannelCount > 0)
                    {
                        axTTLLive1.StartChannels();
                        timer1.Start();
                    }
                }
                else
                {
                    MessageBox.Show("No encoder found.");
                }
            }
            catch (COMException Ex)
            {
                timer1.Stop();
                axTTLLive1.CloseConnections();
                string S = "Exception! : ";
                S += Ex.Message;
                MessageBox.Show(S);
            }
        }

        private void setupChannels()
        {
            axTTLLive1.AutoSetupChannels();
            int hChan = axTTLLive1.GetFirstChannelHND();
            while (hChan >= 0)
            {
                // Setting SensorType sets default unit type.
                int sensor_id = axTTLLive1.get_SensorID(hChan);
                axTTLLive1.set_SensorType(hChan, sensor_id);
                hChan = axTTLLive1.GetNextChannelHND();
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            try
            {
                int hChan = axTTLLive1.GetFirstChannelHND();
                while (hChan >= 0)
                {
                    int available = axTTLLive1.get_SamplesAvailable(hChan);
                    if (available > BUFSIZE) available = BUFSIZE;
                    axTTLLive1.ReadChannelData(hChan, out m_data[0], ref available);

                    // print out first element ( channel A only )
                    if ((int)TTLLiveCtrlLib.TTLAPI_CHANNELS.TTLAPI_CHANNEL_A == hChan)
                    {
                        labelDataA.Text = m_data[0].ToString("####.###");
                    }
                    hChan = axTTLLive1.GetNextChannelHND();
                }
            }
            catch (COMException Ex)
            {
                try
                {
                    timer1.Stop();
                    axTTLLive1.CloseConnections();
                    string S = "Exception! : ";
                    S += Ex.Message;
                    S += '\n';
                    S += "Stopping Channels...";
                    MessageBox.Show(S);

                } 
                catch( Exception Exxx )
                {
                    timer1.Stop();
                    // oops how unlucky!!!
                    String S = "Unexpected exception: ";
                    S += Exxx.Message;
                    MessageBox.Show(S);
                }
            }
        }
    }
}