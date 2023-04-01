using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.TerrainTools;
using UnityEngine;
// To do, fruit growth based on tick system
public class PlantController : MonoBehaviour
{
    public static int MAX_GROWTH = 100;
    public Vector3 defaultScale;

    public bool hasFruits = false;
    public PlantSettings plantSettings;
    PlantSettings.SeasonSettings currentSeasonSettings;
    [ConditionalHide("hasFruits")]
    public Transform fruits;
    public Transform leaves;

    private int currentSeason = 0;
    private int dayInSeason = 0;
    private int tick = 0;
    public int tickMultiplier = 1;

    [ConditionalHide("hasFruits"), Range(0, 100)]
    public int growthProgressFruit = 0;// should be out of 100
    [Range(0, 100), SerializeField]
    private int growthProgressPlant = 100;// should be out of 100

    [HideInInspector]
    public bool plantSettingsFoldout;

    void Start()
    {
        hasFruits = plantSettings.hasFruits;
        defaultScale = transform.localScale;
        Initiate();
        // for daily updating
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            currentSeason = e.season;
            currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
            dayInSeason = GetDayInSeason(e.date);
            UpdateLeavesColors();
        };
        // for fruit growing
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            tick++;
            if (tickMultiplier != 0)
            {
                // Update plant growth
                if (currentSeasonSettings.growthSpeed != 0 && IsGrowing() &&
                    tick % (currentSeasonSettings.growthSpeed * tickMultiplier) == 0)
                {
                    growthProgressPlant++;
                    UpdatePlantGrowth();
                }
                // Update fruit growth
                else if (currentSeasonSettings.fruitingSpeed != 0 && IsFruiting() &&
                    tick % (currentSeasonSettings.fruitingSpeed * tickMultiplier) == 0)
                {
                    growthProgressFruit++;
                    UpdateFruitGrowth();
                }
            }
            if (tick <= 100) tick -= 100;
        };
    }
    private void Initiate()
    {
        currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        UpdateLeavesColors();
        UpdateGrowth();
    }
    private void UpdateLeavesColors()
    { // separate leaves and fruits
        if (leaves == null) return;
        if (!currentSeasonSettings.haveFoliage)
        {
            if (leaves.gameObject.activeSelf) SetFoliageActive(false);
            return;
        }
        else
        {
            if (!leaves.gameObject.activeSelf) SetFoliageActive(true);
        }
        float time = Mathf.Lerp(0, 1, dayInSeason / 60f); // current day / days in season
        Color newColor = currentSeasonSettings.leavesColorScheme.Evaluate(time);

        SetObjectsColor(newColor, leaves);
    }
    private void SetFoliageActive(bool state)
    {
        leaves.gameObject.SetActive(state);
        fruits.gameObject.SetActive(state);
    }
    private void UpdateFruitColors()
    {
        if (fruits == null || !fruits.gameObject.activeSelf) return;
        float t = Mathf.Lerp(0, 1, growthProgressFruit / (float)MAX_GROWTH);
        Color newColor = plantSettings.fruitsColorScheme.Evaluate(t);

        SetObjectsColor(newColor, fruits);
    }
    private void SetObjectsColor(Color color, Transform objects)
    { //ToDo add some randomness 
        if (objects.childCount == 0) 
        {
            objects.GetComponent<MeshRenderer>().material.color = color;
        }
        else
        {
            foreach (Transform obj in objects)
            {
                for (int i = 0; i < obj.GetComponent<MeshRenderer>().materials.Length; i++)
                {
                    obj.GetComponent<MeshRenderer>().materials[i].color = color;
                }
            }
        }
    }

    private void UpdateGrowth()
    {
        UpdatePlantGrowth();
        UpdateFruitGrowth();
        //Debug.Log("Update growth");
    }
    private void UpdatePlantGrowth()
    {
        float newScale = Mathf.Lerp(0, defaultScale.x, growthProgressPlant / (float)MAX_GROWTH);
        transform.localScale = new Vector3(defaultScale.x, newScale, newScale);
        //Debug.Log("Update plant growth");
    }
    private void UpdateFruitGrowth()
    {
        UpdateFruitColors();
        //Debug.Log("Update fruit growth");
    }
    private bool IsGrowing()
    {
        bool isPlantFullyGrown = growthProgressPlant >= MAX_GROWTH;
        bool isGrowingThisSeason = currentSeasonSettings.growthSpeed > 0;

        return isGrowingThisSeason && !isPlantFullyGrown;
    }
    private bool IsFruiting()
    {
        bool isPlantFullyGrown = growthProgressPlant >= MAX_GROWTH;
        bool isFruitFullyGrown = growthProgressFruit < MAX_GROWTH;
        bool isFruitingThisSeason = currentSeasonSettings.fruitingSpeed > 0;

        return  isFruitingThisSeason && isFruitFullyGrown && isPlantFullyGrown;
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
        growthProgressFruit = 0;
        UpdateFruitColors();
    }
    public void ResetPlantGrowth()
    {
        growthProgressPlant = 0;
        CollectFruit();
        UpdateFruitColors();
    }
}
