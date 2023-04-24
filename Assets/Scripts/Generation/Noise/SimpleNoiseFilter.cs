using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    NoiseSettings.SimpleNoiseSettings settings;
    Noise noise = new Noise();

    public SimpleNoiseFilter(NoiseSettings.SimpleNoiseSettings settings)
    {
        this.settings = settings;
    }
    public float Evaluate(Vector3 point, bool waterNoise = false)
    {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < settings.numOfLayers; i++)
        {
            float v = noise.Evaluate(point * frequency + settings.centre);
            noiseValue += (v + 1) * .5f * amplitude;
            frequency *= settings.roughness;
            amplitude *= settings.persistance;
        }
        
        if (!waterNoise) noiseValue = Mathf.Max(0, noiseValue - settings.minValue);
        else 
            noiseValue = Mathf.Min(noiseValue - settings.minValue, 0);
        return noiseValue * settings.strength;
    }
}
