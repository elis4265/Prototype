using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassRegrow : MonoBehaviour
{
	private float regrowthTimer;
	void Start()
	{
		regrowthTimer = Random.Range(10f, 30f);
	}
	
    void Update()
	{
		//todo
		if (this.tag == "OnRespawnTimer")
		{
			regrowthTimer -= Time.deltaTime;
			if(regrowthTimer <= 0)
			{
				this.tag = "EdibleByHerbivores";
				regrowthTimer = Random.Range(10f, 30f);
				this.GetComponent<Renderer>().enabled = true;	 
			}
		}
	}
}
