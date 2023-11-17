using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconController : MonoBehaviour
{
    public int beaconID;
    protected List<SensorInformation> data;
    protected bool searching;
    [SerializeField] protected bool sendSignal;
    [SerializeField] protected BeaconViewerController beaconUI;
    [SerializeField] protected BeaconConnectionsController beaconConnections;
    private void Start()
    {
        SetupBeacon(Random.Range(0, 1000));
        StartCoroutine(SendSignalCycle());
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
        yield return new WaitForSeconds(Random.Range(0, 10));
        while (true)
        {
            SendSignal();
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
    }
    public virtual void SendSignal()
    {
        Packet packet = new Packet
        {
            beaconID = beaconID,
            signalType = (SignalType)Random.Range(1, 3),
        };
        if (GridController.instance != null)
        {
            packet.info = GridController.instance.GetSensorInfo(transform.position);
            beaconUI.UpdateInformation((SensorInformation)packet.info);
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
            beaconUI.UpdateInformation((SensorInformation)packet.info);
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
