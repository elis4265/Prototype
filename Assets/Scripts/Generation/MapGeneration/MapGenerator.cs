using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public bool generateWaterSurface = true;

    public int chunks = 1;

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
    

    public void Initialize()
    {
        shapeGenerator.UpdateSettings(shapeSettings);
        //colorGenerator.UpdateSettings(colorSettings);

        if (meshFilters == null || meshFilters.Length != chunks) meshFilters = new MeshFilter[chunks];
        if (waterMeshFilters == null || waterMeshFilters.Length != chunks) waterMeshFilters = new MeshFilter[chunks];
        terrainFaces = new TerrainFace[chunks];

        for (int i = 0; i < chunks; i++)
        {
            if (meshFilters[i] == null) // to do add or meshwaterfilters and cleaning of chidren
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
                meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = demoMaterial; //assigning demo material for now
            }
            if (waterMeshFilters[i] == null) // to do add or meshwaterfilters and cleaning of chidren
            {
                GameObject meshWaterObj = new GameObject("meshWater");
                meshWaterObj.transform.parent = transform;

                meshWaterObj.AddComponent<MeshRenderer>();
                waterMeshFilters[i] = meshWaterObj.AddComponent<MeshFilter>();
                waterMeshFilters[i].sharedMesh = new Mesh();
                waterMeshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = demoMaterial; //assigning demo material for now
            }
            //meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            //waterMeshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.waterMaterial;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, waterMeshFilters[i].sharedMesh, resolution, Vector3.up);//directions[i]);
            bool renderFace = true; // calculate if face should be rendered
            meshFilters[i].gameObject.SetActive(renderFace);
            waterMeshFilters[i].gameObject.SetActive(renderFace && generateWaterSurface);
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
}
