using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(ClockHnadler))]
public class TimeManagmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ClockHnadler clockHnadler = (ClockHnadler)target;

        if (GUILayout.Button("Pause/Unpause"))
        {
            clockHnadler.SwapPause();
        }

        if (GUILayout.Button("UpdateLightPosition"))
        {
            clockHnadler.dayNightCycler.UpdateLight();
        }
        GUILayout.Label("Use only when setuping scene.");
        if (GUILayout.Button("SetupLightDefaultPosition"))
        {
            clockHnadler.dayNightCycler.SetupLightDefaultRotation();
        }

    }
}
