using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DayNightCycler : MonoBehaviour
{
    public const int MINUTES_IN_HOUR = 60;
    public const int HOURS_IN_DAY = 24;
    private const int MORNING_START = 6;
    private const int EVENING_START = 20;
    private const int NIGHT_LENGHT = MORNING_START + (HOURS_IN_DAY - EVENING_START);
    private const int DAY_LENGHT = HOURS_IN_DAY - NIGHT_LENGHT;
    private const float LIGHT_ANGLE_STEP_NIGHT = 180f / (NIGHT_LENGHT * MINUTES_IN_HOUR);
    private const float LIGHT_ANGLE_STEP_DAY = 180f / (DAY_LENGHT * MINUTES_IN_HOUR);

    private Vector3 lightAngle;
    public int2 time;
    public bool timeDay = false;
    public GameObject ligthSourceDay;
    public GameObject ligthSourceNight;

    private void Start()
    {
        lightAngle = Vector3.zero;
        timeDay = CheckDay();
        //SetLightAngleBasedOnTime(time);

    }
    public void UpdateDayNightTime(int2 time)
    {
        this.time = time;
        UpdateLightDirection(timeDay ? LIGHT_ANGLE_STEP_DAY : LIGHT_ANGLE_STEP_NIGHT);
        Debug.Log(timeDay ? "day" : "Night");
    }
    private void UpdateLightDirection(float increment)
    {
        lightAngle.x += increment;
        UpdateShadowSource();
        SetLightAngleBasedOnTime(time);
        //ligthSourceDay.transform.parent.eulerAngles = lightAngle;
    }
    private void UpdateShadowSource()
    {
        if (time.x == MORNING_START && !timeDay)
        {
            Debug.Log("Zacina Den!");
            timeDay = true;
            ligthSourceDay.GetComponent<Light>().shadows = LightShadows.Soft;
            ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;
        }
        else if (time.x == EVENING_START && timeDay)
        {
            Debug.Log("Zacina Noc!");
            timeDay = false;
            ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
            ligthSourceNight.GetComponent<Light>().shadows = LightShadows.Soft;
        }
    }
    public void SetLightAngleBasedOnTime(int2 time)
    {
        int timeHours = time.x;
        int timeMinutes = time.y;

        float angle = 0;

        if (timeHours < MORNING_START)
        {
            angle = ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_NIGHT;
        }
        else
        {
            if (timeHours < EVENING_START)
            {
                timeHours -= MORNING_START;
                angle = (MORNING_START * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_NIGHT;
                angle += ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_DAY;
            }
            else
            {
                timeHours -= MORNING_START;
                timeHours -= DAY_LENGHT;
                angle = (MORNING_START * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_NIGHT;
                angle += (DAY_LENGHT * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_DAY;
                angle += ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_NIGHT;
            }
        }
        lightAngle.x = angle;
        ligthSourceDay.transform.parent.eulerAngles = lightAngle;
    }
    private bool CheckDay()
    {
        if (time.x < MORNING_START || time.x > EVENING_START) return false;
        return true;
    }
    public int2 GetDayLenght() { return new int2(HOURS_IN_DAY, MINUTES_IN_HOUR); }
}
