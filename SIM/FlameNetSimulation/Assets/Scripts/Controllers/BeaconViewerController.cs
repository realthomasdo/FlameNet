using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BeaconViewerController : MonoBehaviour
{
    [SerializeField] private GameObject display;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI temp, humidity, windDir, windSpeed, co2;
    private static string TEMP_TEXT_DEFAULT = "Temperature: ";
    private static string HUMIDITY_TEXT_DEFAULT = "Humidity: ";
    private static string WINDDIR_TEXT_DEFAULT = "Wind Direction: ";
    private static string WINDSPEED_TEXT_DEFAULT = "Wind Speed: ";
    private static string CO2_TEXT_DEFAULT = "CO2 Level: ";
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform);
        display.SetActive(false);
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }
    public void SetupID(int beaconID)
    {
        title.text = "BeaconID: " + beaconID.ToString();
    }
    public bool ToggleDisplay()
    {
        display.SetActive(!display.activeSelf);
        return display.activeSelf;
    }
    public void UpdateSensors(List<DataType> sensorsMalfunctioning)
    {
        temp.text = TEMP_TEXT_DEFAULT + "WORKING";
        co2.text = CO2_TEXT_DEFAULT + "WORKING";
        humidity.text = HUMIDITY_TEXT_DEFAULT + "WORKING";
        windDir.text = WINDDIR_TEXT_DEFAULT + "WORKING";
        windSpeed.text = WINDSPEED_TEXT_DEFAULT + "WORKING";

        sensorsMalfunctioning.ForEach(sensor =>
        {
            switch (sensor)
            {
                case DataType.TEMPERATURE:
                    temp.text = TEMP_TEXT_DEFAULT + "MALFUNCTIONING";
                    break;
                case DataType.CO2LEVEL:
                    co2.text = CO2_TEXT_DEFAULT + "MALFUNCTIONING";
                    break;
                case DataType.HUMIDITY:
                    humidity.text = HUMIDITY_TEXT_DEFAULT + "MALFUNCTIONING";
                    break;
                case DataType.WIND_DIRECTION:
                    windDir.text = WINDDIR_TEXT_DEFAULT + "MALFUNCTIONING";
                    break;
                case DataType.WIND_SPEED:
                    windSpeed.text = WINDSPEED_TEXT_DEFAULT + "MALFUNCTIONING";
                    break;
            }
        });

    }

}
