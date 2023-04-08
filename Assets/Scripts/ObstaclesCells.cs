using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesCells : MonoBehaviour
{
	private Terrain terrain;
	private int mapWidth;
	private int mapHeight;
	private bool[,] obstacleCells;
	
	public bool[,] GetObstacleCells{get{return obstacleCells;}}

    void Start()
    {
        terrain = GetComponent<Terrain>();
		mapWidth = GetComponent<TerrainGeneration>().width;
		mapHeight = GetComponent<TerrainGeneration>().height;
		
		InstantiateCells();
		
		GameObject[] obstacles;
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		int obstaclePosX, obstaclePosY;
		foreach (GameObject obj in obstacles)
		{
			obstaclePosX = (int)(obj.transform.position.x);
			obstaclePosY = (int)(obj.transform.position.z);
			if(obstaclePosX < mapWidth || obstaclePosY < mapHeight || obstaclePosX > 0 || obstaclePosY > 0)
			{
				obstacleCells[obstaclePosY, obstaclePosX] = true;
			}
		}
		Debug.Log("obstacles scanned");
    }
	
    private void InstantiateCells()
	{
		obstacleCells = new bool[mapHeight, mapWidth];
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				obstacleCells[y,x] = false;
			}
		}
	}
}
