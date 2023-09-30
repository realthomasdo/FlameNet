using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Packet
{
    public int beaconID;
    public SensorInformation info;
}
public class SignalController : MonoBehaviour
{
    [SerializeField] private float maxDistance;
    private float distanceTraveled;
    [SerializeField] private float growthSpeed;
    [SerializeField] private Renderer myModel;
    private Packet packet;
    public void SetValues(Packet packet, float maxDistance)
    {
        this.packet = packet;
        this.maxDistance = maxDistance;
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
        Debug.Log("trigger");
        Debug.Log("collision occured:  tag was " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Beacon"))
        {
            other.gameObject.GetComponent<BeaconController>().CollectData(packet);
        }
    }
}
