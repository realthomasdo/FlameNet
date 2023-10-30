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
    private int beaconID;
    private List<SensorInformation> data;
    private int timingSpot;
    private int timingSize;
    private Date old;
    private Date currTime;
    [SerializeField] private BeaconViewerController beaconUI;
    private void Start()
    {
        timingSpot = -1;
        beaconID = Random.Range(0, 100);
        data = new List<SensorInformation>
        {
            new SensorInformation { }
        };
        beaconUI.SetupID(beaconID);
        old = new Date();
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
    private IEnumerator SendSignal()
    {
        yield return new WaitForSeconds(5 * timingSpot);
        while (true)
        {
            Packet packet = new Packet
            {
                beaconID = beaconID,
                info = new SensorInformation
                {
                    time = currTime,
                    temp = Random.Range(0, 100),
                    humidity = Random.Range(0, 100),
                    windDirection = new Vector2(Random.Range(0, 1.0f), Random.Range(0, 1.0f)),
                    windSpeed = Random.Range(0, 100),
                },
                signalType = SignalType.SENSOR_INFORMATION,
                isPropogated = false,
            };
            beaconUI.UpdateInformation((SensorInformation)packet.info);
            if (beaconID == packet.beaconID)
            {
                SignalFactory.CreateSignal(transform.position, 5, packet);
            }
            yield return new WaitForSeconds(5 * timingSize);
        }
    }
    public void ReceiveSignal(SignalController signal)
    {
        Packet packet = signal.packet;
        switch (packet.signalType)
        {
            case SignalType.SENSOR_INFORMATION:
                if (packet.beaconID != beaconID)
                {
                    SensorInformation sensorInfo = (SensorInformation)packet.info;
                    if (sensorInfo.time > old && !data.Contains(sensorInfo))
                    {
                        data.Add(sensorInfo);
                        SignalFactory.CreateSignal(transform.position, 5, signal);
                    }
                }
                break;
            case SignalType.TIMING_SETUP:
                if (timingSpot == -1)
                {
                    List<int> spots = (List<int>)packet.info;
                    for (int i = 0; i < spots.Count; i++)
                    {
                        if (spots[i] == beaconID)
                        {
                            timingSpot = i;
                            timingSize = spots.Count;
                            beaconUI.SetupTiming(timingSpot * 5);
                            SignalFactory.CreateSignal(transform.position, 5, signal);
                            StartCoroutine(SendSignal());
                            return;
                        }
                    }
                }
                break;
            case SignalType.OFFSHORE_REQUEST:
                if (!packet.isPropogated)
                {
                    Packet sensorInfo = new Packet
                    {
                        beaconID = beaconID,
                        info = data,
                        signalType = SignalType.OFFSHORE_RESPONSE,
                        isPropogated = false
                    };
                    SignalFactory.CreateSignal(transform.position, 5, sensorInfo);
                }
                if (data.Count > 0)
                {
                    data.Clear();
                    SignalFactory.CreateSignal(transform.position, 5, signal);
                    old = (Date)packet.info;
                }
                break;
        }
    }
    public void ToggleDisplay()
    {
        beaconUI.ToggleDisplay();
    }

}
