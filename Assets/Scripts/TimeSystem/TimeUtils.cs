using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class TimeUtils
{
    /// <summary>
    /// Calculates what day in season it is based on date
    /// </summary>
    /// <param name="date">Current date.</param>
    /// <returns>Day in season.</returns>
    public static int GetDayInSeason(int3 date)
    {
        int day = date.z;
        int month = date.y;
        int daysInMonth = ClockHnadler.DAYS_IN_MONTH;
        int monthsInYear = ClockHnadler.MONTHS_IN_YEAR;
        int seasons = ClockHnadler.SEASONS;

        int monthsInSeason = monthsInYear / seasons;
        int dayIncrement = daysInMonth * ((month + 1) % monthsInSeason);
        return day + dayIncrement;
    }

    public static int GetDaysInSeason()
    {
        return (ClockHnadler.MONTHS_IN_YEAR / ClockHnadler.SEASONS) * ClockHnadler.DAYS_IN_MONTH;
    }
}
