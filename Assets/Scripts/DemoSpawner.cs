using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DemoSpawner : MonoBehaviour
{
    public GameObject demoSpawnedPlants;

    public GameObject[] prefabs;
    public PlantSettings[] berrieSettings;
    public GameObject terrain;

    public float spawnBorder = 1000;
    public float spacing = 10;

    // starting in top left corner, need to decrease coords to fill terrain
    public Vector3 startSpwanPosition = Vector3.zero; 

    public void Initialize()
    {
        demoSpawnedPlants = new GameObject("demoSpawnedPlants");

        Vector3 terrainSize = ObjectUtils.GetObjectSize(terrain);
        startSpwanPosition = new Vector3(terrainSize.x, terrain.transform.position.y, terrainSize.z);
    }
    public void DeleteSpawnedPlants()
    {
        foreach (Transform obj in demoSpawnedPlants.transform)
        {
            DestroyObject(obj.gameObject);
        }
    }
    public void SpawnRandomPlants()
    {
        float xAxis = 0;
        while (xAxis < spawnBorder)
        {
            xAxis += spacing;
            float zAxis = 0;
            while (zAxis < spawnBorder)
            {
                int prefabIndex = SelectRandomPrefab();
                GameObject obj = Instantiate(prefabs[prefabIndex], demoSpawnedPlants.transform);
                if (prefabIndex == 0) obj.GetComponent<PlantController>().plantSettings = SelectRandomSettings();

                zAxis += spacing;
                float objectHeight = ObjectUtils.GetObjectSize(obj).y;
                obj.transform.position = new Vector3(startSpwanPosition.x - xAxis, startSpwanPosition.y + objectHeight, startSpwanPosition.z - zAxis);
            }
        }
    }
    private int SelectRandomPrefab()
    {
        int value = Random.Range(0, prefabs.Length);

        return value;
    }
    private PlantSettings SelectRandomSettings()
    {
        return berrieSettings[Random.Range(0, berrieSettings.Length)];
    }
}
