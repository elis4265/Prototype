using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGeneration : MonoBehaviour
{
    public int width = 500; //x-axis of the terrain
    public int height = 500; //z-axis

    public int depth = 10; //y-axis

    public float scale = 20f;

    public float offsetX = 50f;
    public float offsetY = 100f;

    private void Start()
    {
        
        offsetX = Random.Range(0f, 9999f);
        offsetY = Random.Range(0f, 9999f);
        var terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    private void OnApplicationQuit()
    {
        var terrain = GetComponent<Terrain>();
    }

    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);

        terrainData.SetHeights(0, 0, GenerateHeights());
        print(terrainData);
        return terrainData;
    }

    private float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }
        return heights;
    }

    private float CalculateHeight (int x, int y)
    {
        var xCoord = (float)x / width * scale + offsetX;
        var yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}

