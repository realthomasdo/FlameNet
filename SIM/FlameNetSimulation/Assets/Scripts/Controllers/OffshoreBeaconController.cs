using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffshoreBeaconController : MonoBehaviour
{
    private int beaconID = 0;
    [SerializeField] private bool sendSignal;
    private List<SensorInformation> data;
    [SerializeField] private BeaconConnectionsController beaconConnections;
    private Date currTime;
    // Start is called before the first frame update
    void Start()
    {
        data = new List<SensorInformation>();
        // StartCoroutine(SetupTimings());
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
    // Update is called once per frame
    void Update()
    {
        if (sendSignal)
        {
            Packet packet = new Packet
            {
                beaconID = beaconID,
                info = currTime,
                signalType = SignalType.OFFSHORE_REQUEST,
            };
            SignalFactory.CreateSignal(transform.position, 5, packet);
            sendSignal = false;
        }
    }
    public void ReceiveSignal(SignalController signal)
    {
        Packet packet = signal.packet;
        Debug.Log(packet.signalType);
        switch (packet.signalType)
        {
            case SignalType.OFFSHORE_RESPONSE:
                List<SensorInformation> dataIncoming = (List<SensorInformation>)packet.info;
                dataIncoming.ForEach(sensorInfo =>
                {
                    if (!data.Contains(sensorInfo))
                    {
                        data.Add(sensorInfo);
                    }
                });
                break;
            case SignalType.MESH_CONNECTION:
                BeaconController beacon = (BeaconController)packet.info;
                beaconConnections.AddConnection(beacon);
                break;
        }
    }
    private IEnumerator SetupTimings()
    {
        yield return new WaitForSeconds(2);
        List<int> beaconTimings = new List<int>();
        for (int i = 0; i < 100; i++)
        {
            beaconTimings.Add(i);
        }
        Packet packet = new Packet
        {
            beaconID = beaconID,
            info = beaconTimings,
            signalType = SignalType.TIMING_SETUP,
        };
        SignalFactory.CreateSignal(transform.position, 5, packet);
    }
}
