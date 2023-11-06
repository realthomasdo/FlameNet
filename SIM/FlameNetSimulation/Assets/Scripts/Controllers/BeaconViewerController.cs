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
    [SerializeField] private TextMeshProUGUI temp, humidity, windDir, windSpeed, timing;
    private static string TEMP_TEXT_DEFAULT = "Temperature: ";
    private static string HUMIDITY_TEXT_DEFAULT = "Humidity: ";
    private static string WINDDIR_TEXT_DEFAULT = "Wind Direction: ";
    private static string WINDSPEED_TEXT_DEFAULT = "Wind Speed: ";
    private static string TIMING_TEXT_DEFAULT = "Timing: ";
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
    public void SetupTiming(int time)
    {
        timing.text = TIMING_TEXT_DEFAULT + time.ToString();
    }
    public bool ToggleDisplay()
    {
        display.SetActive(!display.activeSelf);
        return display.activeSelf;
    }
    public void UpdateInformation(SensorInformation latestInformation)
    {
        temp.text = TEMP_TEXT_DEFAULT + latestInformation.temp.ToString();
        humidity.text = HUMIDITY_TEXT_DEFAULT + latestInformation.humidity.ToString();
        windDir.text = WINDDIR_TEXT_DEFAULT + latestInformation.windDirection.ToString();
        windSpeed.text = WINDSPEED_TEXT_DEFAULT + latestInformation.windSpeed.ToString();
    }

}
