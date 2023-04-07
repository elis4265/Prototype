using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public static class CloudGenerator 
{
    public static Texture2D GenerateTexture(int resolution, float scale, Vector2 offset)
    {
        Texture2D texture = new Texture2D(resolution, resolution);

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                Color color = CalculateColor(new int2(x,y), resolution, scale, offset);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        return texture;
    }

    private static Color CalculateColor(int2 coords, float resolution, float scale, Vector2 offset)
    {
        float xCoord = coords.x / (float)resolution * scale + offset.x;
        float yCoord = coords.y / (float)resolution * scale + offset.y;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }
    private static float TileWrapper()
    { // wrapping coockie map 
        float newVal = 0;


         
        return newVal;
    }
}
