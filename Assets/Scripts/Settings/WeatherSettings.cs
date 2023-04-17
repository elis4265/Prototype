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
    public Material[] particleMaterials;
    //public Material particleMaterialRain;
    //public Material particleMaterialSnow;
    //public Material particleMaterialClouds;
    //public Material particleMaterialGround;

    public Weather[] weathers;

    [System.Serializable]
    public class Weather
    {
        [HideInInspector]
        public string name;
        public bool areCloudsOn = false;
        public bool useParticlesRain;
        public bool useParticlesCloud;
        public bool useParticlesGround;

        [ConditionalHide("useParticlesRain"), Header("Rain settings")]
        public float rainHeight = 50f;
        [ConditionalHide("useParticlesRain")]
        public bool applyWindToRain = true;
        [ConditionalHide("useParticlesRain")]
        public ParticleProperites particleProperitesRain;

        [ConditionalHide("useParticlesCloud"), Header("Clouds settings")]
        public float cloudHeight = 10f;
        [ConditionalHide("useParticlesCloud")]
        public ParticleProperites particleProperitesCloud;

        [ConditionalHide("useParticlesGround"), Header("Ground fog settings")]
        public float groundFog = 0f;
        [ConditionalHide("useParticlesGround")]
        public ParticleProperites particleProperitesGround;
    }
}
