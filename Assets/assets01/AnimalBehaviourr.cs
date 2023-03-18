using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBehaviourr : MonoBehaviour
{
	private float HungerTimer;
	private GameObject targetedObject;
	private NavTerrain agent;
	private bool hungry;
	private bool isDestinationTargeted;
	private bool isWaiting;
	private float waitTimer;

    void Start()
    {
		HungerTimer = Random.Range(7f, 15f);
		//hungry = Random.value < 0.5f;
		hungry = true;
		agent = GetComponent<NavTerrain>();
		SeekGrass();
		isDestinationTargeted = IsObjectTargeted();
		waitTimer = Random.Range(1f, 6f);
		
		isWaiting = false;
    }

    void Update()
    {
		if (targetedObject == null || !hungry)
		{
			if(!isWaiting)
			{
				if(Random.value < 0.5f)
				{
					RandomRoam();
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
					waitTimer = Random.Range(1f, 6f);
				}
			}
			HungerTimer -= Time.deltaTime;
			if (HungerTimer <= 0)
			{
				if (!(Vector3.Distance(agent.destination, this.transform.position) >= 3.0f))
				{
					RandomRoam();
				}
				hungry = !hungry;
				SeekGrass();
				HungerTimer = Random.Range(7f, 25f);
			}
		}
		else	
		{
			if(IsObjectTargeted())
			{
				SeekGrass();
			}
			else
			{
				isDestinationTargeted = IsObjectTargeted();
				hungry = isDestinationTargeted;
				return;
			}
		}
    }
	
	private bool IsObjectTargeted()
	{
		if (targetedObject == null)
			return false;
		else
		{
			if (Vector3.Distance(targetedObject.transform.position, this.transform.position) <= 2f)
				return EatObject();
		}
		return true;
	}
	
	private void RandomRoam()
	{
		Vector3 destination;
		while(true)
		{
			Vector3 newPosition = new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
			destination = this.transform.position + newPosition;
				if(destination.x >= 0.5f && destination.z >= 0.5f)
				{
					break;
				}
		}
		agent.SetDestination(destination);
		isDestinationTargeted = true;
	}
	
	private void SeekGrass()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("EdibleByHerbivores");
		
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
			agent.SetDestination(closest.transform.position);
		targetedObject = closest;
    }
	
	private bool EatObject()
	{
		targetedObject.tag = "OnRespawnTimer";
		targetedObject.GetComponent<Renderer>().enabled = false;	 
		return false;
	}
}