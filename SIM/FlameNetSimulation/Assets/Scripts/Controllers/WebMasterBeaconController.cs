using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public enum RequestType { NodeLogs, Nodes }
public struct NodeLog
{
    public string _id;
    public string nodeId;
    public string timestamp;
    public string commitTimestamp;
    public float latitude;
    public float longitude;
    public float temperature;
    public float humidity;
    public float co2Level;
    public float ppm;
    public bool fireDetected;
    public bool isMasterNode;
    public int __v;

}
public class WebMasterBeaconController : WebBeaconController
{
    [SerializeField] private ChartsController charts;
    [SerializeField] private GameObject webBeaconPrefab;
    [SerializeField] private float maxDistance;
    private float positionScale;
    private List<WebBeaconController> webBeacons;
    private NodeLog masterNodeLog;
    // Start is called before the first frame update
    void Start()
    {
        positionScale = 10000;
        SetupBeacon(0);
        // StartCoroutine(GetRequest("https://flamenet-server.onrender.com/api/getNodeLogs", RequestType.NodeLogs));
        StartCoroutine(GetRequest("https://flamenet-server.onrender.com/api/getNodes", RequestType.Nodes));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator GetRequest(string uri, RequestType requestType)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
            switch (requestType)
            {
                case RequestType.Nodes:
                    List<NodeLog> nodes = ConvertNodeLogs(webRequest.downloadHandler.text);
                    masterNodeLog = nodes.Find(log => log.isMasterNode);
                    nodes.Remove(masterNodeLog);
                    SpawnSensorNodes(nodes);
                    break;
                case RequestType.NodeLogs:
                    List<NodeLog> nodeLogs = ConvertNodeLogs(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    private List<NodeLog> ConvertNodeLogs(string json)
    {
        string Json = json.Substring(1, json.Length - 2);
        Debug.Log(Json);
        string[] nodeLogObjects = Json.Split("},");
        List<NodeLog> nodeLogs = new List<NodeLog>();
        for (int i = 0; i < nodeLogObjects.Length; i++)
        {
            if (nodeLogObjects[i][nodeLogObjects[i].Length - 1] != '}')
            {
                nodeLogObjects[i] = nodeLogObjects[i] + "}";
            }
            Debug.Log(nodeLogObjects[i]);

            nodeLogs.Add(JsonUtility.FromJson<NodeLog>(nodeLogObjects[i]));
            Debug.Log(nodeLogs[i]);
            Debug.Log(nodeLogs[i].temperature);
            Debug.Log(nodeLogs[i].nodeId);
        }
        return nodeLogs;
    }
    private void SpawnSensorNodes(List<NodeLog> nodes)
    {
        nodes.ForEach(node =>
        {
            Vector3 position = GetNodePosition(node.latitude, node.longitude);
            WebBeaconController webBeacon = Instantiate(webBeaconPrefab, position, Quaternion.identity).GetComponent<WebBeaconController>();
            Debug.Log(int.Parse(node.nodeId.Remove(0, 4)));
            webBeacon.StartupBeacon(int.Parse(node.nodeId.Remove(0, 4)));
            webBeacons.Add(webBeacon);
        });
    }

    private Vector3 GetNodePosition(float latitude, float longitude)
    {
        return new Vector3(latitude - masterNodeLog.latitude, 0, longitude - masterNodeLog.longitude) * positionScale;
    }
}
