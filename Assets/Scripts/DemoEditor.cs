using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DemoSpawner))]
public class DemoEditor : Editor
{
    DemoSpawner demoSpawner;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Initialize"))
        {
            demoSpawner.Initialize();
        }
        if (GUILayout.Button("Respawn Plants"))
        {
            demoSpawner.DeleteSpawnedPlants();
            demoSpawner.SpawnRandomPlants();
        }
        if (GUILayout.Button("Delete Plants"))
        {
            demoSpawner.DeleteSpawnedPlants();
        }
    }

    private void OnEnable()
    {
        demoSpawner = (DemoSpawner)target;
    }
}
