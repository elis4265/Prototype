using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NavTerrain : MonoBehaviour
{
	public Terrain terrain;
	private bool foo;
	public Vector3 destination;
	private float[,,] path;
	
	public float baseOffset = 0.5f;
	public float speed = 5.0f;
	
	private Vector3 step;
	private Vector3 movementDirection;
	private float movementAmount;
	
	private struct Nodes {
	   private float x;
	   private float y;
	   private float z;
	}; 
	
	void Update()
	{
		// if destination != this.Transform.position	
			// MoveTo(path)
		if ((this.transform.position - destination).sqrMagnitude > 0.3f)
		{
			MoveTo(destination);
		}
	}
	
	private void MoveTo(Vector3 goal)
	{
		movementAmount = speed * Time.deltaTime;	// how much it will move on XZ plane
        movementDirection = (goal - this.transform.position).normalized;	//the direction towards which will the unit move
		step = this.transform.position + movementDirection * movementAmount;	// the coordinate of 1xUpdate()
		step.y = terrain.SampleHeight(step) + baseOffset;	//the Y coordinate of one step
		
		this.transform.position = step;
	}
	
	public void SetDestination(Vector3 destinationInput) 
	{
		//destination = destinationInput; ---opravit
		//aStar()
		
		destination = destinationInput;
		//destination.y = baseOffset;
		
		/*
		1) make something move just on X and Z via nodes
		2) make it move on X Z and Y
		3) make it find path
		4) implement obstacles (steepness, Unity Obstacles)
		*/
		
		
	}
	
	
	/*private void aStar(Vector3  destinationInput)	//change type to float[,,]
	{
		//returns an array of coordNodes = path, via which the object will "walk" through 
		//returns float[,,] path
		
		
		Inspo:
			float[,] grid = MakeGrid(terrain.width * 4, terrain.height * 4);
			int y;
			heihgts[x, z];
			for(int x = 0; x < terrain.width * 4; x++)
			{
				for(int z = 0; z < terrain.height * 4; z++)
				{
					y = terrain.TerrainData.GetHeight(x, z);
					heihgts[x, z] = y;
				}
			}
			
		
	}*/
}