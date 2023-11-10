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
    protected Date currTime;
    private bool searching;
    [SerializeField] protected bool sendSignal;
    [SerializeField] protected BeaconViewerController beaconUI;
    [SerializeField] protected BeaconConnectionsController beaconConnections;
    private void Start()
    {
        beaconID = Random.Range(0, 1000);
        sendSignal = false;
        data = new List<SensorInformation>
        {
            new SensorInformation { }
        };
        beaconUI.SetupID(beaconID);
        currTime = new Date();
        StartCoroutine(TimePassing());
        StartCoroutine(SendSignal());
    }
    private void Update()
    {
        if (!searching && !beaconConnections.hasParent())
        {
            StartCoroutine(Searching());
        }
    }
    private IEnumerator Searching()
    {
        searching = true;
        Packet packet = new Packet
        {
            beaconID = beaconID,
            info = this,
            signalType = SignalType.MESH_CONNECTION,
            isPropogated = false,
        };
        SignalFactory.CreateSignal(transform.position, 5, packet);
        yield return new WaitForSeconds(3);
        searching = false;
    }
    private IEnumerator TimePassing()
    {
        while (true)
        {
            currTime.Increment();
            yield return new WaitForSeconds(1);
        }
    }
    private IEnumerator SendSignal()
    {
        yield return new WaitForSeconds(Random.Range(0, 10));
        while (true)
        {
            Debug.Log("Beacon " + beaconID.ToString() + " sending signal");
            Packet packet = new Packet
            {
                beaconID = beaconID,
                //     info = new SensorInformation
                //     {
                //         time = currTime,
                //         temp = Random.Range(70, 80),
                //         humidity = Random.Range(0, 100),
                //         windDirection = new Vector2(Random.Range(0, 1.0f), Random.Range(0, 1.0f)),
                //         windSpeed = Random.Range(0, 100),
                //     },
                //     signalType = SignalType.DIRECT_SIGNAL,
                //     isPropogated = false,
                info = GridController.instance.GetSensorInfo(transform.position),
                signalType = (SignalType)Random.Range(1, 3),
            };
            beaconUI.UpdateInformation((SensorInformation)packet.info);
            beaconConnections.SendDirectSignal(this, packet);
            yield return new WaitForSeconds(Random.Range(2, 10));
        }
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
