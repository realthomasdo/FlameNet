using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MasterBeaconController : BeaconController
{
    [SerializeField] private ChartsController charts;
    void Start()
    {
        SetupBeacon(0);
        beaconConnections.AddParentConnection(this);
    }
    void Update()
    {
        if (sendSignal)
        {
            sendSignal = false;
        }
    }
    public override void ReceiveSignal(Packet packet)
    {
        switch (packet.signalType)
        {
            case SignalType.DIRECT:
                if (packet.info is SensorInformation)
                {
                    SensorInformation sensorInfo = (SensorInformation)packet.info;
                    charts.AddData(packet.beaconID, sensorInfo, true);
                }
                break;
            case SignalType.DISTRESS:
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
    public override bool isConnectedToMesh()
    {
        return true;
    }
}
