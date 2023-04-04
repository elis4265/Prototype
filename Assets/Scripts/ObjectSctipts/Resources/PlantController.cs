using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.TerrainTools;
using UnityEngine;
// To do, set random value as offset when instantiating plants and apply it to leaves gradient value calculation (t in lerp) so its not so uniformal
public class PlantController : MonoBehaviour
{
    public static int MAX_GROWTH = 100;
    public Vector3 defaultScale; //used for growth, not optimal

    public bool hasFruits = false;
    public PlantSettings plantSettings;
    PlantSettings.SeasonSettings currentSeasonSettings;
    [ConditionalHide("hasFruits")]
    public Transform fruits; //fruits object or parent of multiple fruit objects
    public Transform leaves;//leaves object or parent of multiple leaves objects

    private int currentSeason = 0;
    private int dayInSeason = 0;
    private int tick = 0;
    public int tickMultiplier = 1;

    [ConditionalHide("hasFruits"), Range(0, 100)]
    public int growthProgressFruit = 0;// out of 100
    [Range(0, 100), SerializeField]
    private int growthProgressPlant = 100;// out of 100

    [HideInInspector]
    public bool plantSettingsFoldout; // for custom editor, making folding of settings work

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
                if (IsGrowing() && tick % (currentSeasonSettings.growthSpeed * tickMultiplier) == 0)
                {
                    growthProgressPlant++;
                    UpdatePlantGrowth();
                }
                // Update fruit growth
                else if (IsFruiting() && tick % (currentSeasonSettings.fruitingSpeed * tickMultiplier) == 0)
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
    {
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
    /// <summary>
    /// Activate or deactivate leaves and fruits based on state entered.
    /// </summary>
    /// <param name="state">Bool that will be set as active status.</param>
    private void SetFoliageActive(bool state)
    {
        leaves.gameObject.SetActive(state);
        fruits.gameObject.SetActive(state);
    }
    /// <summary>
    /// Updates color of fruit based on assigned gradiend color and growth progress.
    /// </summary>
    private void UpdateFruitColors()
    {
        if (fruits == null || !fruits.gameObject.activeSelf) return;
        float t = Mathf.Lerp(0, 1, growthProgressFruit / (float)MAX_GROWTH);
        Color newColor = plantSettings.fruitsColorScheme.Evaluate(t);

        SetObjectsColor(newColor, fruits);
    }
    /// <summary>
    /// Sets color of object or child of object.
    /// </summary>
    /// <param name="color">New color.</param>
    /// <param name="objects">Obect to change or parent of objects to change.</param>
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
    }
    private void UpdatePlantGrowth()
    {
        float newScale = Mathf.Lerp(0, defaultScale.x, growthProgressPlant / (float)MAX_GROWTH);
        transform.localScale = new Vector3(defaultScale.x, newScale, newScale);
    }
    private void UpdateFruitGrowth()
    {
        UpdateFruitColors();
    }
    /// <summary>
    /// Check if plant is still growing.
    /// </summary>
    /// <returns>True if it's growing otherwise false.</returns>
    private bool IsGrowing()
    {
        bool isPlantFullyGrown = growthProgressPlant >= MAX_GROWTH;
        bool isGrowingThisSeason = currentSeasonSettings.growthSpeed > 0;

        return isGrowingThisSeason && !isPlantFullyGrown;
    }
    /// <summary>
    /// Check if plant is fruiting.
    /// </summary>
    /// <returns>True if fruit is growing and parent plant is fully grown, otherwise false.</returns>
    private bool IsFruiting()
    {
        bool isPlantFullyGrown = growthProgressPlant >= MAX_GROWTH;
        bool isFruitFullyGrown = growthProgressFruit >= MAX_GROWTH;
        bool isFruitingThisSeason = currentSeasonSettings.fruitingSpeed > 0;

        return  isFruitingThisSeason && !isFruitFullyGrown && isPlantFullyGrown;
    }
    /// <summary>
    /// Calculates what day in season it is based on date
    /// </summary>
    /// <param name="date">Current date.</param>
    /// <returns>Day in season.</returns>
    private int GetDayInSeason(int3 date)
    {
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
