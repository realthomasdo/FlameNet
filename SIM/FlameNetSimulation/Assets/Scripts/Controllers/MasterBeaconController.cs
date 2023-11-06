using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBeaconController : BeaconController
{
    [SerializeField] private ChartsController charts;
    void Start()
    {
        beaconID = 0;
        beaconConnections.AddParentConnection(this);
        sendSignal = false;
        currTime = new Date();
        StartCoroutine(TimePassing());
    }
    private IEnumerator TimePassing()
    {
        while (true)
        {
            currTime.Increment();
            yield return new WaitForSeconds(1);
        }
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
            case SignalType.DIRECT_SIGNAL:
                SensorInformation sensorInfo = (SensorInformation)packet.info;
                charts.AddData(packet.beaconID, sensorInfo, true);
                break;
            case SignalType.MESH_CONNECTION:
                BeaconController beacon = (BeaconController)packet.info;
                if (!beacon.hasParent() && beaconConnections.AddConnection(beacon))
                {
                    beacon.AddParentConnection(this);
                }
                break;
        }
    }
    public override bool isConnectedToMesh()
    {
        return true;
    }
}
