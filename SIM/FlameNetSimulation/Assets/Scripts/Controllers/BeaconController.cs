using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SensorInformation
{
    private float temp;
    private float humidity;
    private Vector2 windDirection;
    private float windSpeed;
}
public class BeaconController : MonoBehaviour
{
    private bool isOn;
    private List<SensorInformation> data;
    // Start is called before the first frame update
    void Start()
    {
        setPower(true);
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
}
