using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraContorls : MonoBehaviour
{
    public float speed = 10;

    Vector3 initialPosition;
    private Vector3 eularAngles;
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
        eularAngles = transform.eulerAngles;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.back * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.left * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.right * Time.deltaTime * speed, Space.World);

        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(-transform.forward * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(-transform.right * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(transform.right * Time.deltaTime * speed, Space.World);

        if (Input.GetKey(KeyCode.Q))
        {
            eularAngles.y--;
            transform.eulerAngles = eularAngles;
        }
        if (Input.GetKey(KeyCode.E))
        {
            eularAngles.y++;
            transform.eulerAngles = eularAngles;
        }

        if (Input.GetKey(KeyCode.R))
        {
            eularAngles.x--;
            transform.eulerAngles = eularAngles;
        }
        if (Input.GetKey(KeyCode.F))
        {
            eularAngles.x++;
            transform.eulerAngles = eularAngles;
        }

        if (Input.GetKey(KeyCode.Z)) transform.Translate(Vector3.down * Time.deltaTime * speed, Space.World);
        if (Input.GetKey(KeyCode.X)) transform.Translate(Vector3.up * Time.deltaTime * speed, Space.World);

        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }
}
