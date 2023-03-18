using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(ClockHnadler))]
public class TimeManagmentEditor : Editor
{
    private void OnEnable()
    {
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ClockHnadler clockHnadler = (ClockHnadler)target;
        EditorGUILayout.LabelField("faster<x<slower");
        if (GUILayout.Button("UpdateTimeSpeed"))
        {
            clockHnadler.UpdateTimeSpeed();
        }

        if (GUILayout.Button("Pause/Unpause"))
        {
            clockHnadler.SwapPause();
        }

        if (GUILayout.Button("UpdateLightPosition"))
        {
            clockHnadler.dayNightCycler.SetLightAngleBasedOnTime(clockHnadler.time);
        }

    }
}
