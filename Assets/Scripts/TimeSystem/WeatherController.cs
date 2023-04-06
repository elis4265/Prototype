using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WeatherController : MonoBehaviour
{
    public UniversalAdditionalLightData urpData;

    public GameManager gameManager;
    public DayNightCycler dayNightCycler;
    public WeatherSettings weatherSettings;
    [HideInInspector]
    public bool settingsFoldout;

    public TimeUtils.Weather currentWeather = TimeUtils.Weather.Clear;
    public Vector2 wind = Vector2.zero;
    public float windMultiplier = 50; // bigger number slower wind
    public CloudSettings cloudSettings;

    public GameObject terrain;
    public ParticleSystem particlesRain;
    public ParticleSystem particlesCloud;

    private Texture2D cloudTexture;
    public bool areCloudsOn = false;
    void Start()
    {
        //SetupParticleObjectSizes();
        UpdateWeather();
        // teporary, for changing weather daily
        ClockHnadler.OnDayStart += delegate (object sender, ClockHnadler.OnDayStartEventArgs e)
        {
            currentWeather = (TimeUtils.Weather)UnityEngine.Random.Range(0, Enum.GetNames(typeof(TimeUtils.Weather)).Length);
            UpdateWeather();
            RandomizeWind();
        };
    }

    void Update()
    {
        if(areCloudsOn)
        {
            if (wind != Vector2.zero)
            {
                cloudSettings.offset += wind / windMultiplier;
                cloudTexture = GenerateClouds(cloudSettings);
                UpdateClouds(true);
            }
        }
    }
    public void UpdateWeather()
    {
        SetupWeather(weatherSettings.weathers[(int)currentWeather]);
        if (areCloudsOn) cloudTexture = GenerateClouds(cloudSettings);
        switch (currentWeather)
        {// idk if this is needed
            case TimeUtils.Weather.Clear:
                UpdateClouds();
                break;
            case TimeUtils.Weather.Cloudy:
                UpdateClouds(true);
                break;
            case TimeUtils.Weather.Rain:
                UpdateClouds(true);
                break;
            case TimeUtils.Weather.Storm:
                UpdateClouds(true);
                break;
            case TimeUtils.Weather.Foggy:
                UpdateClouds();
                break;
            case TimeUtils.Weather.Snow:
                UpdateClouds(true);
                break;
            case TimeUtils.Weather.SnowStorm:
                UpdateClouds(true);
                break;
            default:
                break;
        }
        gameManager.weatherDropdown.GetComponent<TMP_Dropdown>().value = (int)currentWeather;
    }

    public void SetupParticleObjectSizes()
    {
        SetupRainParticleObj();
        SetupCloudParticleObj();
    }
    private void SetupRainParticleObj()
    {
        Vector3 newSize = ObjectUtils.GetObjectSize(terrain);
        newSize.y = newSize.z;
        newSize.z = 1f;
        Vector3 newPosition = ObjectUtils.GetObjectCenter(terrain);
        newPosition.y = 50f;// height of object
        particlesRain.transform.position = newPosition;
        particlesRain.transform.localEulerAngles = new Vector3(90f, 0);
        var sh = particlesRain.shape;
        sh.enabled = true;
        sh.scale = newSize;
        sh.shapeType = ParticleSystemShapeType.Box;
    }
    private void SetupCloudParticleObj()
    {
        float height = 10;
        Vector3 newSize = new Vector3(ObjectUtils.GetObjectSize(terrain).x,height,1);
        Vector3 newPosition = new Vector3(ObjectUtils.GetObjectCenter(terrain).x, terrain.transform.position.y + height, terrain.transform.position.z);
        particlesCloud.transform.position = newPosition;
        var sh = particlesCloud.shape;
        sh.enabled = true;
        sh.scale = newSize;
        sh.shapeType = ParticleSystemShapeType.Box;
    }
    public void SetWeather(TimeUtils.Weather newWeather)
    {
        currentWeather = newWeather;
        UpdateWeather();
    }
    private void SetupWeather(WeatherSettings.Weather weather)
    {
        if (weather.useParticlesRain)
        {
            particlesRain.Play();
            Utils.SetupParticleSystem(particlesRain, weather.particleProperitesRain, weatherSettings.particleMaterialRain);
        }
        else Utils.StopParticles(particlesRain);
        if (weather.useParticlesCloud)
        {
            particlesCloud.Play();
            Utils.SetupParticleSystem(particlesCloud, weather.particleProperitesCloud, weatherSettings.particleMaterialClouds);
        }
        else Utils.StopParticles(particlesCloud);
    }
    private void UpdateClouds(bool cloudsOn = false)
    {
        areCloudsOn = cloudsOn;
        if (cloudsOn)
        {
            if (/*dayNightCycler.ligthSourceDay.GetComponent<Light>().cookie == null*/true)
            {
                dayNightCycler.ligthSourceDay.GetComponent<Light>().cookie = cloudTexture;
                dayNightCycler.ligthSourceNight.GetComponent<Light>().cookie = cloudTexture;
            }
        }
        else
        {
            dayNightCycler.ligthSourceDay.GetComponent<Light>().cookie = null;
            dayNightCycler.ligthSourceNight.GetComponent<Light>().cookie = null;
        }
    }
    private Texture2D GenerateClouds(CloudSettings cls)
    {
        return CloudGenerator.GenerateTexture(cls.resolution, cls.scale, cls.offset);
    }
    private void RandomizeWind()
    { // probbably needs more suitable version of wind randomness
        wind = new Vector2(UnityEngine.Random.Range(-1,1), UnityEngine.Random.Range(-1, 1));
    }
}
[System.Serializable]
public class CloudSettings
{
    public int resolution = 256;
    public float scale = 1;
    public Vector2 offset;
}
