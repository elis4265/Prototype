using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DayNightCycler))]
public class DayNightCyclerEditor : Editor
{
    DayNightCycler dayNightCycler;
    Editor lightningSettingsEditor;


    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
            }
        }
        DrawSettingsEditor(dayNightCycler.lightningSettings, ref dayNightCycler.lightningSettingsFoldout, ref lightningSettingsEditor);
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
        dayNightCycler = (DayNightCycler)target;
    }
}
