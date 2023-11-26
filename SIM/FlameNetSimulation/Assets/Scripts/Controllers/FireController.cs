using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private List<(GridCell cell, float dist)> cells;
    public void SetCells(List<(GridCell, float)> cells)
    {
        this.cells = cells;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateCellInfo();
    }
    private void UpdateCellInfo()
    {
        cells.ForEach(grid =>
        {
            SensorInformation sensorInfo = grid.cell.sensorInfo;
            sensorInfo.temp = Mathf.Lerp(sensorInfo.temp, 1400 / grid.dist, Time.deltaTime);
            sensorInfo.humidity = Mathf.Lerp(sensorInfo.humidity, 0 + 10 * grid.dist, Time.deltaTime);
            grid.cell.sensorInfo = sensorInfo;
        });
    }
}
