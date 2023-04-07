using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

// To do, set random value as offset when instantiating plants and apply it to leaves gradient value calculation (t in lerp) so its not so uniformal
// there is combining leaves colliders for demo testing, might need to be removed at some point
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
    public int tickMultiplier = 1;

    [ConditionalHide("hasFruits"), Range(0, 100)]
    public int growthProgressFruit = 0;// out of 100
    private int growthTickFruit = 0;
    [ConditionalHide("hasFruits")]
    public int fruitCooldown = 0;// time between collecting and growing again
    [Range(0, 100), SerializeField]
    private int growthProgressPlant = 100;// out of 100
    private int growthTickPlant = 0;

    [SerializeField]
    private GrowingStage plantStage = GrowingStage.Growing;

    [HideInInspector]
    public bool plantSettingsFoldout; // for custom editor, making folding of settings work

    void Start()
    {
        Initiate();
        SetupColliders();
        // for daily updating
        ClockHnadler.OnDayStart += DayUpdate;
        // Update each tick
        TimeTickSystem.OnTick += TickUpdate;
        // Update each new minute
        ClockHnadler.OnMinuteStart += MinuteUpdate;
    }

    private void SetupColliders()
    {
        if (leaves != null && leaves.GetComponent<MeshCollider>() == null)
        {
            if (leaves.childCount > 0)
            {
                //leaves.AddComponent<MeshCollider>().sharedMesh = ObjectUtils.CombineChildMeshes(leaves);
                foreach (Transform child in leaves)
                {   
                    if(child.GetComponent<MeshCollider>() != null) continue;
                    child.AddComponent<MeshCollider>().sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
                }
            }
            else leaves.AddComponent<MeshCollider>().sharedMesh = leaves.GetComponent<MeshFilter>().sharedMesh;
        }
        //if (fruits != null && fruits.GetComponent<MeshCollider>() == null)
        //{
        //    if (fruits.childCount > 0)
        //    {
        //        //fruits.AddComponent<MeshCollider>().sharedMesh = ObjectUtils.CombineChildMeshes(fruits);
        //        foreach(Transform child in fruits)
        //        {
        //            if(child.GetComponent<MeshCollider>() != null) continue;
        //            child.AddComponent<MeshCollider>().sharedMesh = child.GetComponent<MeshFilter>().sharedMesh;
        //        }
        //    }
        //    else fruits.AddComponent<MeshCollider>().sharedMesh = fruits.GetComponent<MeshFilter>().sharedMesh;
        //}
    }

    private void Initiate()
    {
        hasFruits = plantSettings.hasFruits;
        defaultScale = transform.localScale;
        currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        UpdateLeavesColors();
        if (hasFruits) UpdateFruitGrowth();
        UpdatePlant();
    }
    private void TickUpdate(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        //UpdatePlant();
    }
    private void MinuteUpdate(object sender, ClockHnadler.OnMinuteStartEventArgs e)
    {
        UpdateGrowthTicks();
        UpdatePlant();
    }
    private void DayUpdate(object sender, ClockHnadler.OnDayStartEventArgs e)
    {
        currentSeason = e.season;
        currentSeasonSettings = plantSettings.seasonSettings[currentSeason];
        dayInSeason = TimeUtils.GetDayInSeason(e.date);
        UpdateLeavesColors();
    }
    private void UpdatePlant()
    {
        switch (plantStage)
        {
            case GrowingStage.Growing:
                if (IsGrowing())
                {
                    if (fruits.gameObject.activeSelf) fruits.gameObject.SetActive(false);
                    if (growthTickPlant % (currentSeasonSettings.growthSpeed * tickMultiplier) == 0)
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
                else if (IsFruiting() && growthTickFruit % (currentSeasonSettings.fruitingSpeed * tickMultiplier) == 0)
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

    private void UpdateGrowthTicks()
    {
        if (IsFruiting()) growthTickFruit++;
        if (IsGrowing()) growthTickPlant++;
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
    private void UpdateFruitColors(float growthProgress)
    {
        if (fruits == null || !fruits.gameObject.activeSelf) return;
        Color newColor = Utils.CalculateColor(plantSettings.fruitsColorScheme, growthProgress);

        ObjectUtils.SetObjectsColor(newColor, fruits); //ToDo add some randomness to progress
    }
    private void UpdateFruitSize(float growthProgress)
    {
        Vector3 maxScale = plantSettings.fruitFullScale;
        float scaleStepX, scaleStepY, scaleStepZ;

        if (maxScale.x == maxScale.y && maxScale.x == maxScale.z)
            scaleStepX = scaleStepY = scaleStepZ = Mathf.Lerp(0, plantSettings.fruitFullScale.x, growthProgress);
        else
        {
            scaleStepX = Mathf.Lerp(0, plantSettings.fruitFullScale.x, growthProgress);
            scaleStepY = Mathf.Lerp(0, plantSettings.fruitFullScale.y, growthProgress);
            scaleStepZ = Mathf.Lerp(0, plantSettings.fruitFullScale.z, growthProgress);
        }
        Vector3 newScale = new Vector3(scaleStepX, scaleStepY, scaleStepZ);
        ObjectUtils.SetObjectsScale(newScale, fruits);
    }
    private void UpdatePlantGrowth()
    { //ToDo needs rework in future for better system, maybe with more models for growing stages
        float newScale = Mathf.Lerp(0, defaultScale.x, growthProgressPlant / (float)MAX_GROWTH);
        transform.localScale = new Vector3(defaultScale.x, newScale, newScale);
    }
    private void UpdateFruitGrowth()
    { //In future it would be nice if size of fruits changed as well
        float progress = growthProgressFruit / (float)MAX_GROWTH;

        UpdateFruitColors(progress);
        UpdateFruitSize(progress);
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
        UpdateFruitGrowth();
        plantStage = GrowingStage.Fruiting;
        fruits.gameObject.SetActive(false);
        fruitCooldown = currentSeasonSettings.fruitRegrowCooldown;
    }
    public void ResetPlantGrowth()
    {
        growthProgressPlant = 0;
        UpdateFruitGrowth();
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
    private void OnDestroy()
    {
        ClockHnadler.OnDayStart -= DayUpdate;
        TimeTickSystem.OnTick -= TickUpdate;
        ClockHnadler.OnMinuteStart -= MinuteUpdate;
    }
}
