using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float timeTillDestroy = 1f;

    private void Start()
    {
        if (timeTillDestroy != 0)
        {
            Destroy(gameObject, timeTillDestroy);
        }
    }
}
