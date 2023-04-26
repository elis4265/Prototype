using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;

public class TerrainFace
{
    ShapeGenerator shapeGenerator;
    Mesh mesh;
    Mesh waterMesh;
    int resolution;
    Vector3 localUp;
    Vector3 axisA;
    Vector3 axisB;
    Vector2 terrainOffset;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, Mesh waterMesh, int resolution, Vector3 localUp, Vector2 terrainOffset)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        this.waterMesh = waterMesh;

        this.terrainOffset = terrainOffset;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh()
    {
        Vector3 positionOffset = terrainOffset * (2 * shapeGenerator.GetPlanetScale());
        positionOffset = new Vector3(positionOffset.x, 0, positionOffset.y);
        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector3[] waterVertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = mesh.uv;

        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                int i = x+ y *resolution; //numbers of loops passed
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;
                pointOnUnitCube += positionOffset;
                vertices[i] = shapeGenerator.CalculatePointOnMapHeight(pointOnUnitCube);
                waterVertices[i] = pointOnUnitCube * shapeGenerator.GetPlanetScale();
                vertices[i].y -= shapeGenerator.GetPlanetScale(); //keeps height at 0 when scaling
                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;

                    triIndex += 6;
                }
            }
        }
        waterMesh.Clear();
        waterMesh.vertices = waterVertices;
        waterMesh.triangles = triangles;
        waterMesh.RecalculateNormals();

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.uv = uv;
        
    }

    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[resolution*resolution];
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                int i = x + y * resolution; //numbers of loops passed
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = localUp + (percent.x - .5f) * 2 * axisA + (percent.y - .5f) * 2 * axisB;

                uv[i] = new Vector2(colorGenerator.BiomePercentFromPoint(pointOnUnitCube), 0);
            }
        }
        mesh.uv = uv;
    }
    /// <summary>
    /// scales point without chaning height
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private Vector3 ScalePoint(Vector3 point)
    {
        Vector3 newPoint = point * shapeGenerator.GetPlanetScale();
        newPoint.y = point.y;
        return newPoint;
    }
}