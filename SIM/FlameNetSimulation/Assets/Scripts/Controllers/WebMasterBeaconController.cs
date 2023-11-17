using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WebMasterBeaconController : MasterBeaconController
{
    [SerializeField] private GameObject webBeaconPrefab;
    private float positionScale;
    private List<WebBeaconController> webBeacons;
    private NodeLog masterNodeLog;
    private Coroutine webConnection;
    // Start is called before the first frame update
    void Start()
    {
        positionScale = 10000;
        webBeacons = new List<WebBeaconController>();
        SetupBeacon(0);
        beaconConnections.AddParentConnection(this);
        WebRequests.instance.SendGetRequest(RequestType.Nodes, StartupResponse);
        webConnection = WebRequests.instance.StaticConnection(RequestType.Nodes, GetResponse);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void GetResponse(List<NodeLog> nodes)
    {
        masterNodeLog = nodes.Find(log => log.isMasterNode);
        nodes.Remove(masterNodeLog);
        CheckForSignals(nodes);
    }
    private void StartupResponse(List<NodeLog> nodes)
    {
        masterNodeLog = nodes.Find(log => log.isMasterNode);
        nodes.Remove(masterNodeLog);
        SpawnSensorNodes(nodes);
    }
    private void SpawnSensorNodes(List<NodeLog> nodes)
    {
        nodes.ForEach(node =>
        {
            Vector3 position = GetNodePosition(node.latitude, node.longitude);
            WebBeaconController webBeacon = Instantiate(webBeaconPrefab, position, Quaternion.identity).GetComponent<WebBeaconController>();
            Debug.Log("Unparsed: " + node.nodeId.Remove(0, 4));
            Debug.Log("Parsed: " + int.Parse(node.nodeId.Remove(0, 4)));
            webBeacon.StartupBeacon(node.getNodeID(), node.GetDate());
            webBeacons.Add(webBeacon);
        });
    }
    private void CheckForSignals(List<NodeLog> nodes)
    {
        nodes.ForEach(node =>
        {
            WebBeaconController beacon = webBeacons.Find(beacon => beacon.beaconID == node.getNodeID());
            beacon.SendSignal(node.GetSensorInformation());
        });
    }
    private Vector3 GetNodePosition(float latitude, float longitude)
    {
        return new Vector3(latitude - masterNodeLog.latitude, 0, longitude - masterNodeLog.longitude) * positionScale;
    }
}
