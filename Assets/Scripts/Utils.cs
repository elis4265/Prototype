using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
