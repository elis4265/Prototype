using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockHnadler : MonoBehaviour
{
    private const int MINUTES_IN_HOUR = 60;
    private const int HOURS_IN_DAY = 24;

    private int3 date;
    private int2 time;

    public bool logTimeInfo = false;
    public TextMeshProUGUI timeTextField;
    public GameObject ligthSourceDay;
    public GameObject ligthSourceNight;

    public float timeSpeed;

    private Vector3 lightAngle;

    void Start()
    {
        lightAngle = Vector3.zero;
        date = int3.zero;
        time = new int2(0, 0);
        UpdateTimeSpeed();
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if(logTimeInfo) Debug.Log("Tick " + e.tick);
            IncreaseTime();
            UpdateLightDirection(1);
        };
    }
    private void IncreaseTime()
    {
        int hours = time.x;
        int minutes = time.y;

        minutes++;
        if (minutes >= MINUTES_IN_HOUR)
        {
            hours++;
            minutes -= MINUTES_IN_HOUR;
        }
        if (hours >= HOURS_IN_DAY)
        {
            date.z++;
            hours -= HOURS_IN_DAY;
        }
        time = new int2(hours, minutes);
        timeTextField.text = $"{time.x} : {time.y}";

        if(logTimeInfo) Debug.Log(timeTextField.text);
    }
    private void UpdateLightDirection(int increment)
    {
        lightAngle.x += increment;
        if (lightAngle.x == 360) lightAngle.x = 0;
        UpdateShadowSource();

        ligthSourceDay.transform.eulerAngles = lightAngle;
        lightAngle.x += 180;
        ligthSourceNight.transform.eulerAngles = lightAngle ;
        lightAngle.x -= 180;
    }
    private void UpdateShadowSource()
    {
        if (lightAngle.x == 0f)
        {
            Debug.Log("Zacina Den!");
            ligthSourceDay.GetComponent<Light>().shadows = LightShadows.Soft;
            ligthSourceNight.GetComponent<Light>().shadows = LightShadows.None;
        }
        else if (lightAngle.x == 180f)
        {
            Debug.Log("Zacina Noc!");
            ligthSourceDay.GetComponent<Light>().shadows = LightShadows.None;
            ligthSourceNight.GetComponent<Light>().shadows = LightShadows.Soft;
        }
    }
    public void UpdateTimeSpeed()
    {
        transform.GetComponent<TimeTickSystem>().SetSpeed(timeSpeed);
    }
}