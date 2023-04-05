using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                PlantController plantController;
                Debug.Log(hit.transform.name);
                if (!hit.transform.parent) return;
                if (hit.transform.parent.name == "Leaves") plantController = hit.transform.parent.parent.GetComponent<PlantController>();
                else plantController = hit.transform.parent.GetComponent<PlantController>();
                if (plantController != null) plantController.OnClick();
            }
        }
    }
}