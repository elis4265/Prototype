using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
	public Terrain terrain;
    
	[SerializeField]
	Transform cellPrefab;
	
	private bool[,] obstacleCells;
	

    // Update is called once per frame
    void Start()
    {
        obstacleCells = terrain.GetComponent<ObstaclesCells>().GetObstacleCells;
    }
	
	public void DrawCell(int x, int y)
	{
		Transform cell = Instantiate(cellPrefab);
		cell.localPosition = new Vector3(x + 0.75f, 0.0f, y + 0.75f);
	}
}
