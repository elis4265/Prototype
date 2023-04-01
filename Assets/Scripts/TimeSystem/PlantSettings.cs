using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlantSettings : ScriptableObject
{
    public bool hasFruits = false;
    public SeasonSettings[] seasonSettings = new SeasonSettings[4];


    //this file should carrt all necessary information about plant for each season, one setting should be created for each plant unless they are too similiar
    [System.Serializable]
    public class SeasonSettings
    {
        [ConditionalHide("hasFruits")]
        [Range(0, 10)]
        public int fruitingSpeed;
        [Range(0, 10)]
        public int growthSpeed;
        public int resourceAmount = 0;
        public Gradient leavesColorScheme;
        [ConditionalHide("hasFruits")]
        public Gradient fruitsColorScheme;
        public bool haveFoliage = false;
    }
}
