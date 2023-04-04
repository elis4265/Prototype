using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu()]
public class PlantSettings : ScriptableObject
{
    [HideInInspector]
    public bool hasFruits;
    [ConditionalHide("hasFruits")]
    public Gradient fruitsColorScheme;
    [ConditionalHide("hasFruits"), Range(0,10)]
    public int harvestCooldown = 1;
    public SeasonSettings[] seasonSettings = new SeasonSettings[4];


    //this file should carrt all necessary information about plant for each season, one setting should be created for each plant unless they are too similiar
    [System.Serializable]
    public class SeasonSettings
    {
        [ConditionalHide("hasFruits")]
        [Range(0, 10)]
        public int fruitingSpeed;
        [ConditionalHide("hasFruits")]
        public int fruitRegrowCooldown = 10;
        [Range(0, 10)]
        public int growthSpeed;
        public int resourceAmount = 0;
        public Gradient leavesColorScheme;
        public bool haveFoliage = true;
    }
}
