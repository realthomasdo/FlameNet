using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Date
{
    public int day;
    public int month;
    public int time; // 24 hour time 1200 is noon etc
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
    [SerializeField] private BeaconViewerController beaconUI;
    private void Start()
    {
        beaconID = Random.Range(0, 1000);
        data = new List<SensorInformation>
        {
            new SensorInformation { }
        };
        StartCoroutine(SendSignal());
        beaconUI.Setup(beaconID);
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
                    temp = Random.Range(0, 100),
                    humidity = Random.Range(0, 100),
                    windDirection = new Vector2(Random.Range(0, 1.0f), Random.Range(0, 1.0f)),
                    windSpeed = Random.Range(0, 100),
                },
            };
            beaconUI.UpdateInformation(packet.info);
            if (beaconID == packet.beaconID)
            {
                SignalFactory.CreateSignal(transform.position, packet, 5);
            }
            sendSignal = false;
        }
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
    public void ToggleDisplay()
    {
        beaconUI.ToggleDisplay();
    }

}
