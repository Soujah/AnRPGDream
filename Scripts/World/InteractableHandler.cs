using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHandler : MonoBehaviour
{
    public MapInteractable mapInteractable;

    void Start()
    {

        mapInteractable.SetInteractable(mapInteractable);
        mapInteractable.thisGameObject = gameObject;
       
        gameObject.GetComponent<SpriteRenderer>().sprite = mapInteractable.interactableSprite;
    }

    
}
