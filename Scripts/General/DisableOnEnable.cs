using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnEnable : MonoBehaviour
{
    public float timeTillDisable;

    public void OnEnable()
    {
        StartCoroutine(DisableObject());

    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(timeTillDisable);
        gameObject.SetActive(false);
    }
}
