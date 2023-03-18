using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockHnadler : MonoBehaviour
{
    private int3 date;
    public int2 time;

    public bool logTimeInfo = false;
    public bool pause = false;
    public TextMeshProUGUI timeTextField;
    public DayNightCycler dayNightCycler;

    public float timeSpeed;


    void Start()
    {
        date = int3.zero;
        time = new int2(0, 0);
        UpdateTimeSpeed();
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (pause) return;
            if(logTimeInfo) Debug.Log("Tick " + e.tick);
            IncreaseTime();
            dayNightCycler.UpdateDayNightTime(time);
        };
    }
    private void IncreaseTime()
    {
        int hours = time.x;
        int minutes = time.y;

        minutes++;
        if (minutes >= dayNightCycler.GetDayLenght().y)
        {
            hours++;
            minutes -= dayNightCycler.GetDayLenght().y;
        }
        if (hours >= dayNightCycler.GetDayLenght().x)
        {
            date.z++;
            hours -= dayNightCycler.GetDayLenght().x;
        }
        time = new int2(hours, minutes);
        timeTextField.text = $"{time.x} : {time.y}";

        if(logTimeInfo) Debug.Log(timeTextField.text);
    }
   
    public void UpdateTimeSpeed()
    {
        transform.GetComponent<TimeTickSystem>().SetSpeed(timeSpeed);
    }
    public void SwapPause() { pause = !pause; }
}