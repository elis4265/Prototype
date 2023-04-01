using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsManager : MonoBehaviour
{
    public class OnNewSeasonEventArgs : EventArgs
    {
        public int season = 0;
        public int day = 0;
    }

    public static EventHandler<OnNewSeasonEventArgs> OnNewSeason;

    public PlantSettings plantSettings;
    public MeshRenderer leaves;
    public MeshRenderer fruits;


    public int currentSeason = 0;
    // check every time new day event is fired, also add some randomization so every year is a little different.
    private void Start()
    {
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            if (currentSeason != GetSeason(e.date.y))
            {
                currentSeason = GetSeason(e.date.y);
                OnNewSeason(this, new OnNewSeasonEventArgs { season = currentSeason, day = e.date.z }); // ToDo calculate day in season not in month
            }
            //UpdateColor(e.date.z, e.date.y);
        };
    }
    private int GetSeason(int month) { return (month + 1) / 2 - 1; }  // 2 months per seasons, needs better calculation to be universal
    private void UpdateColor(int day, int month)
    {
        float t = (month + 1) % 2;
        t = 30 * t;
        float time = Mathf.Lerp(0,1, (day + t)  / 60f); // current day / days in season
        //Debug.Log($"day: {day}, month: {month}, time: {time}");
        leaves.material.color = plantSettings.seasonSettings[GetSeason(month)].leavesColorScheme.Evaluate(time);
        fruits.material.color = plantSettings.seasonSettings[GetSeason(month)].fruitsColorScheme.Evaluate(time); // change based on growth stage not day
    }
}
