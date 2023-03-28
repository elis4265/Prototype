using System;
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
    /// <summary>
    /// Function to update time and setup new light position every minute.
    /// It's called from ClockHandler.
    /// </summary>
    /// <param name="time">Currennt time.</param>
    public void UpdateDayNightTime(int2 time)
    {
        this.time = time;
        if(CheckLightObject()) UpdateLightDirection(timeDay ? LIGHT_ANGLE_STEP_DAY : LIGHT_ANGLE_STEP_NIGHT);
        Debug.Log(timeDay ? "day" : "Night");
    }
    /// <summary>
    /// Updates angle of light sources based on time or by incrementing.
    /// </summary>
    /// <param name="increment">How much angle of light changes per minute.</param>
    private void UpdateLightDirection(float increment)
    {
        lightAngle.x += increment;
        UpdateShadowSource();
        SetLightAngleBasedOnTime(time);
        //ligthSourceDay.transform.parent.eulerAngles = lightAngle;
    }
    /// <summary>
    /// Swaps between shadows from day light and shadows from night light based on time.
    /// </summary>
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
    /// <summary>
    /// Calculate light angle based on time.
    /// </summary>
    /// <param name="time">Time for wanted light angle.</param>
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
    /// <summary>
    /// Check if its day based on time.
    /// </summary>
    /// <returns>Returns false if its night otherwise returns true.</returns>
    private bool CheckDay()
    {
        if (time.x < MORNING_START || time.x > EVENING_START) return false;
        return true;
    }
    /// <summary>
    /// Returns lenght of day and length of an hour.
    /// </summary>
    /// <returns>Return int2 (hours,minutes) time.</returns>
    public int2 GetDayLenght() { return new int2(HOURS_IN_DAY, MINUTES_IN_HOUR); }
    /// <summary>
    /// Check if lightsources are missing.
    /// </summary>
    /// <returns>True if no lightsource is missing.</returns>
    private bool CheckLightObject()
    {
        if (!ligthSourceDay|| !ligthSourceNight)
        {
            Debug.Log("Lightsource missing!");
            return false;
        }
        return true;
    }
}
