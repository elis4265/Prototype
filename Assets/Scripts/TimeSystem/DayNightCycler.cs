using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using Unity.Mathematics;
using UnityEngine;

public class DayNightCycler : MonoBehaviour
{
    public const int MINUTES_IN_HOUR = 60;
    public const int HOURS_IN_DAY = 24;
    private const int MORNING_START = 6;
    private const int EVENING_START = 20;
    private const int NIGHT_LENGHT = MORNING_START + (HOURS_IN_DAY - EVENING_START);
    private const int NIGHT_LENGHT_MINUTES = NIGHT_LENGHT * MINUTES_IN_HOUR;
    private const int DAY_LENGHT = HOURS_IN_DAY - NIGHT_LENGHT;
    private const int DAY_LENGHT_MINUTES = DAY_LENGHT * MINUTES_IN_HOUR;
    private const float LIGHT_ANGLE_STEP_NIGHT = 180f / (NIGHT_LENGHT * MINUTES_IN_HOUR);
    private const float LIGHT_ANGLE_STEP_DAY = 180f / (DAY_LENGHT * MINUTES_IN_HOUR);


    public enum DayState { Day, Night }

    /// <summary>
    /// Time format (hh:MM)
    /// </summary>
    public int2 time;
    public DayState dayStage;
    public bool timeDay = false;
    public GameObject ligthSourceDay;
    public GameObject ligthSourceNight;
    public bool logInfo = false;

    public LightningSettings lightningSettings;

    private void Start()
    {
        timeDay = CheckDay();
        //reset shadows
        ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
        ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;
    }

    public void UpdateLight()
    {
        CheckDay();
        UpdateShadowSource();
        UpdateLightsources();
    }
    private void UpdateLightsources()
    {
        float currentStep = 0; // current progress of day stage used to calculate angle of light source
        float lightAngle = 0;

        switch (dayStage)
        {
            case DayState.Day:
                currentStep = (time.x - MORNING_START) * MINUTES_IN_HOUR + time.y;
                currentStep = currentStep / (float)DAY_LENGHT_MINUTES;
                lightAngle = Mathf.Lerp(0, 180, currentStep);
                UpdateLight(ligthSourceDay.GetComponent<Light>(), currentStep);
                break;
            case DayState.Night:
                if (time.x > MORNING_START) currentStep = (time.x - EVENING_START) * MINUTES_IN_HOUR + time.y;
                else currentStep = (HOURS_IN_DAY - EVENING_START + time.x) * MINUTES_IN_HOUR + time.y;
                currentStep = currentStep / (float)NIGHT_LENGHT_MINUTES;
                lightAngle = Mathf.Lerp(-180, 0, currentStep);
                UpdateLight(ligthSourceNight.GetComponent<Light>(), currentStep);
                break;
            default:
                break;
        }

        ligthSourceDay.transform.parent.eulerAngles = new Vector3(lightAngle, 0, 0);
    }
    /// <summary>
    /// Updates light property settings based on lightning settings
    /// </summary>
    /// <param name="lightsource">Light object to update.</param>
    /// <param name="currentStep">Value between 0,1 that shows progress of day stage.</param>
    private void UpdateLight(Light lightsource, float currentStep)
    {
        LightningSettings.LightStateSettings currentSettings = lightningSettings.lightSattesSettings[(int)dayStage];

        lightsource.color = currentSettings.lightGradient.Evaluate(currentStep);

        float lightingIntesity = 0;
        // calculating current intensity based on vector3 in lightSettings, vector3 is in format (start, mid, end)
        float intesityStep = Mathf.InverseLerp(0, 0.5f, currentStep < 0.501f ? currentStep : currentStep - 0.5f); // 0.501 becouse if it's just 0.5 lightningn will blink when change happens
        if (currentStep > 0.5f) lightingIntesity = Mathf.Lerp(currentSettings.lightIntensity.y, currentSettings.lightIntensity.z, intesityStep);
        else lightingIntesity = Mathf.Lerp(currentSettings.lightIntensity.x, currentSettings.lightIntensity.y, intesityStep);

        lightsource.intensity = lightingIntesity;

    }
    /// <summary>
    /// Function to update time and setup new light position every minute.
    /// It's called from ClockHandler.
    /// </summary>
    /// <param name="time">Currennt time.</param>
    public void UpdateDayNightTime(int2 time)
    {
        this.time = time;
        if(CheckLightObject()) UpdateLight();
        if (logInfo) Debug.Log(dayStage.ToString());
    }
    
    /// <summary>
    /// Swaps between shadows from day light and shadows from night light based on time.
    /// </summary>
    private void UpdateShadowSource()
    {
        switch (dayStage)
        {
            case DayState.Day:
                if (ligthSourceDay.GetComponent<Light>().shadows != LightShadows.Soft)
                {
                    if (logInfo) Debug.Log("Zacina Den!");

                    ligthSourceDay.GetComponent<Light>().shadows = LightShadows.Soft;
                    ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;
                }
                break;
            case DayState.Night:
                if (ligthSourceNight.GetComponent<Light>().shadows != LightShadows.Soft)
                {
                    if (logInfo) Debug.Log("Zacina Noc!");

                    ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
                    ligthSourceNight.GetComponent<Light>().shadows = LightShadows.Soft;
                }
                break;
            default:
                break;
        }
        //To delete rest
        if (time.x == MORNING_START && !timeDay)
        {
        }
        else if (time.x == EVENING_START && timeDay)
        {
        }
    }
   
    /// <summary>
    /// Check if its day based on time.
    /// </summary>
    /// <returns>Returns false if its night otherwise returns true.</returns>
    private bool CheckDay()
    {
        timeDay = !(time.x < MORNING_START || time.x > EVENING_START);
        dayStage = timeDay ? DayState.Day : DayState.Night;
        return timeDay;
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
        if (!ligthSourceDay || !ligthSourceNight || !ligthSourceDay.transform.parent || !ligthSourceNight.transform.parent)
        {
            Debug.Log("Lightsource missing!");
            return false;
        }
        return true;
    }
    /// <summary>
    /// Sets default position for light sources. Should be used only when setuping scene or switching light source managment system.
    /// </summary>
    public void SetupLightDefaultRotation()
    { // 0 for curent light source system 1 for scrapped system that will be probably removed
        int currentSettings = 0;
        switch (currentSettings)
        {
            case 0: // use with UpdateLightsource()
                ligthSourceDay.transform.localEulerAngles = new Vector3(0, 0, 0);
                ligthSourceNight.transform.localEulerAngles = new Vector3(180, 0, 0);
                break;
            case 1: // use with SetLightAngleBasedOnTime(), this is scrapped code
                ligthSourceDay.transform.localEulerAngles = new Vector3(-90, -30, 0);
                ligthSourceNight.transform.localEulerAngles = new Vector3(90, -30, 0);
                break;
            default:
                break;
        }
    }

}
