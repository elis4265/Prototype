using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// To do, set random value as offset when instantiating plants and apply it to leaves gradient value calculation (t in lerp) so its not so uniformal
public class PlantController : MonoBehaviour
{
    public enum GrowingStage { Growing, Static, Fruiting, Decaying }


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
    [ConditionalHide("hasFruits")]
    public int fruitCooldown = 0;// time between collecting and growing again
    [Range(0, 100), SerializeField]
    private int growthProgressPlant = 100;// out of 100

    [SerializeField]
    private GrowingStage plantStage = GrowingStage.Growing;

    [HideInInspector]
    public bool plantSettingsFoldout; // for custom editor, making folding of settings work

    void Start()
    {
        Initiate();
        // for daily updating
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            currentSeason = e.season;
            currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
            dayInSeason = TimeUtils.GetDayInSeason(e.date);
            UpdateLeavesColors();
        };
        // Update each tick
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            tick++;
            UpdatePlant();
            if (tick <= 100) tick -= 100;
        };
    }

    private void Initiate()
    {
        hasFruits = plantSettings.hasFruits;
        defaultScale = transform.localScale;
        currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        UpdateLeavesColors();
        UpdatePlant();
    }

    private void UpdatePlant()
    {
        switch (plantStage)
        {
            case GrowingStage.Growing:
                if (IsGrowing())
                {
                    if (fruits.gameObject.activeSelf) fruits.gameObject.SetActive(false);
                    if (tick % (currentSeasonSettings.growthSpeed * tickMultiplier) == 0)
                    {
                        growthProgressPlant++;
                        UpdatePlantGrowth();
                    }
                }
                else
                {
                    if (hasFruits) plantStage = GrowingStage.Fruiting;
                    else plantStage= GrowingStage.Static;
                }
                break;
            case GrowingStage.Static:
                //ToDo stay still or randomized events
                break;
            case GrowingStage.Fruiting:
                if (growthProgressFruit >= MAX_GROWTH) plantStage = GrowingStage.Static; // if grown up
                else if (IsFruiting() && tick % (currentSeasonSettings.fruitingSpeed * tickMultiplier) == 0)
                {
                    if (fruitCooldown <= 0)
                    {
                        if (!fruits.gameObject.activeSelf) fruits.gameObject.SetActive(true);
                        growthProgressFruit++;
                        UpdateFruitGrowth();
                    }
                    else fruitCooldown--;
                }
                break;
            case GrowingStage.Decaying:
                //ToDo dacaying of plant
                break;
            default:
                break;
        }
    }


    private void UpdateLeavesColors()
    { //might need change to state mashine if we need to do different behavior based on seasons and plant stage
        if (leaves == null) return;
        if (!currentSeasonSettings.haveFoliage)
        {
            if (leaves.gameObject.activeSelf) SetFoliageActive(false);
            return;
        }
        else
        {
            if (!leaves.gameObject.activeSelf) SetFoliageActive();
        }

        float progress = dayInSeason / (float)TimeUtils.GetDaysInSeason(); // current day / days in season
        Color newColor = Utils.CalculateColor(currentSeasonSettings.leavesColorScheme, progress);

        ObjectUtils.SetObjectsColor(newColor, leaves); //ToDo add some randomness to progress
    }
    /// <summary>
    /// Activate or deactivate leaves and fruits based on state entered.
    /// </summary>
    /// <param name="state">Bool that will be set as active status.</param>
    private void SetFoliageActive(bool state = true)
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
        float progress = growthProgressFruit / (float)MAX_GROWTH;
        Color newColor = Utils.CalculateColor(plantSettings.fruitsColorScheme, progress);

        ObjectUtils.SetObjectsColor(newColor, fruits); //ToDo add some randomness to progress
    }
    
    private void UpdatePlantGrowth()
    { //ToDo needs rework in future for better system, maybe with more models for growing stages
        float newScale = Mathf.Lerp(0, defaultScale.x, growthProgressPlant / (float)MAX_GROWTH);
        transform.localScale = new Vector3(defaultScale.x, newScale, newScale);
    }
    private void UpdateFruitGrowth()
    { //In future it would be nice if size of fruits changed as well
        UpdateFruitColors();
    }
    /// <summary>
    /// Check if plant is still growing.
    /// </summary>
    /// <returns>True if it's growing otherwise false.</returns>
    private bool IsGrowing()
    {
        bool isPlantFullyGrown = growthProgressPlant >= MAX_GROWTH;
        //bool isGrowingThisSeason = currentSeasonSettings.growthSpeed > 0;

        return !isPlantFullyGrown;
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
    
    public void CollectFruit()
    {
        growthProgressFruit = 0;
        UpdateFruitColors();
        plantStage = GrowingStage.Fruiting;
        fruits.gameObject.SetActive(false);
        fruitCooldown = currentSeasonSettings.fruitRegrowCooldown;
    }
    public void ResetPlantGrowth()
    {
        growthProgressPlant = 0;
        UpdateFruitColors();
        CollectFruit();
        plantStage = GrowingStage.Growing;
    }

    public void OnClick()
    {
        if (hasFruits)
        { // if plant have fruits
            CollectFruit();
        }
        else
        {// if plant doesn't have fruits

        }
    }
}
