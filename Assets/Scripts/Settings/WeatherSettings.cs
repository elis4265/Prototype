using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class WeatherSettings : ScriptableObject
{
    [Tooltip("Material that is assigned to weather particle system.")]
    public Material particleMaterialRain;
    public Material particleMaterialClouds;

    public Weather[] weathers;

    [System.Serializable]
    public class Weather
    {
        [HideInInspector]
        public string name;
        public bool useParticlesRain;
        public bool useParticlesCloud;

        [ConditionalHide("useParticlesRain"), Header("Rain settings")]
        public float rainHeight = 50f;
        [ConditionalHide("useParticlesRain")]
        public ParticleProperites particleProperitesRain;

        [ConditionalHide("useParticlesCloud"), Header("Clouds settings")]
        public float cloudHeight = 10f;
        [ConditionalHide("useParticlesCloud")]
        public ParticleProperites particleProperitesCloud;
    }
}
