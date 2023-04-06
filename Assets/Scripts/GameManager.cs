using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public WeatherController weatherController;
    public ClockHnadler clockHnadler;
    public DayNightCycler dayNightCycler;

    public GameObject weatherDropdown;
    public GameObject terrain;
    // Start is called before the first frame update
    void Start()
    {
        dayNightCycler.ligthSourceDay.GetComponent<UniversalAdditionalLightData>().lightCookieSize = new Vector2(ObjectUtils.GetObjectSize(terrain).x, ObjectUtils.GetObjectSize(terrain).z);
        dayNightCycler.ligthSourceNight.GetComponent<UniversalAdditionalLightData>().lightCookieSize = new Vector2(ObjectUtils.GetObjectSize(terrain).x, ObjectUtils.GetObjectSize(terrain).z);

        Utils.SetupWeatherDropDown(weatherDropdown);
        weatherDropdown.GetComponent<TMP_Dropdown>().value = (int)weatherController.currentWeather;
        weatherDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(delegate { weatherController.SetWeather((TimeUtils.Weather)weatherDropdown.GetComponent<TMP_Dropdown>().value); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
