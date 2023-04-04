using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectUtils
{
    /// <summary>
    /// Sets color of object or all children of object.
    /// </summary>
    /// <param name="color">New color.</param>
    /// <param name="objects">Obect to change or parent of objects to change.</param>
    public static void SetObjectsColor(Color color, Transform objects)
    { //ToDo add some randomness
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
}
