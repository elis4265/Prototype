using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
// To do, fruit growth based on tick system
public class PlantController : MonoBehaviour
{
    public PlantSettings plantSettings;
    PlantSettings.SeasonSettings currentSeasonSettings;
    public MeshRenderer[] fruits;
    public MeshRenderer[] leaves;

    private int currentSeason = 0;
    private int dayInSeason = 0;
    private int tick = 0;
    public int tickMultiplier = 1;

    [HideInInspector]
    public bool plantSettingsFoldout;

    void Start()
    {
        Initiate();
        // for seasonly changes
        SeasonsManager.OnNewSeason += delegate (object sender, SeasonsManager.OnNewSeasonEventArgs e)
        {
            currentSeason = e.season;
            currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        };
        // for daily updating
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            dayInSeason = GetDayInSeason(e.date);
            UpdatePlantColors();
        };
        // for fruit growing
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            tick++;
            if (IsGrowing() && tick >= currentSeasonSettings.growthSpeed * tickMultiplier)
            {
                UpdatePlantGrowth();
            }
            if (IsFruiting() && tick >= currentSeasonSettings.fruitingSpeed * tickMultiplier)
            {
                UpdateFruitGrowth();
            }
        };
    }
    private void Initiate()
    {
        currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        UpdatePlantColors();
        UpdateGrowth();
    }
    private void UpdatePlantColors()
    { // separate leaves and fruits
        float time = Mathf.Lerp(0, 1, dayInSeason / 60f); // current day / days in season
        //Debug.Log($"day: {day}, month: {month}, time: {time}");
        SetObjectsColor(currentSeasonSettings.leavesColorScheme.Evaluate(time), leaves);
        SetObjectsColor(currentSeasonSettings.fruitsColorScheme.Evaluate(time), fruits);
    }
    private void SetObjectsColor(Color color, MeshRenderer[] objects)
    { //ToDo add some randomness 
        foreach (MeshRenderer obj in objects)
        {
            obj.material.color = color;
        }
    }
    private void UpdateGrowth()
    {
        UpdatePlantGrowth();
        UpdateFruitGrowth();
        Debug.Log("Update growth");
    }
    private void UpdatePlantGrowth()
    {
        Debug.Log("Update plant growth");

    }
    private void UpdateFruitGrowth()
    {
        Debug.Log("Update fruit growth");

    }
    private bool IsGrowing()
    {
        return currentSeasonSettings.fruitingSpeed > 0;
    }
    private bool IsFruiting()
    {
        return currentSeasonSettings.fruitingSpeed > 0;
    }
    private int GetDayInSeason(int3 date)
    {//ToDo all
        int day = date.z;
        int month = date.y;
        int daysInMonth = ClockHnadler.DAYS_IN_MONTH;
        int monthsInYear = ClockHnadler.MONTHS_IN_YEAR;
        int seasons = ClockHnadler.SEASONS;

        int monthsInSeason = monthsInYear / seasons;
        int dayIncrement = daysInMonth * ((month + 1) % monthsInSeason);
        return day + dayIncrement;
    }
    public void CollectFruit()
    {
        //ToDo
    }
}
