using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public bool generateWaterSurface = true;

    public int chunksResolution = 1;
    int chunks = 1;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    MeshFilter[] waterMeshFilters;
    TerrainFace[] terrainFaces;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    //public ColorSettings colorSettings;
    public ShapeSettings shapeSettings;

    public Material demoMaterial;

    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;
    private bool debugInfo = false;

    public void Initialize()
    {
        chunks = chunksResolution * chunksResolution;
        shapeGenerator.UpdateSettings(shapeSettings);
        //colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length != chunks)
        {
            if (meshFilters != null) DeleteOldMeshes();
            meshFilters = new MeshFilter[chunks];
            waterMeshFilters = new MeshFilter[chunks];
        }
        terrainFaces = new TerrainFace[chunks];

        int index = 0;
        for (int x = 0; x < chunksResolution; x++)
        {
            for (int y = 0; y < chunksResolution; y++)
            {
                if (debugInfo)Debug.Log($"x: {x}, y {y}, index: {index}");
                if (meshFilters[index] == null) // to do add or meshwaterfilters and cleaning of chidren
                {
                    GameObject meshObj = new GameObject("mesh " + index);
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>();
                    meshFilters[index] = meshObj.AddComponent<MeshFilter>();
                    meshFilters[index].sharedMesh = new Mesh();
                    meshFilters[index].GetComponent<MeshRenderer>().sharedMaterial = demoMaterial; //assigning demo material for now
                }
                if (waterMeshFilters[index] == null) // to do add or meshwaterfilters and cleaning of chidren
                {
                    GameObject meshWaterObj = new GameObject("meshWater " + index);
                    meshWaterObj.transform.parent = transform;

                    meshWaterObj.AddComponent<MeshRenderer>();
                    waterMeshFilters[index] = meshWaterObj.AddComponent<MeshFilter>();
                    waterMeshFilters[index].sharedMesh = new Mesh();
                    waterMeshFilters[index].GetComponent<MeshRenderer>().sharedMaterial = demoMaterial; //assigning demo material for now
                }
                //meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
                //waterMeshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.waterMaterial;
                terrainFaces[index] = new TerrainFace(shapeGenerator, meshFilters[index].sharedMesh, waterMeshFilters[index].sharedMesh, resolution, Vector3.up, new Vector2(x,y));//directions[i]);
                bool renderFace = true; // calculate if face should be rendered
                meshFilters[index].gameObject.SetActive(renderFace);
                waterMeshFilters[index].gameObject.SetActive(renderFace && generateWaterSurface);
                index++;
            }
        }
    }
    public void GenerateMap()
    { //commented so nothing will generate
        Initialize();
        GenerateMesh();
        //GenerateColors();
    }
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            //GenerateColors();
        }
    }

    public void GenerateMesh()
    {
        if(debugInfo)Debug.Log($"chunks: {chunks}, resolution {chunksResolution}, meshes: {meshFilters.Length}");
        for (int i = 0; i < chunks; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }
        //colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }
    //public void GenerateColors()
    //{
    //    colorGenerator.UpdateColors();

    //    for (int i = 0; i < 6; i++)
    //    {
    //        if (meshFilters[i].gameObject.activeSelf)
    //        {
    //            terrainFaces[i].UpdateUVs(colorGenerator);
    //        }
    //    }
    //}
    public void DeleteOldMeshes()
    {
        foreach (var mesh in meshFilters)
        {
            DestroyImmediate(mesh.gameObject);
        }
        foreach (var mesh in waterMeshFilters)
        {
            DestroyImmediate(mesh.gameObject);
        }
    }
}
