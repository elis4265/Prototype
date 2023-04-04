using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightUtils
{
    private enum State { morning, evening, storm }

    public static void SetLightColor(GameObject light, Color color)
    {
        light.GetComponent<Light>().color = color;
    }
    public static void SetLightIntesity(GameObject light, float inteity)
    {
        light.GetComponent<Light>().intensity = inteity;
    }
    public static float CalculateDot(Transform lightSource)
    {
        return Vector3.Dot(lightSource.forward, Vector3.down);
    }
}
