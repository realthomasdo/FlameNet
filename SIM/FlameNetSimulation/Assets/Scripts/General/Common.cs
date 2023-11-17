using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public struct Date
{
    public int day;
    public int month;
    public int hour;
    public int minute;
    public int second;
    public void Increment()
    {
        hour++;
        if (hour >= 24)
        {
            hour = 00;
            day++;
            if (day > 30)
            {
                month++;
                if (month > 12)
                {
                    month = 0;
                }
            }
        }
    }
    public int GetFullTime()
    {
        return month * 1000000 + day * 10000 + hour;
    }
    public static bool operator <(Date x, Date y)
    {
        if (x.month < y.month)
        {
            return true;
        }
        else if (x.month == y.month)
        {
            if (x.day < y.day)
            {
                return true;
            }
            else if (x.day == y.day)
            {
                if (x.hour < y.hour)
                {
                    return true;
                }
                else if (x.hour == y.hour)
                {
                    if (x.minute < y.minute)
                    {
                        return true;
                    }
                    else if (x.minute == y.minute)
                    {
                        if (x.second < y.second)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    public static bool operator >(Date x, Date y)
    {
        if (x.month > y.month)
        {
            return true;
        }
        else if (x.month == y.month)
        {
            if (x.day > y.day)
            {
                return true;
            }
            else if (x.day == y.day)
            {
                if (x.hour > y.hour)
                {
                    return true;
                }
                else if (x.hour == y.hour)
                {
                    if (x.minute > y.minute)
                    {
                        return true;
                    }
                    else if (x.minute == y.minute)
                    {
                        if (x.second > y.second)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
public struct SensorInformation
{
    public DateTime time;
    public float temp;
    public float humidity;
    public float windDirection;
    public float windSpeed;
}
public class Common : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
