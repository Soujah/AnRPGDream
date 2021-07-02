using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /*
    public Map referenceMap;
    public string mapIDName;

    public List<GameObject> interactableMapObjects = new List<GameObject>();

    public List<GameObject> groundTileList2D = new List<GameObject>(); 

    public void InteractWithMapPosition(Vector3 pos)
    {
        for (int i = 0; i < interactableMapObjects.Count; i++)
        {
            if (pos == interactableMapObjects[i].GetComponent<InteractableHandler>().mapInteractable.interactablePosition)
            {
                if (interactableMapObjects[i].GetComponent<InteractableHandler>().mapInteractable.interactableType == MapInteractable.InteractableType.TreasureChest)
                {
                    OpenTreasureChest(interactableMapObjects[i].GetComponent<InteractableHandler>().mapInteractable.thisGameObject);
                    break;
                }
                /*
                else if (item.GetComponent<MapInteractable>().interactableType == MapInteractable.InteractableType.MapChangePosition)
                {
                    LoadNewMap(item.GetComponent<MapInteractable>().thisGameObject);
                    return;
                }
            }
        }
    }


    

    public void OpenTreasureChest(GameObject treasureChest)
    {
        //open a treasurechest
        //GameObject newTreasureChest = treasureChest;

        DropSelector.instance.RandomReward(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel());
        interactableMapObjects.Remove(treasureChest);
        Destroy(treasureChest);

    }
    */

    
}
