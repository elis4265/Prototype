using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeatherController))]
public class WeatherEditor : Editor
{
    WeatherController weatherController;
    Editor weatherSettingsEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            { // update weather if something in inspector changed
                weatherController.UpdateWeather();
            }
        }

        if (GUILayout.Button("Setup particles"))
        {
            weatherController.SetupParticleObjectSizes();
        }

        SetWeatherNames();
        DrawSettingsEditor(weatherController.weatherSettings, ref weatherController.settingsFoldout, ref weatherSettingsEditor);
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
        weatherController = (WeatherController)target;
    }

    private void SetWeatherNames()
    {
        ref WeatherSettings.Weather[] weathers = ref weatherController.weatherSettings.weathers;

        for (int i = 0; i < weathers.Length; i++)
        {
            weathers[i].name = ((TimeUtils.Weather)i).ToString();
        }
    }
}
