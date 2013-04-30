using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SensorLib.Sensors
{

    public interface ILiveSession
    {
        /// <summary>
        /// Gets the battery percentage of the specified encoder.
        /// </summary>
        /// <param name="encoder"></param>
        /// <returns>The percentage of the battery left.  Returns zero if encoder is not found.</returns>
        float GetBatteryPercentage(ITtlEncoder encoder);

        /// <summary>
        /// Starts gathering data from all the sensors.
        /// </summary>
        void StartAllSensors();

        /// <summary>
        /// Stops gathering data from all the sensors.
        /// </summary>
        void StopAllSensors();

        void RemoveSensor(ISensor sensor);

        /// <summary>
        /// Creates an audio sensor from the default system microphone.
        /// </summary>
        /// <returns></returns>
        IAudioSensor CreateAudioSensor();
        IAudioSensor CreateAudioSensor(String sensorName_);
        
        /// <summary>
        /// Creates an audio sensor from the default system microphone.
        /// </summary>
        /// <param name="queueData">
        /// Whether the library will queue the sensor data.
        /// </param>
        /// <returns></returns>
        IAudioSensor CreateAudioSensor(String sensorName_, bool queueData);

        /// <summary>
        /// Note: You cannot use the Mindset if you have asked for the device controls for devices with Windows Media Encoder
        /// </summary>
        /// <param name="comNumber"></param>
        void ConnectMindset(String portNumber);
        void DisconnectMindset();
        bool MindsetConnected { get; }
        event Action<bool> MindsetConnectedChanged;
        IBrainSensor CreateBrainSensor(String sensorName_);
        IBrainSensor CreateBrainSensor(String sensorName_, bool queueData);


        IEyeTracker[] EyeTrackers { get; }
        event EyeTrackerEvent EyeTrackerAdded;
        event EyeTrackerEvent EyeTrackerRemoved;

        IEyeSensor CreateEyeSensor(IEyeTracker eyeTracker);
        IEyeSensor CreateEyeSensor(String sensorName_, IEyeTracker eyeTracker);
        IEyeSensor CreateEyeSensor(String sensorName_, IEyeTracker eyeTracker, bool queueData);

        ITtlEncoder[] TtlEncoders { get; }

        ITtlSensor CreateTtlSensor(SensorType type, String name, ITtlEncoder encoder, Channel channel);
        ITtlSensor CreateTtlSensor(SensorType type, String name, ITtlEncoder encoder, Channel channel, bool queueData);
        ITempSensor CreateTempSensor(String name, ITtlEncoder encoder, Channel channel);
        ITempSensor CreateTempSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IBvpSensor CreateBvpSensor(String name, ITtlEncoder encoder, Channel channel);
        IBvpSensor CreateBvpSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IGsrSensor CreateGsrSensor(String name, ITtlEncoder encoder, Channel channel);
        IGsrSensor CreateGsrSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IStrainSensor CreateStrainSensor(String name, ITtlEncoder encoder, Channel channel);
        IStrainSensor CreateStrainSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IMuscleSensor CreateMuscleSensor(String name, ITtlEncoder encoder, Channel channel);
        IMuscleSensor CreateMuscleSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IHeartSensor CreateHeartSensor(String name, ITtlEncoder encoder, Channel channel);
        IHeartSensor CreateHeartSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        //IBrainSensor CreateBrainSensor(String name, ITtlEncoder encoder, Channel channel);
        //IBrainSensor CreateBrainSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        IRawSensor CreateRawSensor(String name, ITtlEncoder encoder, Channel channel);
        IRawSensor CreateRawSensor(String name, ITtlEncoder encoder, Channel channel, bool queueData);
        

        //bool TtlAvailable { get; }  //This tries to connect with the encoder and is computationally intensive
        //ITtlSensors TtlSensors { get; }  //This tries to connect with the encoder and is computationally intensive until the device is found
        

    }
}
