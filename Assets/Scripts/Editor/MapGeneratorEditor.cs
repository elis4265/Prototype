using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    MapGenerator mapGenerator;
    Editor shapeEditor;
    Editor colorEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed) mapGenerator.GenerateMap();
        }
        if (GUILayout.Button("Generate Map")) mapGenerator.GenerateMap();

        DrawSettingsEditor(mapGenerator.shapeSettings, mapGenerator.OnShapeSettingsUpdated, ref mapGenerator.shapeSettingsFoldout, ref shapeEditor);
        //DrawSettingsEditor(mapGenerator.colorSettings, mapGenerator.OnColorSettingsUpdated, ref mapGenerator.colorSettingsFoldout, ref colorEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    { // uncomment once map generation is fixed
                        if (onSettingsUpdated != null) onSettingsUpdated();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        mapGenerator = (MapGenerator)target;
    }
}
