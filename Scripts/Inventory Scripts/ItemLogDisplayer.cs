using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemLogDisplayer : MonoBehaviour
{
    public GameObject textObject;
    public float timeTillDestroy;

    private void Start()
    {
        Destroy(gameObject, timeTillDestroy);
    }

    public void SetText(string textToSet)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = textToSet;
    }

    
}
