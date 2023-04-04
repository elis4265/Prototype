using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ScrapedCode
{
    /// <summary>
    /// Calculate light angle based on time.
    /// </summary>
    /// <param name="time">Time for wanted light angle.</param>
    public void SetLightAngleBasedOnTime(int2 time)
    {
        //int timeHours = time.x;
        //int timeMinutes = time.y;

        //float angle = 0;

        //if (timeHours < MORNING_START)
        //{
        //    angle = ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_NIGHT;
        //}
        //else
        //{
        //    if (timeHours < EVENING_START)
        //    {
        //        timeHours -= MORNING_START;
        //        angle = (MORNING_START * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_NIGHT;
        //        angle += ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_DAY;
        //    }
        //    else
        //    {
        //        timeHours -= MORNING_START;
        //        timeHours -= DAY_LENGHT;
        //        angle = (MORNING_START * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_NIGHT;
        //        angle += (DAY_LENGHT * MINUTES_IN_HOUR) * LIGHT_ANGLE_STEP_DAY;
        //        angle += ((timeHours * MINUTES_IN_HOUR) + timeMinutes) * LIGHT_ANGLE_STEP_NIGHT;
        //    }
        //}
        //lightAngle.x = angle;
        //ligthSourceDay.transform.parent.eulerAngles = lightAngle;
    }
    /// <summary>
    /// Updates angle of light sources based on time or by incrementing.
    /// </summary>
    /// <param name="increment">How much angle of light changes per minute.</param>
    private void UpdateLightDirection(float increment)
    {
        //SetLightAngleBasedOnTime(time);
    }
}
