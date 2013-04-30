using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SensorLib.Sensors
{

    public enum Channel
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J
    }

    public interface ITtlSensors
    {
        /*
        float BatteryPercentage { get; }

        ISensor CreateSensor(SensorType type, Channel channel);
        ISensor CreateSensor(String sensorName_, SensorType type, Channel channel);
        ISensor CreateSensor(String sensorName_, SensorType type, Channel channel, bool queueData);

        ITempSensor CreateTempSensor(Channel channel);
        ITempSensor CreateTempSensor(String sensorName_, Channel channel);
        ITempSensor CreateTempSensor(String sensorName_, Channel channel, bool queueData);

        IBvpSensor CreateBvpSensor(Channel channel);
        IBvpSensor CreateBvpSensor(String sensorName_, Channel channel);
        IBvpSensor CreateBvpSensor(String sensorName_, Channel channel, bool queueData);

        IGsrSensor CreateGsrSensor(Channel channel);
        IGsrSensor CreateGsrSensor(String sensorName_, Channel channel);
        IGsrSensor CreateGsrSensor(String sensorName_, Channel channel, bool queueData);

        IStrainSensor CreateStrainSensor(Channel channel);
        IStrainSensor CreateStrainSensor(String sensorName_, Channel channel);
        IStrainSensor CreateStrainSensor(String sensorName_, Channel channel, bool queueData);

        IMuscleSensor CreateMuscleSensor(Channel channel);
        IMuscleSensor CreateMuscleSensor(String sensorName_, Channel channel);
        IMuscleSensor CreateMuscleSensor(String sensorName_, Channel channel, bool queueData);

        IHeartSensor CreateHeartSensor(Channel channel);
        IHeartSensor CreateHeartSensor(String sensorName_, Channel channel);
        IHeartSensor CreateHeartSensor(String sensorName_, Channel channel, bool queueData);

        IBrainSensor CreateBrainSensor(Channel channel);
        IBrainSensor CreateBrainSensor(String sensorName_, Channel channel);
        IBrainSensor CreateBrainSensor(String sensorName_, Channel channel, bool queueData);

        IRawSensor CreateRawSensor(Channel channel);
        IRawSensor CreateRawSensor(String sensorName_, Channel channel);
        IRawSensor CreateRawSensor(String sensorName_, Channel channel, bool queueData);
        */
    }
}
