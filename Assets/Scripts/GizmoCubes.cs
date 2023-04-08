using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoCubes : MonoBehaviour
{
	private Vector3 gizmoPosition;
    private Vector3 gizmoSize = new Vector3(0.25f, 0.25f, 0.25f);
    private	Color gizmoColor = Color.yellow;
	
	private void Start()
	{
		gizmoPosition = this.transform.position;
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(gizmoPosition, gizmoSize);
    }
}
