using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    public DateTime GetDate()
    {
        System.DateTime.TryParse(timestamp, out DateTime date);
        return date;
    }
    public int getNodeID()
    {
        return int.Parse(nodeId.Remove(0, 4));
    }
    public SensorInformation GetSensorInformation()
    {
        /*
        public DateTime time;
        public float temp;
        public float humidity;
        public float windDirection;
        public float windSpeed;
        */
        SensorInformation sensorInfo = new SensorInformation
        {
            time = GetDate(),
            temp = temperature,
            humidity = humidity,
        };
        return sensorInfo;
    }
}
public class WebRequests : MonoBehaviour
{
    private static string NodesURL = "https://flamenet-server.onrender.com/api/getNodes";
    private static string NodeLogsURL = "https://flamenet-server.onrender.com/api/getNodeLogs";
    public static WebRequests instance;
    private void Awake()
    {
        instance = this;
    }
    public void SendGetRequest(RequestType requestType, Action<List<NodeLog>> action)
    {
        StartCoroutine(GetRequest(requestType, action));
    }
    public Coroutine StaticConnection(RequestType requestType, Action<List<NodeLog>> action)
    {
        return StartCoroutine(RepeatRequests(requestType, action));
    }
    private IEnumerator RepeatRequests(RequestType requestType, Action<List<NodeLog>> action)
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            yield return StartCoroutine(GetRequest(requestType, action));
        }
    }
    private static List<NodeLog> ConvertNodeLogs(string json)
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
    private static IEnumerator GetRequest(RequestType requestType, Action<List<NodeLog>> action)
    {
        string uri = "";
        switch (requestType)
        {
            case RequestType.Nodes:
                uri = NodesURL;
                break;
            case RequestType.NodeLogs:
                uri = NodeLogsURL;
                break;
        }
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
                    action.Invoke(nodes);
                    break;
                case RequestType.NodeLogs:
                    List<NodeLog> nodeLogs = ConvertNodeLogs(webRequest.downloadHandler.text);
                    break;
            }
        }
    }
}
