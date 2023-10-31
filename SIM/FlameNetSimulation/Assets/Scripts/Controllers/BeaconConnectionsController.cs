using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconConnectionsController : MonoBehaviour
{
    private BeaconController parent;
    private List<(LineRenderer line, BeaconController beacon)> connections;
    [SerializeField] private GameObject linePrefab;
    private float maxDist = 10;
    private void Start()
    {
        connections = new List<(LineRenderer, BeaconController)>();
    }
    private void Update()
    {
        UpdateConnections();
        CheckConnections();
    }
    private void UpdateConnections()
    {
        connections.ForEach(connection =>
        {
            connection.line.SetPosition(0, transform.position);
            connection.line.SetPosition(1, connection.beacon.transform.position);
        });
    }
    private void CheckConnections()
    {
        for (int i = 0; i < connections.Count; i++)
        {

            LineRenderer line = connections[i].line;
            float dist = Vector3.Distance(line.GetPosition(0), line.GetPosition(1));
            if (dist > maxDist)
            {
                StartCoroutine(ConnectionSevered(line, connections[i].beacon));
                connections.RemoveAt(i);
                i--;
            }
        }
    }
    public bool AddConnection(BeaconController beacon)
    {
        if (connections.Count < 6 && isConnectedToMesh())
        {
            GameObject line = Instantiate(linePrefab);
            line.transform.SetParent(transform, false);
            LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, beacon.transform.position);
            connections.Add((lineRenderer, beacon));
            return true;
        }
        return false;
    }
    public bool AddParentConnection(BeaconController beacon)
    {
        if (parent == null)
        {
            parent = beacon;
            return true;
        }
        return false;
    }
    public void RemoveParentConnection()
    {
        parent = null;
        connections.ForEach(connection =>
        {
            StartCoroutine(ConnectionSevered(connection.line, connection.beacon));
        });
        connections.Clear();
    }
    public bool hasParent()
    {
        return parent != null;
    }
    public void SendDirectSignal(BeaconController beacon, Packet packet)
    {
        if (parent != null && parent != beacon)
        {
            DirectSignalController directSignal = SignalFactory.CreateDirectSignal(beacon, parent, packet);
            directSignal.transform.SetParent(beacon.transform);
        }
    }
    private IEnumerator ConnectionSevered(LineRenderer line, BeaconController beacon)
    {
        line.startColor = Color.red;
        line.endColor = Color.red;
        yield return new WaitForSeconds(2);
        Destroy(line);
        beacon.RemoveParentConnection();
    }
    public bool isConnectedToMesh()
    {
        if (parent == null)
        {
            return false;
        }
        return parent.isConnectedToMesh();
    }
}
