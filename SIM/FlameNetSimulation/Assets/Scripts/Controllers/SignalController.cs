using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignalType { MESH_CONNECTION, DIRECT_SIGNAL };
public struct Packet
{
    public int beaconID;
    public object info;
    public SignalType signalType;
    public bool isPropogated;
}
public class SignalController : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    private float distanceTraveled;
    [SerializeField] private float growthSpeed;
    [SerializeField] private Renderer myModel;
    public Packet packet;
    public void SetValues(Packet packet, float maxDistance)
    {
        this.packet = packet;
        this.maxDistance = maxDistance;
    }
    public void SetValues(Packet packet, float maxDistance, Color color)
    {
        SetValues(packet, maxDistance);
        myModel.material.color = color;
    }
    public Color GetColor()
    {
        return myModel.material.color;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale += transform.localScale * growthSpeed * Time.deltaTime;
        distanceTraveled += transform.localScale.x * growthSpeed * Time.deltaTime / 2;
        Color matColor = myModel.material.color;
        matColor.a = 1 - distanceTraveled / maxDistance;
        myModel.material.color = matColor;
        if (maxDistance <= distanceTraveled)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Beacon"))
        {
            BeaconController beacon = other.gameObject.GetComponent<BeaconController>();
            beacon.ReceiveSignal(packet);
        }
        else if (other.gameObject.CompareTag("MasterBeacon"))
        {
            other.gameObject.GetComponent<MasterBeaconController>().ReceiveSignal(packet);
        }
    }
}
