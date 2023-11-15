using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class WebBeaconController : BeaconController
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void StartupBeacon(int beaconID)
    {
        SetupBeacon(beaconID);
    }
    public void ConnectToMesh()
    {

    }
}
