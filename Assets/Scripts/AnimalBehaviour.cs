using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class animalBehaviour : MonoBehaviour
{
	private float HungerTimer;
	private GameObject targetedGrass;
	private NavMeshAgent _agent;
	private bool hungry;
	private bool isDestinationTargeted;
	private bool isWaiting;
	private float waitTimer;
	private System.Random random;

    void Start()
    {
		random = new System.Random();
		HungerTimer = Random.Range(7f, 15f);
		hungry = true;
		_agent = GetComponent<NavMeshAgent>();
		targetedGrass = SeekGrass(_agent);
		isDestinationTargeted = GrassTargeted(hungry, targetedGrass);
		waitTimer = Random.Range(1f, 6f);
		
		isWaiting = false;
    }

    void Update()
    {
		random = new System.Random();
		if (targetedGrass == null || !hungry)
		{
			if(!isWaiting)
			{
				if(random.Next(100) < 75)
				{
					isDestinationTargeted = RandomRoam(_agent);
					isWaiting = !isWaiting;
				}
				else
				{
					isDestinationTargeted = true;
					isWaiting = !isWaiting;
				}
			}
			else
			{
				waitTimer -= Time.deltaTime;
				if(waitTimer <= 0)
				{
					isWaiting = !isWaiting;
					waitTimer = (float)(random.NextDouble() * (6 - 1) + 1);
				}
			}
			HungerTimer -= Time.deltaTime;
			if (HungerTimer <= 0)
			{
				if (!(Vector3.Distance(_agent.destination, this.transform.position) >= 3.0f))
				{
					isDestinationTargeted = RandomRoam(_agent);
				}
				hungry = !hungry;
				targetedGrass = SeekGrass(_agent);
				HungerTimer = Random.Range(7f, 15f);
			}
		}
		else	
		{
			if(GrassTargeted(hungry, targetedGrass)) //if o == null 
			{
				targetedGrass = SeekGrass(_agent);
			}
			else
			{
				isDestinationTargeted = GrassTargeted(hungry, targetedGrass);
				hungry = isDestinationTargeted;
				return;
			}
		}
    }
	
	private bool GrassTargeted(bool hungry ,GameObject o)
	{
		if (o == null)
			return false;
		else
		{
			if (Vector3.Distance(o.transform.position, this.transform.position) <= 0.5)
				return EatGrass(o);
		}
		return true;
	}
	
	private bool RandomRoam(NavMeshAgent a)
	{
        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
		Vector3 destination = this.transform.position + position;
		a.SetDestination(destination);
		return true;
	}
	
	public GameObject SeekGrass(NavMeshAgent a)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Grass");
		
        GameObject closest = null;
        float distance = Mathf.Infinity; 
        Vector3 position = transform.position;
		float curDistance;
		Vector3 diff;
        foreach (GameObject go in gos)
        {
            diff = go.transform.position - position;
            curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
		if(closest != null)
			a.SetDestination(closest.transform.position);
		return closest;
    }
	
	private bool EatGrass(GameObject o)
	{
		o.tag = "EatenGrass";
		o.GetComponent<Renderer>().enabled = false;	 
		return false;
	}
}