using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlantSettings : ScriptableObject
{
    public SeasonSettings[] seasonSettings = new SeasonSettings[4];

    //this file should carrt all necessary information about plant for each season, one setting should be created for each plant unless they are too similiar
    [System.Serializable]
    public class SeasonSettings
    {
        [Range(0, 10)]
        public int fruitingSpeed;
        public bool harvestable = false;
        public int resourceAmount = 0;
        public Gradient colorScheme;
        public bool haveFoliage = false;
    }
}
