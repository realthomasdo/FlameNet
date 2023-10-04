using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Date
{
    public int day;
    private int month;
    private int time; // 24 hour time 1200 is noon etc
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
    [SerializeField] private bool sendSignal = false;
    private void Start()
    {
        beaconID = Random.Range(0, 1000);
        data = new List<SensorInformation>
        {
            new SensorInformation { }
        };
        StartCoroutine(SendSignal());
    }

    private IEnumerator SendSignal()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 70));
            sendSignal = true;
        }
    }

    public void CollectData(Packet packet)
    {
        if (packet.beaconID != beaconID)
        {
            if (!data.Contains(packet.info))
            {
                data.Add(packet.info);
                SignalFactory.CreateSignal(transform.position, packet, 5);
            }
        }
    }
    private void Update()
    {
        if (sendSignal)
        {
            Packet packet = new Packet
            {
                beaconID = beaconID,
                info = new SensorInformation
                {
                    temp = Random.Range(0, 100000),
                },
            };
            SignalFactory.CreateSignal(transform.position, packet, 5);
            sendSignal = false;
        }
    }
}
