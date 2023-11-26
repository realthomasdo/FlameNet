using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridCell : MonoBehaviour
{
    private int posX;
    private int posY;
    private int layer = 0;
    public SensorInformation sensorInfo;
    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }
    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }
    public SensorInformation GetSensorInformation()
    {
        sensorInfo.time = DateTime.Now;
        return sensorInfo;
    }
    private void Start()
    {
        sensorInfo = new SensorInformation
        {
            temp = UnityEngine.Random.Range(65.0f, 75),
            windSpeed = UnityEngine.Random.Range(5.0f, 8),
            humidity = UnityEngine.Random.Range(60.0f, 70)
        };
        GetSensorInformation();
    }
    private void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            layer = (layer + 1) % 3;
        }
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        switch (layer)
        {
            case 0: // Clear
                renderer.enabled = false;
                break;
            case 1: // Temperature Layer
                renderer.enabled = true;
                float bindTemp = sensorInfo.temp / 1000;
                float R = 1f * bindTemp;
                float G = (-1f * bindTemp) + 1f;
                renderer.color = new Color(R, G, 0f, 0.65f);
                break;
            case 2: // Humidity Layer
                renderer.enabled = true;
                float bindHumidity = sensorInfo.humidity / 100;
                float B = 1f * bindHumidity;
                renderer.color = new Color(0f, 0f, B, 0.65f);
                break;
        }
    }
}
