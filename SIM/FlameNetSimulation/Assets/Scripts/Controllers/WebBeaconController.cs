using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class WebBeaconController : BeaconController
{
    private DateTime lastSent;
    [SerializeField] private bool resetTime;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (!searching && !beaconConnections.hasParent())
        {
            StartCoroutine(Searching());
        }
        if (resetTime)
        {
            lastSent = DateTime.MinValue;
        }
    }
    public override void SendSignal(SensorInformation sensorInfo)
    {
        if (sensorInfo.time > lastSent)
        {
            lastSent = sensorInfo.time;
            base.SendSignal(sensorInfo);
        }
    }
    public void StartupBeacon(int beaconID, DateTime date)
    {
        SetupBeacon(beaconID);
        lastSent = date;
    }
}
