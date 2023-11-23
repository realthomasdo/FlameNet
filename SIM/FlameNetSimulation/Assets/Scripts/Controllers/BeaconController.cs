using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BeaconController : MonoBehaviour
{
    public int beaconID;
    protected List<SensorInformation> data;
    protected List<DataType> sensorsMalfunctioning;
    protected bool searching;
    [SerializeField] protected bool sendSignal;
    [SerializeField] protected BeaconViewerController beaconUI;
    [SerializeField] protected BeaconConnectionsController beaconConnections;
    [SerializeField] protected Renderer myModel;
    private void Start()
    {
        SetupBeacon(Random.Range(0, 10000));
        StartCoroutine(SendSignalCycle());
        MalfunctionSensors();
    }
    private void Update()
    {
        if (!searching && !beaconConnections.hasParent())
        {
            StartCoroutine(Searching());
        }
    }
    protected void SetupBeacon(int beaconID)
    {
        this.beaconID = beaconID;
        sendSignal = false;
        data = new List<SensorInformation>
        {
            new SensorInformation { }
        };
        beaconUI.SetupID(beaconID);
    }
    protected IEnumerator Searching()
    {
        searching = true;
        Packet packet = new Packet
        {
            beaconID = beaconID,
            info = this,
            signalType = SignalType.MESH_CONNECTION,
        };
        SignalFactory.CreateSignal(transform.position, packet);
        yield return new WaitForSeconds(3);
        searching = false;
    }
    private IEnumerator SendSignalCycle()
    {
        yield return new WaitForSeconds(Random.Range(1, 10));
        while (true)
        {
            SendSignal();
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }
    private void MalfunctionSensors()
    {
        int amntEnums = Enum.GetValues(typeof(DataType)).Length;
        int numSensors = 0;
        float value = Random.value;
        if (value > 0.97)
        {
            numSensors = Random.Range(3, amntEnums);
            myModel.material.color = Color.red;
        }
        else if (value > 0.90)
        {
            numSensors = Random.Range(1, 3);
            myModel.material.color = Color.yellow;
        }
        List<DataType> sensorsLeft = new List<DataType>();
        for (int i = 0; i < amntEnums; i++)
        {
            sensorsLeft.Add((DataType)i);
        }
        sensorsMalfunctioning = new List<DataType>();
        for (int i = 0; i < numSensors; i++)
        {
            DataType sensorToRemove = sensorsLeft[Random.Range(0, sensorsLeft.Count)];
            sensorsMalfunctioning.Add(sensorToRemove);
            sensorsLeft.Remove(sensorToRemove);
        }
        beaconUI.UpdateSensors(sensorsMalfunctioning);
    }
    private SensorInformation ZeroSensorInfo(SensorInformation sensorInformation)
    {
        SensorInformation sensorZeroed = sensorInformation;
        sensorsMalfunctioning.ForEach(sensor =>
        {
            switch (sensor)
            {
                case DataType.TEMPERATURE:
                    sensorZeroed.temp = 0;
                    break;
                case DataType.CO2LEVEL:
                    sensorZeroed.co2Level = 0;
                    break;
                case DataType.HUMIDITY:
                    sensorZeroed.humidity = 0;
                    break;
                case DataType.WIND_DIRECTION:
                    sensorZeroed.windDirection = 0;
                    break;
                case DataType.WIND_SPEED:
                    sensorZeroed.windSpeed = 0;
                    break;
            }
        });
        return sensorZeroed;
    }
    public virtual void SendSignal()
    {
        Packet packet = new Packet
        {
            beaconID = beaconID,
        };
        if (GridController.instance != null)
        {
            SensorInformation sensorInfo = GridController.instance.GetSensorInfo(transform.position);
            sensorInfo = ZeroSensorInfo(sensorInfo);
            packet.info = sensorInfo;
            packet.signalType = sensorInfo.fireDetected ? SignalType.DISTRESS : SignalType.DIRECT;
        }
        beaconConnections.SendDirectSignal(this, packet);
    }
    public virtual void SendSignal(SensorInformation sensorInfo)
    {
        Packet packet = new Packet
        {
            beaconID = beaconID,
            signalType = SignalType.DIRECT,
        };
        if (GridController.instance != null)
        {
            packet.info = sensorInfo;
        }
        beaconConnections.SendDirectSignal(this, packet);
    }
    public virtual void ReceiveSignal(Packet packet)
    {
        switch (packet.signalType)
        {
            case SignalType.DIRECT:
            case SignalType.DISTRESS:
                beaconConnections.SendDirectSignal(this, packet);
                break;
            case SignalType.MESH_CONNECTION:
                if (packet.info is BeaconController)
                {
                    BeaconController beacon = (BeaconController)packet.info;
                    if (!beacon.hasParent() && beaconConnections.AddConnection(beacon))
                    {
                        beacon.AddParentConnection(this);
                    }
                }
                break;
        }
    }
    public virtual bool isConnectedToMesh()
    {
        return beaconConnections.isConnectedToMesh();
    }
    public void AddParentConnection(BeaconController beacon)
    {
        beaconConnections.AddParentConnection(beacon);
    }
    public void RemoveParentConnection()
    {
        beaconConnections.RemoveParentConnection();
    }
    public bool hasParent()
    {
        return beaconConnections.hasParent();
    }
    public void ToggleDisplay()
    {
        if (beaconUI.ToggleDisplay())
        {
            ChartsController.i.AddBeaconToTrack(beaconID);
        }
        else
        {
            ChartsController.i.RemoveBeaconToTrack(beaconID);
        }
    }
}
