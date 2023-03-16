using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockHnadler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            Debug.Log("Tick " + e.tick);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
