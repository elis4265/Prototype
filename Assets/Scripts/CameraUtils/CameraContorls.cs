using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContorls : MonoBehaviour
{
    Vector3 initialPosition;
    Quaternion initialRotation;
    // WSAD birdview controls
    // QE rotations on Z
    // RF rotations on X
    // ZX zoom
    // Very basic movement, there are many issues with it I just wanted to quickly check different angles.
    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) transform.position += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) transform.position += Vector3.back;
        if (Input.GetKey(KeyCode.A)) transform.position += Vector3.left;
        if (Input.GetKey(KeyCode.D)) transform.position += Vector3.right;

        if (Input.GetKey(KeyCode.Q))
        {
            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z - 1
            );
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            transform.eulerAngles.z + 1
            );  
        }

        if (Input.GetKey(KeyCode.R))
        {
            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x - 1,
            transform.eulerAngles.y,
            transform.eulerAngles.z
            );
        }
        if (Input.GetKey(KeyCode.F))
        {
            transform.eulerAngles = new Vector3(
            transform.eulerAngles.x + 1,
            transform.eulerAngles.y,
            transform.eulerAngles.z
            );
        }

        if (Input.GetKey(KeyCode.Z)) transform.position += Vector3.down;
        if (Input.GetKey(KeyCode.X)) transform.position += Vector3.up;

        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}
