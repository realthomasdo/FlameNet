using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeaconConnectionsController : MonoBehaviour
{
    private List<(LineRenderer line, BeaconController beacon)> connections;
    [SerializeField] private GameObject linePrefab;
    private void Start()
    {
        connections = new List<(LineRenderer, BeaconController)>();
    }
    private void Update()
    {
        UpdateConnections();
    }
    private void UpdateConnections()
    {
        connections.ForEach(connection =>
        {
            connection.line.SetPosition(0, transform.position);
            connection.line.SetPosition(1, connection.beacon.transform.position);
        });
    }
    public bool AddConnection(BeaconController beacon)
    {
        Debug.Log("trying to add connection");
        if (connections.Count < 6)
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
}
