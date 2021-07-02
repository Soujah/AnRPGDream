using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StoredItemButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    public Item referencedItem;
    public TextMeshProUGUI hoverTextBox;
    public AudioSource AS;

    private void Start()
    {
        buttonText = gameObject.transform.GetComponentInChildren<TextMeshProUGUI>();
        hoverTextBox = InventoryManager.instance.hoveredTextBox;
        AS = GameObject.Find("InventorySound").GetComponent<AudioSource>();
    }

    public void SetText(string text)
    {
        buttonText.text = text;
    }

    //HOVER STUFF
    public void OnPointerEnter(PointerEventData eventData)
    {
        //When player hover's over backpack button the equipped item of the same type if any is displayed in the text box
        //Debug.Log("hover");
        foreach (Item item in GameManager.instance.player.GetComponent<PlayerManager>().GetPlayersEquippedItems())
        {
            if (item.itemType == referencedItem.itemType || item.isWeapon && referencedItem.isWeapon)
            {
                hoverTextBox.text = LoadItemText(item);
                break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("hover exit");
        hoverTextBox.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("Left click");
        if (eventData.button == PointerEventData.InputButton.Left) {
            UseItem();
        }
        //Debug.Log("Middle click");
        else if (eventData.button == PointerEventData.InputButton.Middle) { }
        //Debug.Log("Right click");
        else if  (eventData.button == PointerEventData.InputButton.Right) { 
            //DropItem();
        }
    }

    public string LoadItemText(Item item)
    {
        string newString = "Lv. " + item.itemLevel + " " + item.itemQuality + "\n"
            + item.itemType + "\n"
            + item.name + "\n"
            + "<sprite=0> " + item.damageValue + " <sprite=2> " + item.armorValue + " <sprite=1> " + item.healingValue;

        return newString;
    }

    public void UseItem()
    {
        if (referencedItem.equippable)
        {
            //Debug.Log(referencedItem.name);
            //Debug.Log(referencedItem.equippable);
            //if item is equipable - equip item and reload stored items ui check item type isnt equipped already
            //Debug.Log("Equippable");
            foreach (Item item in InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().GetPlayersEquippedItems())
            {
                if (item.itemType == referencedItem.itemType)
                {
                    //Debug.Log("Item Type Already Equipped");
                    //Debug.Log("Unequipping: " + item.GetItemType().ToString());
                    //inventoryManager.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().DeEquipItem(item);
                    return;
                }
            }

            //check if the player has a weapon

            if (GameManager.instance.player.GetComponent<PlayerManager>().hasWeapon() && referencedItem.isWeapon)
            {
                return;
            }

            if (referencedItem.itemLevel <= InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().GetPlayerLevel())
            {
                //Debug.Log("Equipping item");
                InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().EquipItem(referencedItem);
                InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().RemoveStoredItem(referencedItem);
                InventoryManager.instance.GetComponent<InventoryTabSwitch>().UpdateInventory();
                //Debug.Log("Equipped");
                AS.Play();
            }

            InventoryManager.instance.GetComponent<InventoryTabSwitch>().EnableStoredItems();

        } else if (referencedItem.consumable)
        {
            //if item is consumable - consume item and reload stored items ui
            if (GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth() >= GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaxHealth())
            {
                return;
            }


            if (referencedItem.itemLevel <= InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().GetPlayerLevel())
            {
                InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().ConsumeItem(referencedItem.healingValue);
                referencedItem.itemAmount--;
                Debug.Log("Consumed item");
                AS.Play();
            }

            if (referencedItem.itemAmount <= 0)
            {
                InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().RemoveStoredItem(referencedItem);
            }
            InventoryManager.instance.GetComponent<InventoryTabSwitch>().UpdateInventory();
        }

    }

    public void DropItem()
    {
        InventoryManager.instance.GetComponent<InventoryManager>().player.GetComponent<PlayerManager>().RemoveStoredItem(referencedItem);
        InventoryManager.instance.GetComponent<InventoryTabSwitch>().EnableStoredItems();
    }

    public Item GetReferencedItem()
    {
        return referencedItem;
    }

    public void SetReferencedItem(Item item)
    {
        referencedItem = item;
    }
}
