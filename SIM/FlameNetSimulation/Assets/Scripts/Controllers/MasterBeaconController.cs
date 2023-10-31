using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBeaconController : BeaconController
{
    void Start()
    {
        beaconID = 0;
        beaconConnections.AddParentConnection(this);
        data = new List<SensorInformation>();
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
    new public void ReceiveSignal(Packet packet)
    {
        switch (packet.signalType)
        {
            case SignalType.DIRECT_SIGNAL:
                data.Add((SensorInformation)packet.info);
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
