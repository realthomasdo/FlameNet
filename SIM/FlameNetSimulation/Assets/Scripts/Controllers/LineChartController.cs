using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class LineChartController : MonoBehaviour
{
    private LineChart lineChart;
    // Start is called before the first frame update
    void Start()
    {
        lineChart = gameObject.GetComponent<LineChart>();
        StartCoroutine(AddData());
    }

    private IEnumerator AddData()
    {
        int i = 5;
        while (true)
        {
            lineChart.AddData("serie0", i++, 10);
            yield return new WaitForSeconds(2);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
