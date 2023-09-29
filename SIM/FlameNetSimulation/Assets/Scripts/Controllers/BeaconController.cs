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
    private bool isOn;
    private List<SensorInformation> data;
    // Start is called before the first frame update
    void Start()
    {
        beaconID = 1;
        data = new List<SensorInformation>();
        data.Add(new SensorInformation { });
        setPower(true);
        StartCoroutine(SpawnSignal());
    }
    public void setPower(bool isOn)
    {
        this.isOn = isOn;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            CollectData();
        }
    }
    private void CollectData()
    {
    }
    private IEnumerator SpawnSignal()
    {
        while (true)
        {
            Packet packet = new Packet
            {
                beaconID = beaconID,
                info = data[0],
            };
            SignalFactory.CreateSignal(transform.position, packet, 5);
            yield return new WaitForSeconds(2);
        }
    }
}
