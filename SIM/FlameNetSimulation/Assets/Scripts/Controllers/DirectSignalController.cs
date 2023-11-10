using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectSignalController : MonoBehaviour
{
    private BeaconController start;
    private BeaconController end;
    private float percentTraveled;
    private Packet packet;
    [SerializeField] private Renderer myModel;
    public void SetValues(BeaconController start, BeaconController end, Packet packet)
    {
        this.start = start;
        this.end = end;
        this.packet = packet;
        percentTraveled = 0;
        if (packet.signalType == SignalType.DISTRESS)
        {
            myModel.material.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (percentTraveled >= 1)
        {
            end.ReceiveSignal(packet);
            Destroy(gameObject);
        }
    }
    private void Move()
    {
        Vector3 distToEnd = end.transform.position - start.transform.position;
        transform.position = start.transform.position + distToEnd * percentTraveled;
        transform.LookAt(end.transform);
        percentTraveled += 0.5f * Time.deltaTime;
    }
}
