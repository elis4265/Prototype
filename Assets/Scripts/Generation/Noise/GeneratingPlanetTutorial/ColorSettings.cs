using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ColorSettings : ScriptableObject
{
    public Material planetMaterial;
    public Material waterMaterial;
    public BiomeColorSettings biomeColorSettings;

    [System.Serializable]
    public class BiomeColorSettings
    {
        public Biome[] biomes;
        public NoiseSettings noise;
        public float noiseOffset;
        public float noiseStrenght;
        [Range(0, 1)]
        public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradientAboveWater;
            public Gradient gradientUnderWater;
            public Color tint;
            [Range(0, 1)]
            public float startHeight;
            [Range(0, 1)]
            public float tintPercent;
        }
    }
}
