using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

//using System.Runtime.InteropServices;
using Microsoft.DirectX.DirectSound;

//[DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
//private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);


/*
// record from microphone
mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
mciSendString("record recsound", "", 0, 0);

// stop and save
mciSendString("save recsound c:\\record.wav", "", 0, 0);
mciSendString("close recsound ", "", 0, 0);
Computer c = new Computer();
c.Audio.Stop();

3. Under Read Button Click

Computer computer = new Computer();
computer.Audio.Play("c:\\record.wav", AudioPlayMode.Background);
*/


namespace SensorLib.Inputs
{

    
    public class DirectXAudioInput : IInput<float>
    {


        public DirectXAudioInput()
        {
            //DirectSound
            captureDevice = new Capture();
            WaveFormat waveFormat = SetupWaveFormat();
            bufferSize = waveFormat.AverageBytesPerSecond / 20;

            CaptureBufferDescription captureBufferDescription = new Microsoft.DirectX.DirectSound.CaptureBufferDescription();
            captureBufferDescription.BufferBytes = bufferPositions * bufferSize;
            captureBufferDescription.Format = waveFormat;
            captureBuffer = new CaptureBuffer(captureBufferDescription, captureDevice);

            notificationEvent = new AutoResetEvent(false);
            for (int i=0; i<bufferPositions; i++)
            {
                positionNotify[i].Offset = (bufferSize * i) + bufferSize - 1;
                positionNotify[i].EventNotifyHandle = notificationEvent.SafeWaitHandle.DangerousGetHandle();
            }
            audioNotification = new Notify(captureBuffer);
            audioNotification.SetNotificationPositions(positionNotify, bufferPositions);

            isStarted = false;
            return;
        }

        public event Action<IInput> DeviceDisconnected;

        public void Start()
        {
            if (!isStarted)
            {
                captureBuffer.Start(true);
            }
            return;
        }

        public void Stop()
        {
            if (isStarted)
            {
                captureBuffer.Stop();
            }
            return;
        }



        public float[] GetData(int maxSamples)
        {
            //Get data from microphone   
            byte[] buffer = (byte[])captureBuffer.Read(offset, typeof(byte), LockFlag.None, maxSamples);
            offset = (offset + bufferSize) % (bufferPositions * bufferSize);

            if (buffer == null)
                return new float[0];

            //Convert all bytes to floats
            float[] output = new float[buffer.Length];
            for (int i = 0; i < buffer.Length; i++)
                output[i] = (float)buffer[i];

            return output;
        }



        public void SignalExit()
        {
            throw new NotImplementedException();
        }




        //[DllImport("winmm.dll")]
        //private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr oCallback);

        WaveFormat SetupWaveFormat()
        {
            const short CHANNELS = 1;
            const int SAMPLES_PER_SECOND = 8000; //!!22050;
            const short BITS_PER_SAMPLE = 8;

            //Set up the wave format to be captured
            WaveFormat waveFormat = new Microsoft.DirectX.DirectSound.WaveFormat();
            waveFormat.Channels = CHANNELS;
            waveFormat.FormatTag = WaveFormatTag.Pcm;
            waveFormat.SamplesPerSecond = SAMPLES_PER_SECOND;
            waveFormat.BitsPerSample = BITS_PER_SAMPLE;
            waveFormat.BlockAlign = (short)(CHANNELS * (BITS_PER_SAMPLE / 8));
            waveFormat.AverageBytesPerSecond = waveFormat.BlockAlign * SAMPLES_PER_SECOND;

            return waveFormat;
        }



        /*
    void StartThread()
    {

        while(!threadExit)
        {
            notificationEvent.WaitOne(Timeout.Infinite, true);
            ProcessData();
        }
    }
    */




        bool isStarted;

        int offset = 0;
        int bufferSize = 0;

        //DirectSound stuff
        readonly Capture captureDevice;
        readonly CaptureBuffer captureBuffer;

        const int bufferPositions = 4;
        readonly Notify audioNotification;
        readonly AutoResetEvent notificationEvent;
        private BufferPositionNotify[] positionNotify = new BufferPositionNotify[bufferPositions + 1];



    }
}
