using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapInteractable
{
    public string interactableName;
    public Vector3 interactablePosition;
    public InteractableType interactableType;
    public Sprite interactableSprite;
    public GameObject thisGameObject;
    public GameObject interactableGameObject;

    public enum InteractableType
    {
        TreasureChest   
    }

    public MapInteractable(string name, Vector3 position, InteractableType type)
    {
        this.interactableName = name;
        this.interactablePosition = position;
        this.interactableType = type;
        //this.interactableGameObject = obj;
        if (Resources.Load<Sprite>("Sprites/" + type) != null)
        {
            this.interactableSprite = Resources.Load<Sprite>("Sprites/" + type);
            //gameObject.GetComponent<SpriteRenderer>().sprite = interactableSprite;
        }
        
    }

    public void SetInteractable(MapInteractable interactable)
    {
        this.interactableName = interactable.interactableName;
        this.interactablePosition = interactable.interactablePosition;
        this.interactableType = interactable.interactableType;
        if (Resources.Load<Sprite>("Sprites/" + interactable.interactableType) != null)
        {
            this.interactableSprite = Resources.Load<Sprite>("Sprites/" + interactable.interactableType);
            //gameObject.GetComponent<SpriteRenderer>().sprite = interactableSprite;
        }
        
    }
}
