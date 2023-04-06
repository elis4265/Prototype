using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlantController))]
public class PlantControllerEditor : Editor
{
    PlantController plantController;
    Editor plantSettingsEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                plantController.plantSettings.hasFruits = plantController.hasFruits;
            }
        }

        if (plantController.plantSettings.hasFruits)
        {
            if (GUILayout.Button("Collect berries"))
            {
                plantController.CollectFruit();
            }
        }

        if (GUILayout.Button("Reset growth"))
        {
            plantController.ResetPlantGrowth();
        }
        DrawSettingsEditor(plantController.plantSettings, ref plantController.plantSettingsFoldout, ref plantSettingsEditor);
    }


    void DrawSettingsEditor(Object settings, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            if (foldout)
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();
            }
        }
    }

    private void OnEnable()
    {
        plantController = (PlantController)target;
    }
}
