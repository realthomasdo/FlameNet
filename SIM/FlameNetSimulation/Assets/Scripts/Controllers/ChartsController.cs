using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using XCharts.Runtime;

public enum DataType { TEMPERATURE, HUMIDITY, WIND_DIRECTION, WIND_SPEED };
public class ChartsController : MonoBehaviour
{
    [SerializeField] private List<LineChart> charts;
    [SerializeField] private List<DataType> dataTypes;
    private List<(int beaconID, SensorInformation info)> data;
    [SerializeField] private InputActionReference toggleTempGraph;

    // Start is called before the first frame update
    void Start()
    {
        data = new List<(int beaconID, SensorInformation info)>();
        toggleTempGraph.action.performed += ToggleTemperatureGraph;
    }
    public void AddData(int beaconID, SensorInformation sensorData)
    {
        string serieName = "Beacon: " + beaconID.ToString();
        data.Add((beaconID, sensorData));
        charts.ForEach(chart =>
        {
            List<Serie> series = chart.series;
            if (series.FindIndex(serie => serie.serieName == serieName) == -1 && series.Count < 2)
            {
                chart.AddSerie<Line>(serieName);
            }
        });
        for (int i = 0; i < charts.Count; i++)
        {
            DataType dataType = dataTypes[i];
            switch (dataType)
            {
                case DataType.TEMPERATURE:
                    charts[i].AddData(serieName, sensorData.time.GetFullTime(), sensorData.temp);
                    break;
            }
        }
    }
    private void ToggleTemperatureGraph(InputAction.CallbackContext context)
    {
        ToggleDisplay((int)DataType.TEMPERATURE);
    }
    public void ToggleDisplay(int type)
    {
        GameObject chart = charts[dataTypes.FindIndex(dataType => dataType == (DataType)type)].gameObject;
        chart.SetActive(!chart.activeSelf);
    }
}
