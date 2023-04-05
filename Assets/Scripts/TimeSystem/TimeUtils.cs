using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class TimeUtils
{
    // previously in TimeTickSystem
    // how often should tick occur (0.2 == 200ms) max is FPS
    public const float TICK_TIMER_MAX = 0.01f;
    public static float tickSpeedMultiplier = 1;

    // previously in clock handler
    public static int DAYS_IN_MONTH = 30;
    public static int MONTHS_IN_YEAR = 8;
    public static int SEASONS = 4;
    // previouslt in DayNightCycler
    public const int MINUTES_IN_HOUR = 60;
    public const int HOURS_IN_DAY = 24;
    public const int MORNING_START = 6;
    public const int EVENING_START = 20;
    public const int NIGHT_LENGHT = MORNING_START + (HOURS_IN_DAY - EVENING_START);
    public const int NIGHT_LENGHT_MINUTES = NIGHT_LENGHT * MINUTES_IN_HOUR;
    public const int DAY_LENGHT = HOURS_IN_DAY - NIGHT_LENGHT;
    public const int DAY_LENGHT_MINUTES = DAY_LENGHT * MINUTES_IN_HOUR;
    public enum DayState { Day, Night }
    public enum Weather { Clear, Cloudy, Rain, Storm}

    /// <summary>
    /// Calculates what day in season it is based on date
    /// </summary>
    /// <param name="date">Current date.</param>
    /// <returns>Day in season.</returns>
    public static int GetDayInSeason(int3 date)
    {
        int day = date.z;
        int month = date.y;
        int daysInMonth = DAYS_IN_MONTH;
        int monthsInYear = MONTHS_IN_YEAR;
        int seasons = SEASONS;

        int monthsInSeason = monthsInYear / seasons;
        int dayIncrement = daysInMonth * ((month + 1) % monthsInSeason);
        return day + dayIncrement;
    }

    public static int GetDaysInSeason()
    {
        return (MONTHS_IN_YEAR / SEASONS) * DAYS_IN_MONTH;
    }

    public static int GetMonthsPerSeason()
    {
        return MONTHS_IN_YEAR / SEASONS;
    }

    /// <summary>
    /// Returns lenght of day and length of an hour.
    /// </summary>
    /// <returns>Return int2 (hours,minutes) time.</returns>
    public static int2 GetDayLenght() { return new int2(HOURS_IN_DAY, MINUTES_IN_HOUR); }
    public static void SetTickSpeed(float newSpeed)
    {
        tickSpeedMultiplier = newSpeed;
    }
}
