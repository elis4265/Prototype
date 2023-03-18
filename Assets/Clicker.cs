using UnityEngine;

public class Clicker : MonoBehaviour
{
    public Terrain terrain;
    public float brushSize = 10f;
    public float brushStrength = 0.1f;
	
	private void Start()
	{
		terrain = GetComponent<Terrain>();
	}

    private void Update()
    {
        // Check for left mouse button down
        if (!Input.GetMouseButton(0)) return;
		
		// Get the terrain data
		TerrainData terrainData = terrain.terrainData;

		// Calculate the position of the mouse in terrain coordinates
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo))
		{
			Vector3 terrainPos = hitInfo.point;
			Vector3 normalizedPos = terrainPos - terrain.transform.position;
			normalizedPos.x /= terrainData.size.x;
			normalizedPos.y /= terrainData.size.y;
			normalizedPos.z /= terrainData.size.z;

			// Calculate the heightmap coordinates
			int heightmapX = (int)(normalizedPos.x * terrainData.heightmapResolution);
			int heightmapY = (int)(normalizedPos.z * terrainData.heightmapResolution);

			// Modify the heightmap
			float[,] heightmap = terrainData.GetHeights(heightmapX - (int)(brushSize / 2), heightmapY - (int)(brushSize / 2), (int)brushSize, (int)brushSize);
			for (int i = 0; i < brushSize; i++)
			{
				for (int j = 0; j < brushSize; j++)
				{
					float distance = Mathf.Sqrt(Mathf.Pow(i - brushSize / 2f, 2) + Mathf.Pow(j - brushSize / 2f, 2));
					if (distance < brushSize / 2f)
					{
						heightmap[i, j] += brushStrength * Time.deltaTime;
						heightmap[i, j] = Mathf.Clamp01(heightmap[i, j]);
					}
				}
			}

			// Apply the modified heightmap
			terrainData.SetHeights(heightmapX - (int)(brushSize / 2), heightmapY - (int)(brushSize / 2), heightmap);
        }
    }
}