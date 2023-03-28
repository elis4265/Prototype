using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsManager : MonoBehaviour
{
    public PlantSettings plantSettings;
    public MeshRenderer leaves;


    public int currentSeason = 0;
    // check every time new day event is fired, also add some randomization so every year is a little different.
    private void Start()
    {
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            currentSeason = GetSeason(e.date.y);
            UpdateColor(e.date.z, e.date.y);
        };
    }
    private int GetSeason(int month) { return (month + 1) / 2 - 1; }  // 2 months per seasons, needs better calculation to be universal
    private void UpdateColor(int day, int month)
    {
        float t = (month + 1) % 2;
        t = 30 * t;
        float time = Mathf.Lerp(0,1, (day + t)  / 60f); // current day / days in season
        //Debug.Log($"day: {day}, month: {month}, time: {time}");
        leaves.material.color = plantSettings.seasonSettings[GetSeason(month)].colorScheme.Evaluate(time);
    }
}
