using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquippedItemButton : MonoBehaviour
{
    public GameObject inventoryManager;
    //[SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Item referencedItem;
    public bool empty = true;
    public AudioSource AS;
    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryManager");
        AS = GameObject.Find("InventorySound").GetComponent<AudioSource>();
        //buttonText = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        //buttonText.text = text;
    }

    public void OnButtonClicked()
    {
        if (referencedItem != null && empty == false)
        {
            //deequip item from player and reload equipped items ui
            inventoryManager.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().DeEquipItem(referencedItem);
            gameObject.GetComponent<EquippedItemHoverText>().SetItem(null);
            gameObject.GetComponent<EquippedItemHoverText>().hoverTextBox.text = "";
            referencedItem = null;
            inventoryManager.GetComponent<InventoryTabSwitch>().UpdateInventory();
            empty = true;
            AS.Play();
        }


    }

    public Item GetReferencedItem()
    {
        return referencedItem;
    }

    public void SetReferencedItem(Item item)
    {
        referencedItem = item;
        empty = false;
    }
}
