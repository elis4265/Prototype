using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
	[SerializeField]
	public GameObject target;
	
	[SerializeField]
	public GameObject visualizer;
	
	public Terrain terrain;
	
	private int startX, startY, targetX, targetY, mapWidth, mapHeight;
	
	private int x, y;
	
	private float timer = 0.0f;
	
	private bool[,] obstacleCells;
	
	private bool pathfindingProceeding = false;
	private bool yep = false;
	private bool pathFound = false;
	
	private List<Tuple<int, int>> path;
	private Tuple<int, int> lastObstacle;
	private int listPoint = 1;
	
	private Vector3 nextPoint;
	
	private bool walking = false;
	private bool done = false;
	
    void Start()
    {
		startX = (int)(this.transform.position.x);
		startY = (int)(this.transform.position.z);
		
        target = GameObject.FindGameObjectWithTag("Finish");
		targetX = (int)(target.transform.position.x);
		targetY = (int)(target.transform.position.z);
		mapWidth = terrain.GetComponent<TerrainGeneration>().width;
		mapHeight = terrain.GetComponent<TerrainGeneration>().height;
		
		path = new List<Tuple<int, int>>();
		
		x = startX;
		y = startY;
    }

    void Update()
    {
		if (done)
		{
			return;
		}
		
		if (obstacleCells == null)
		{
			obstacleCells = terrain.GetComponent<ObstaclesCells>().GetObstacleCells;
		}
		
		if(pathfindingProceeding)
		{
			GoingThroughtCells();
		}
		
		if (pathFound)
		{
			TargetDestinationNextPoint();
		}
		
		if(!yep)
		{
			pathfindingProceeding = SearchDiretlyToThetarget();
		}
    }
	
	private void GoingThroughtCells()
	{
		if(timer > 0)
			{
				timer -= Time.deltaTime;
				return;
			}
			
			TopRightSearch();
			
			if(timer <= 0)
			{
				timer = 0.15f;
				return;
			}
			timer -= Time.deltaTime;
	}
	
	private bool IsThisCellObstacle()
	{
		if (obstacleCells[y,x] == false)
		{
			visualizer.GetComponent<Visualizer>().DrawCell(x, y);
			return false;
		}
		else
		{
			if(x > 0 && y < mapHeight)
				lastObstacle = new Tuple<int, int>(x, y+1);
			return true;
		}
	}
	
	private bool IsThisCellTarget()
	{
		if(x == targetX && y == targetY)
		{
			Debug.Log("found target");
			pathfindingProceeding = false;
			Debug.Log("pathfinding no longer proceeding");
			yep = true;
			path.Add(new Tuple<int, int>(x, y));
			pathFound = true;
			return true;
		}
		return false;
	}
	
	private bool SearchDiretlyToThetarget()	//this is anal-geometry
	{
		float lengthX = targetX - startX;
		float lengthY = targetY - startY;
		
		if(lengthX>lengthY)
		{
			float increment = lengthY / lengthX;
			for (int i = startX; i < targetX; i++)
			{
				if(obstacleCells[(int)(startY + (i-startX)*increment),i])
				{
					Debug.Log("pathfinding needed");
					return true;
				}
			}
		}
		else if (lengthY > lengthX)
		{
			float increment = lengthX / lengthY;
			for (int i = startY; i < targetY; i++)
			{
				if(obstacleCells[i,(int)(i*increment)])
				{
					Debug.Log("pathfinding needed");
					return true;
				}
			}
		}
		else
		{
			for (int i = startY; i < targetY; i++)
			{
				if(obstacleCells[i,i])
				{
					Debug.Log("pathfinding needed");
					return true;
				}
			}
		}
		
		Debug.Log("pathfinding not needed");
		pathFound = true;
		yep = true;
		path.Add(new Tuple<int, int>((int)(target.transform.position.x + 1f), (int)(target.transform.position.z)));
		return false;
	}
	
	private void TopRightSearch()
	{
		if(!IsThisCellObstacle())
		{
			if(lastObstacle != null)
			{
				if(lastObstacle.Item1 == (x))
				{
					path.Add(lastObstacle);
					Debug.Log("last obstacle added");
					startX = lastObstacle.Item1;
					startY = lastObstacle.Item2;
					pathfindingProceeding = SearchDiretlyToThetarget();
				}
			}
			IsThisCellTarget();
		}
		else
		{
			y++;
			x = startX-1;
		}
		x++;
		if(x == mapWidth)
		{
			y++;
			x = startX;
		}
	}
	
	private void List2V3()
	{
		nextPoint = new Vector3(path[listPoint-1].Item1 - 0.5f, 0, path[listPoint-1].Item2 + 0.5f);
	}
	
	private void TargetDestinationNextPoint()
	{
		if (walking)
		{
			if(Vector3.Distance(this.transform.position, nextPoint) > 0.65f)
			{
				Debug.Log("walking");
				return;
			}
			else
				walking = false;
		}
		else if (listPoint <= path.Count)
		{
			Debug.Log(path.Count);
			List2V3();
			GetComponent<NavTerrain>().SetDestination(nextPoint);
			nextPoint.y = 0.5f;
			listPoint++;
			Debug.Log("list point " + listPoint);
			Debug.Log("path found " + pathFound);
			walking = true;
		}
		else
		{
			done = true;
			Debug.Log("done");
		}
	}
	
	/*private bool Walking()
	{
		
	}*/
}
