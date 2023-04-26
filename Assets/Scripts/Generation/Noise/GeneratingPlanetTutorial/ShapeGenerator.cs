using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    INoiseFilter[] noiseFilters;
    INoiseFilter[] waterNoiseFilters;
    public MinMax elevationMinMax;

    public void UpdateSettings(ShapeSettings shapeSettings)
    {
        this.settings = shapeSettings;
        noiseFilters = new INoiseFilter[settings.noiseLayers.Length];
        waterNoiseFilters = new INoiseFilter[settings.waterNoiseLayers.Length];

        for (int i = 0; i < noiseFilters.Length; i++)
        {
            noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
        for (int i = 0; i < waterNoiseFilters.Length; i++)
        {
            waterNoiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.waterNoiseLayers[i].noiseSettings);
        }

        elevationMinMax = new MinMax();
    }
    public Vector3 CalculatePointOnMapHeight(Vector3 pointOnUnitMap) { return CalculatePointOnMap(pointOnUnitMap); }
    public Vector3 CalculatePointOnMap(Vector3 pointOnUnitMap) //optimized for radial planet generating
    {
        float firstLayarValue = 0;
        float waterLayerMask = 0;
        float elevation = 0;

        if (noiseFilters.Length > 0)
        {
            firstLayarValue = noiseFilters[0].Evaluate(pointOnUnitMap);
            if (settings.noiseLayers[0].enabled) elevation = firstLayarValue;
        }
        if (waterNoiseFilters.Length > 0)
        {
            waterLayerMask = waterNoiseFilters[0].Evaluate(pointOnUnitMap, true);
        }

        for (int i = 1; i < noiseFilters.Length; i++)
        {
            if (settings.noiseLayers[i].enabled)
            {
                float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayarValue : 1;
                elevation += noiseFilters[i].Evaluate(pointOnUnitMap) * mask;
            }
        }
        if (elevation == 0)
        {
            if (settings.waterNoiseLayers != null && settings.waterNoiseLayers.Length > 0 &&
                settings.waterNoiseLayers[0].enabled) elevation = waterLayerMask;
            for (int i = 1; i < waterNoiseFilters.Length; i++)
            {
                if (settings.waterNoiseLayers[i].enabled)
                {
                    float mask = (settings.waterNoiseLayers[i].useFirstLayerAsMask) ? waterLayerMask : 1;
                    elevation -= waterNoiseFilters[i].Evaluate(pointOnUnitMap, true) * mask;
                }
            }
        }
        elevation = settings.planetRadius * (1 + elevation);

        elevationMinMax.AddValue(elevation);

        return new Vector3(pointOnUnitMap.x * settings.planetRadius, pointOnUnitMap.y * elevation, pointOnUnitMap.z * settings.planetRadius);
    }

    public float GetPlanetScale() { return settings.planetRadius; }
}
