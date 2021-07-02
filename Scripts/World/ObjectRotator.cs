using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ObjectRotator : MonoBehaviour
{
    public GameObject bigMap;
    public float rotationSpeed = 0.2f;

    void OnMouseDrag()
    {
        Debug.Log("mouse drag");
        
    }

    public void Update()
    {
        if (Input.GetMouseButton(0) && bigMap.activeSelf)
        {
            
            float XaxisRotation = -1 * Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            
            transform.Rotate(Vector3.up, -XaxisRotation);
            

            
        }
    }

}