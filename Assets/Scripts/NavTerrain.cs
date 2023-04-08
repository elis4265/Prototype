using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	
	void Start()
	{
		destination = this.transform.position;
	}
	
	void Update()
	{
		// if destination != this.Transform.position	
			// MoveTo(path)
		if (Vector3.Distance(this.transform.position, destination) > 0.01f)
		{
			MoveTo(destination);
			RotateTo(destination, 10f);
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
	
	private void RotateTo(Vector3 goal, float rotationSpeed)
	{
		// if looking at the same direction - return
		//if (this.transform.rotation == goal) return;
		goal.y = this.transform.position.y;
		Quaternion targetRotation = Quaternion.LookRotation(goal - this.transform.position);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
	}
	
	public void SetDestination(Vector3 destinationInput) 
	{
		destination = destinationInput;
	}
}