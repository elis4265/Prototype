using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Utils
{
    /// <summary>
    /// Calculate color from gradinet in position of time.
    /// </summary>
    /// <param name="gradient">Gradient to get color.</param>
    /// <param name="progress">Value of progress. (Max/curr)</param>
    /// <returns>Color from gradient based on progress</returns>
    public static Color CalculateColor(Gradient gradient, float progress)
    {
        float t = Mathf.Lerp(0, 1, progress);
        return gradient.Evaluate(t);
    }


    /// <summary>
    /// Stops and cleanup particle animation.
    /// </summary>
    /// <param name="ps"></param>
    public static void StopParticles(ParticleSystem ps)
    {
        ps.Stop();
        ps.Clear();
    }
    public static void SetupParticleSystem(ParticleSystem particle, ParticleProperites properties, Material particleMaterial = null)
    {
        particle.GetComponent<ParticleSystemRenderer>().sharedMaterial.SetTexture("_MainTex", properties.particleSprite);

        var main = particle.main;
        main.startSize = properties.size;
        main.startLifetime = properties.lifeTime;
        main.gravityModifier = properties.fallSpeed;

        var emission = particle.emission;
        emission.rateOverTime = properties.rateOverTime;

        var velocityOverLifetime = particle.velocityOverLifetime;
        velocityOverLifetime.enabled = properties.hasVelocity;
        if (properties.hasVelocity)
        {
            velocityOverLifetime.orbitalX = properties.orbitalVelocity.x;
            velocityOverLifetime.orbitalY = properties.orbitalVelocity.y;
            velocityOverLifetime.orbitalZ = properties.orbitalVelocity.z;

            velocityOverLifetime.x = properties.linearVelocity.x;
            velocityOverLifetime.y = properties.linearVelocity.y;
            velocityOverLifetime.z = properties.linearVelocity.z;
        }

        var rotationOverTime = particle.rotationOverLifetime;
        rotationOverTime.enabled = properties.hasRotation;
        if (properties.hasRotation)
        {
            rotationOverTime.z = properties.rotationOverLifetime / 60f; //this might need fixing
        }
    }

    public static void SetParticleSystemSize(ParticleSystem particleSystem, Vector3 size, ParticleSystemShapeType psShape = ParticleSystemShapeType.Box, bool enabled = true)
    {
        var sh = particleSystem.shape;
        sh.enabled = enabled;
        sh.scale = size;
        sh.shapeType = psShape;
    }

    public static void SetupWeatherDropDown(GameObject dropDown)
    {
        dropDown.GetComponent<TMP_Dropdown>().ClearOptions();

        List<string> options = new List<string>();
        foreach (var item in Enum.GetNames(typeof(TimeUtils.Weather)))
        {
            options.Add(item);
        }

        dropDown.GetComponent<TMP_Dropdown>().AddOptions(options);
    }
}
[System.Serializable]
public class ParticleProperites
{
    public Texture2D particleSprite;
    public float size = 1f;
    public float lifeTime = 5f;
    public float fallSpeed = 0f; // gravity modifier in particle system
    public float rateOverTime = 50f; // number of particles over time

    public bool hasVelocity = false;
    [ConditionalHide("hasVelocity")]
    public Vector3 orbitalVelocity = Vector3.zero;// will be usefull for things like tornado or similliar
    [ConditionalHide("hasVelocity")]
    public Vector3 linearVelocity = Vector3.zero;// will be usefull for windy rain or similliar

    public bool hasRotation = false;
    [ConditionalHide("hasRotation")]
    public float rotationOverLifetime = 0f; //approximate value of degrees per second or something
    // public bool noise = false; // could be usefull for snowing or something 
    // public bool collision = false; // could be usefull for hailing or natural disasters
    // public bool lights = false; // could be usefull for ligtning or radioactive fallout or shit
    // public bool trails = false; // could be usefull for rain
}