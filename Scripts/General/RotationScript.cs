using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public float speed;
    void Update()
    {
        if (GameManager.instance.mapGenerationFinished)
        {
            transform.Rotate(speed * Vector3.up * Time.deltaTime);
        }
    }
}
