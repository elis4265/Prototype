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
    /// <summary>
    /// Time format (hh:MM)
    /// </summary>
    public int2 time;
    public TimeUtils.DayState dayStage;
    public bool timeDay = false;
    public GameObject ligthSourceDay;
    public GameObject ligthSourceNight;
    public bool logInfo = false;

    public LightningSettings lightningSettings;
    [HideInInspector]
    public bool lightningSettingsFoldout = true;

    private void Start()
    {
        timeDay = CheckDay();
        //reset shadows
        if (CheckLightObject())
        {
            ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
            ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;
        }
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
            case TimeUtils.DayState.Day:
                currentStep = (time.x - TimeUtils.MORNING_START) * TimeUtils.MINUTES_IN_HOUR + time.y;
                currentStep = currentStep / (float)TimeUtils.DAY_LENGHT_MINUTES;
                lightAngle = Mathf.Lerp(0, 180, currentStep);
                UpdateLight(ligthSourceDay.GetComponent<Light>(), currentStep);
                break;
            case TimeUtils.DayState.Night:
                if (time.x > TimeUtils.MORNING_START) currentStep = (time.x - TimeUtils.EVENING_START) * TimeUtils.MINUTES_IN_HOUR + time.y;
                else currentStep = (TimeUtils.HOURS_IN_DAY - TimeUtils.EVENING_START + time.x) * TimeUtils.MINUTES_IN_HOUR + time.y;
                currentStep = currentStep / (float)TimeUtils.NIGHT_LENGHT_MINUTES;
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
            case TimeUtils.DayState.Day:
                if (ligthSourceDay.GetComponent<Light>().shadows != LightShadows.Soft)
                {
                    if (logInfo) Debug.Log("Zacina Den!");

                    ligthSourceDay.GetComponent<Light>().shadows = LightShadows.Soft;
                    ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;

                    ligthSourceDay.SetActive(true);
                    ligthSourceNight.SetActive(false);
                }
                break;
            case TimeUtils.DayState.Night:
                if (ligthSourceNight.GetComponent<Light>().shadows != LightShadows.Soft)
                {
                    if (logInfo) Debug.Log("Zacina Noc!");

                    ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
                    ligthSourceNight.GetComponent<Light>().shadows = LightShadows.Soft;

                    ligthSourceDay.SetActive(false);
                    ligthSourceNight.SetActive(true);
                }
                break;
            default:
                break;
        }
    }
   
    /// <summary>
    /// Check if its day based on time.
    /// </summary>
    /// <returns>Returns false if its night otherwise returns true.</returns>
    private bool CheckDay()
    {
        timeDay = !(time.x < TimeUtils.MORNING_START || time.x >= TimeUtils.EVENING_START);
        dayStage = timeDay ? TimeUtils.DayState.Day : TimeUtils.DayState.Night;
        return timeDay;
    }
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
