using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LightningSettings : ScriptableObject
{//possibility to create custom settings for each season
    //public Gradient dayLightGradient;
    //public Vector3 dayLightIntensity; // format (morning, noon, evening)
    //public Gradient nightLightGradient;
    //public Vector3 nightLightIntensity; // format (evening, midnight, morning)

    public LightStateSettings[] lightSattesSettings; 

    [System.Serializable]
    public class LightStateSettings
    {
        public Gradient lightGradient;
        public Vector3 lightIntensity;
    }

}
