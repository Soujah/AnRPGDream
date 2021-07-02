using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEnabled : MonoBehaviour
{

    public GameObject inventoryManager;

    /*
    public void Awake()
    {
        if (gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
        }
    }*/

    void OnEnable()
    {
        //Debug.Log("enabled");
        //UpdatePlayerInv();
    }

    public void UpdatePlayerInv ()
    {
        inventoryManager.GetComponent<InventoryTabSwitch>().UpdateInventory();
    }
}
