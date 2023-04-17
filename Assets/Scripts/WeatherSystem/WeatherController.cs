using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;
//need to get mesh from terrain and set it as shape ========================================================================================================

public class WeatherController : MonoBehaviour
{
    private enum ParticleTypes { rain, cloud, ground }

    public bool showSetupSettings = false;


    [ConditionalHide("hideSetupSettings")]
    public GameManager gameManager;
    [ConditionalHide("hideSetupSettings")]
    public DayNightCycler dayNightCycler;
    [ConditionalHide("hideSetupSettings")]
    public WeatherSettings weatherSettings;
    [HideInInspector]
    public bool settingsFoldout;

    public TimeUtils.Weather currentWeather = TimeUtils.Weather.Clear;
    public Vector2 wind = Vector2.zero;
    public float windMultiplier = 50; // bigger number slower wind
    public bool areCloudsOn = false;
    [ConditionalHide("areCloudsOn")]
    public CloudSettings cloudSettings;

    [ConditionalHide("hideSetupSettings")]
    public GameObject terrain;
    [ConditionalHide("hideSetupSettings")]
    public ParticleSystem particlesRain;
    [ConditionalHide("hideSetupSettings")]
    public ParticleSystem particlesCloud;
    [ConditionalHide("hideSetupSettings")]
    public ParticleSystem particlesGround;

    private Texture2D cloudTexture;
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
                break;
            case TimeUtils.Weather.Cloudy:
                break;
            case TimeUtils.Weather.Rain:
                break;
            case TimeUtils.Weather.Storm:
                break;
            case TimeUtils.Weather.Foggy:
                break;
            case TimeUtils.Weather.Snow:
                break;
            case TimeUtils.Weather.SnowStorm:
                break;
            default:
                break;
        }
        gameManager.weatherDropdown.GetComponent<TMP_Dropdown>().value = (int)currentWeather;
    }

    public void SetupParticleObjectSizes()
    {
        SetupParticle(ParticleTypes.rain, particlesRain);
        SetupParticle(ParticleTypes.cloud, particlesCloud);
        SetupParticle(ParticleTypes.ground, particlesGround);
    }

    private void SetupParticle(ParticleTypes particleType, ParticleSystem particleSystem)
    {
        Vector3 newSize = Vector3.one;
        Vector3 newPosition = Vector3.one;
        Vector3 terrainSize = ObjectUtils.GetObjectSize(terrain);
        float height = 10;


        switch (particleType)
        {
            case ParticleTypes.rain:
                height = 50f;
                newSize = new Vector3(terrainSize.x, terrainSize.z, 1);
                newPosition = ObjectUtils.GetObjectCenter(terrain);
                newPosition.y = height;
                particlesRain.transform.position = newPosition;
                particlesRain.transform.localEulerAngles = new Vector3(90f, 0); // aiming torwards ground
                break;
            case ParticleTypes.cloud:
                height = 10;
                newSize = new Vector3(ObjectUtils.GetObjectSize(terrain).x, height, 1);
                newPosition = new Vector3(ObjectUtils.GetObjectCenter(terrain).x, terrain.transform.position.y + height, terrain.transform.position.z);
                break;
            case ParticleTypes.ground:
                height = terrain.transform.position.y;
                newSize = new Vector3(terrainSize.x, terrainSize.z, 1);
                newPosition = ObjectUtils.GetObjectCenter(terrain);
                newPosition.y = height;
                particlesGround.transform.position = newPosition;
                particlesGround.transform.localEulerAngles = new Vector3(-90f, 0); // aiming torwards Sky
                //need to get mesh from terrain and set it as shape ======================================================================================================== ToDo when map generation is done
                //var sh = particlesGround.shape;
                //sh.enabled = enabled;
                //sh.shapeType = ParticleSystemShapeType.Mesh;
                //sh.mesh = 
                break;
            default:
                break;
        }

        particleSystem.transform.position = newPosition;
        Utils.SetParticleSystemSize(particleSystem, newSize);
    }
    public void SetWeather(TimeUtils.Weather newWeather)
    {
        currentWeather = newWeather;
        UpdateWeather();
    }
    private void SetupWeather(WeatherSettings.Weather weather)
    {
        areCloudsOn = weather.areCloudsOn;
        UpdateClouds(areCloudsOn);
        if (weather.useParticlesRain)
        {
            particlesRain.Play();
            Utils.SetupParticleSystem(particlesRain, weather.particleProperitesRain, weatherSettings.particleMaterials);

            if (weather.applyWindToRain)
            {
                weather.particleProperitesRain.hasVelocity = weather.applyWindToRain;
                var velocityOverLifetime = particlesRain.velocityOverLifetime;
                velocityOverLifetime.enabled = weather.applyWindToRain;
                if (weather.applyWindToRain)
                {
                    velocityOverLifetime.x = wind.x;
                    velocityOverLifetime.y = wind.y;
                }
            }
        }
        else Utils.StopParticles(particlesRain);
        if (weather.useParticlesCloud)
        {
            particlesCloud.Play();
            Utils.SetupParticleSystem(particlesCloud, weather.particleProperitesCloud, weatherSettings.particleMaterials);
        }
        else Utils.StopParticles(particlesCloud);
        if (weather.useParticlesGround)
        {
            particlesGround.Play();
            Utils.SetupParticleSystem(particlesGround, weather.particleProperitesGround, weatherSettings.particleMaterials);
        }
        else Utils.StopParticles(particlesGround);
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
