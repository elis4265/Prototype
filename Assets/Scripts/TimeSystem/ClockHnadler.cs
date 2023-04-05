using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ClockHnadler : MonoBehaviour
{
    public class OnDayStartEventArgs: EventArgs
    {
        public int3 date;
        public int season;
    }

    public static event EventHandler<OnDayStartEventArgs> OnDayStart;

    private int3 date = new int3(0, 1, 0); // create event for new day
    public int2 time = new int2(0, 0);

    public bool logTimeInfo = false;
    public bool pause = false;
    public bool americanDateFormat = false;
    public TextMeshProUGUI timeTextField;
    public TextMeshProUGUI dateTextField;
    public DayNightCycler dayNightCycler;

    [Range(1,20)]
    public int timeSpeed = 20;
    public int tick;
    public bool ultraSpeed = false; // only for testing shit like seasons, increments days instead of minutes

    /// <summary>
    /// Initiating properties and subcribing to tick event.
    /// </summary>
    void Start()
    {
        ///Subscribinf to Tick system and updating time and daylight.
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            if (pause) return;
            tick++;
            if (tick >= timeSpeed)
            {
                tick -= timeSpeed;
                if (ultraSpeed)
                {
                    IncrementDate();
                    return;
                }
                if(logTimeInfo) Debug.Log("Tick " + e.tick);
                IncreaseTime();
                dayNightCycler.UpdateDayNightTime(time);
            }
        };
        UpdateDateText();
    }
    /// <summary>
    /// Increasing time value and updating OnScreen clock.
    /// </summary>
    private void IncreaseTime()
    {
        int hours = time.x;
        int minutes = time.y;

        minutes++;
        if (CheckHourLenght())
        {
            hours++;
            minutes -= TimeUtils.GetDayLenght().y;
            if (CheckDayLenght())
            {
                hours -= TimeUtils.GetDayLenght().x;
                IncrementDate();
            }
        }
        time = new int2(hours, minutes);
        timeTextField.text = $"{time.x} : {time.y}";

        if(logTimeInfo) Debug.Log(timeTextField.text);
    }

    public void SwapPause() { pause = !pause; }
    public void SwapUltraSpeed() { ultraSpeed = !ultraSpeed; }
    /// <summary>
    /// Checks if there is more minutes then set limit.
    /// </summary>
    /// <returns>True if num of current minues >= num of minues in hour.</returns>
    private bool CheckHourLenght() { return time.y >= TimeUtils.GetDayLenght().y - 1; }
    /// <summary>
    /// Checks if there is more hours then set limit.
    /// </summary>
    /// <returns>True if num of current hours >= num of hours in day.</returns>
    private bool CheckDayLenght() { return time.x >= TimeUtils.GetDayLenght().x - 1; }
    /// <summary>
    /// Checks if there is more days then set limit.
    /// </summary>
    /// <returns>True if num of current days >= num of days in month.</returns>
    private bool CheckMonthLenght() { return date.z >= TimeUtils.DAYS_IN_MONTH; }
    /// <summary>
    /// Checks if there is more months then set limit.
    /// </summary>
    /// <returns>True if num of current months >= num of months in year.</returns>
    private bool CheckYearLenght() { return date.y > TimeUtils.MONTHS_IN_YEAR; }
    private void IncrementDate(int incrementDays = 1)
    {
        date.z++;
        if (CheckMonthLenght()) 
        {
            date.z -= TimeUtils.DAYS_IN_MONTH;
            date.y++;
            if (CheckYearLenght())
            {
                date.y -= TimeUtils.MONTHS_IN_YEAR;
                date.x++;
            }
        }
        OnDayStart(this, new OnDayStartEventArgs { date = date, season = GetSeason(date.y) });
        UpdateDateText();
    }
    private void UpdateDateText()
    {
        if (!americanDateFormat)dateTextField.text = $"{date.z} / {date.y} / {date.x}";
        else dateTextField.text = $"{date.y} / {date.z} / {date.x}";
    }
    private int GetSeason(int month) { return (month + 1) / TimeUtils.GetMonthsPerSeason() - 1; }  // 2 months per seasons, needs better calculation to be universal
    public void SetSpeed(int speed) { timeSpeed = Mathf.Max(1, 20 - speed); }
}