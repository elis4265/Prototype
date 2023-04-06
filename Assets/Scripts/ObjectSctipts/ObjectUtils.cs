using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ObjectUtils
{
    /// <summary>
    /// Sets color of object or all children of object.
    /// </summary>
    /// <param name="color">New color.</param>
    /// <param name="objects">Obect to change or parent of objects to change.</param>
    public static void SetObjectsColor(Color color, Transform objects)
    { 
        if (objects.childCount == 0)
        {
            if (!objects.GetComponent<MeshRenderer>()) throw new System.Exception($"Object you tired to recolor, {objects.name} desn't have MeshRenderer.");
            objects.GetComponent<MeshRenderer>().material.color = color;
        }
        else
        {
            foreach (Transform obj in objects)
            {
                for (int i = 0; i < obj.GetComponent<MeshRenderer>().materials.Length; i++)
                {
                    if (!obj.GetComponent<MeshRenderer>()) throw new System.Exception($"Object you tired to recolor, {objects.name} desn't have MeshRenderer.");
                    obj.GetComponent<MeshRenderer>().materials[i].color = color;
                }
            }
        }
    }

    /// <summary>
    /// Spawns 3 cubes. 
    /// Cube1 on object position.
    /// Cube2 on object center.
    /// Cube3 of object size.
    /// </summary>
    /// <param name="obj"></param>
    public static void TestObjectPositions(GameObject obj)
    {
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube1.name = ("Cube1, position");
        cube1.transform.position = GetObjectPosition(obj);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.name = ("Cube2, center");
        cube2.transform.position = GetObjectCenter(obj);
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube3.name = ("Cube3, size");
        cube3.transform.position = GetObjectPosition(obj);
        cube3.transform.position = GetObjectCenter(obj);
        cube3.transform.position = new Vector3(cube2.transform.position.x, 0, cube2.transform.position.z);
        CopyObjectSize(cube3, obj);
    }
    /// <summary>
    /// Get size of object form Terrain or MeshRenderer property.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Vector3 GetObjectSize(GameObject obj)
    {
        if (obj.GetComponent<Terrain>()) return obj.GetComponent<Terrain>().terrainData.size;
        else if (obj.GetComponent<MeshRenderer>()) return obj.GetComponent<MeshRenderer>().bounds.size;
        return Vector2.zero;
    }
    public static Vector3 GetObjectPosition(GameObject obj)
    {
        return obj.transform.position;
    }
    /// <summary>
    /// Claculates center of an object based on its position and size.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static Vector3 GetObjectCenter(GameObject obj)
    {
        Vector3 center = GetObjectPosition(obj);
        center += GetObjectSize(obj) / 2f;
        return center;
    }
    /// <summary>
    /// Set scale of objectCopyTo to size of objectCopyFrom.
    /// If keepYOne is true, then set y axis to 1.
    /// </summary>
    /// <param name="objectCopyTo"></param>
    /// <param name="objectCopyFrom"></param>
    /// <param name="keepYOne"></param>
    public static void CopyObjectSize(GameObject objectCopyTo, GameObject objectCopyFrom, bool keepYOne = true)
    {
        Vector3 newSize = GetObjectSize(objectCopyFrom);
        if (keepYOne) newSize.y = 1f;
        objectCopyTo.transform.localScale = newSize;
    }
}
