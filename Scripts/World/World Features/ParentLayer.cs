using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentLayer : MonoBehaviour
{
    void Start()
    {
        if (transform.parent != null)
        {
            gameObject.layer = transform.parent.gameObject.layer;
        }
    }

    
    
}
