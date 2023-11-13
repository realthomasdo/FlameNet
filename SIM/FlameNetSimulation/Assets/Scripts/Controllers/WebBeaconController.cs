using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
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
public class WebBeaconController : BeaconController
{
    // Start is called before the first frame update
    void Start()
    {
        SetupBeacon();
        StartCoroutine(GetRequest("https://flamenet-server.onrender.com/api/getNodeLogs"));
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator GetRequest(string uri)
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
            List<NodeLog> nodeLogs = ConvertNodeLogs(webRequest.downloadHandler.text);
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
        }
        return nodeLogs;
    }
}
