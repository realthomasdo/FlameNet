using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Date
{
    public int day;
    public int month;
    public int hour; // 24 hour time 1200 is noon etc
    public void Increment()
    {
        hour++;
        if (hour >= 2400)
        {
            hour = 0000;
            day++;
            if (day > 30)
            {
                month++;
                if (month > 12)
                {
                    month = 0;
                }
            }
        }
    }
    public int GetFullTime()
    {
        return month * 1000000 + day * 10000 + hour;
    }
    public static bool operator <(Date x, Date y)
    {
        if (x.month < y.month)
        {
            return true;
        }
        else if (x.month == y.month)
        {
            if (x.day < y.day)
            {
                return true;
            }
            else if (x.day == y.day)
            {
                if (x.hour < y.hour)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public static bool operator >(Date x, Date y)
    {
        if (x.month > y.month)
        {
            return true;
        }
        else if (x.month == y.month)
        {
            if (x.day > y.day)
            {
                return true;
            }
            else if (x.day == y.day)
            {
                if (x.hour > y.hour)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
public struct SensorInformation
{
    public Date time;
    public float temp;
    public float humidity;
    public Vector2 windDirection;
    public float windSpeed;
}
public class BeaconController : MonoBehaviour
{
    protected int beaconID;
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
    private IEnumerator Searching()
    {
        searching = true;
        Packet packet = new Packet
        {
            beaconID = beaconID,
            info = this,
            signalType = SignalType.MESH_CONNECTION,
        };
        SignalFactory.CreateSignal(transform.position, 5, packet);
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
    public void SendSignal()
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
    public void SendSignal(SensorInformation sensorInfo)
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
