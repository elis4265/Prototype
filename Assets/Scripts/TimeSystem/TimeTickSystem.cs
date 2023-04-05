using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
        public float tickInterval = TimeUtils.TICK_TIMER_MAX;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;

    private float tickTimer;
    private int tick;


    void Start()
    {
        tickTimer = 0;
        tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= TimeUtils.TICK_TIMER_MAX * TimeUtils.tickSpeedMultiplier)
        {
            tickTimer -= TimeUtils.TICK_TIMER_MAX * TimeUtils.tickSpeedMultiplier;
            tick++;
            if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });
        }
    }
}
