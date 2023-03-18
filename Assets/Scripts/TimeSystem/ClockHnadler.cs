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

    [Range(1,20)]
    public int timeSpeed = 10;
    public int tick;

    /// <summary>
    /// Initiating properties and subcribing to tick event.
    /// </summary>
    void Start()
    {
        date = int3.zero;
        time = new int2(0, 0);
        ///Subscribinf to Tick system and updating time and daylight.
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (pause) return;
            tick++;
            if (tick >= timeSpeed)
            {
                tick -= timeSpeed;
                if(logTimeInfo) Debug.Log("Tick " + e.tick);
                IncreaseTime();
                dayNightCycler.UpdateDayNightTime(time);
            }
        };
    }
    /// <summary>
    /// Increasing time value and updating OnScreen clock.
    /// </summary>
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

    public void SwapPause() { pause = !pause; }
}