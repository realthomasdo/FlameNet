using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XCharts.Runtime;

public enum DataType { TEMPERATURE, HUMIDITY, WIND_DIRECTION, WIND_SPEED, CO2LEVEL };
public class ChartsController : MonoBehaviour
{
    public static ChartsController i;
    [SerializeField] private List<LineChart> charts;
    [SerializeField] private List<DataType> dataTypes;
    private List<(int beaconID, SensorInformation info)> data;
    private List<int> beaconIDs;
    [SerializeField] private GameObject display;
    [SerializeField] private InputActionReference toggleGraphs;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<(int beaconID, SensorInformation info)>();
        beaconIDs = new List<int>();
        toggleGraphs.action.performed += ToggleDisplay;
        i = this;
    }
    public void AddData(int beaconID, SensorInformation sensorData, bool newData)
    {
        string serieName = GetSerieName(beaconID);
        if (newData)
        {
            data.Add((beaconID, sensorData));
        }
        for (int i = 0; i < charts.Count; i++)
        {
            DataType dataType = dataTypes[i];
            switch (dataType)
            {
                case DataType.TEMPERATURE:
                    charts[i].AddData(serieName, GetTime(sensorData.time), sensorData.temp);
                    break;
                case DataType.HUMIDITY:
                    charts[i].AddData(serieName, GetTime(sensorData.time), sensorData.humidity);
                    break;
                case DataType.WIND_SPEED:
                    charts[i].AddData(serieName, GetTime(sensorData.time), sensorData.windSpeed);
                    break;
            }
        }
    }
    private int GetTime(DateTime date)
    {
        return date.Minute * 100 + date.Second;
    }
    public void AddBeaconToTrack(int beaconID)
    {
        charts.ForEach(chart =>
        {
            chart.AddSerie<Line>(GetSerieName(beaconID));
        });
        data.ForEach(dataPoint =>
        {
            if (beaconID == dataPoint.beaconID)
            {
                AddData(beaconID, dataPoint.info, false);
            }
        });
    }
    public void RemoveBeaconToTrack(int beaconID)
    {
        charts.ForEach(chart =>
        {
            chart.RemoveSerie(GetSerieName(beaconID));
        });
    }
    private void ToggleDisplay(InputAction.CallbackContext context)
    {
        display.SetActive(!display.activeSelf);
    }
    private string GetSerieName(int beaconID)
    {
        return "Beacon: " + beaconID.ToString();
    }
}
