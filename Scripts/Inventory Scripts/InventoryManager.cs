using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    #region Singleton

    public static InventoryManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Inventory Manager Found");
        }
        instance = this;
    }
    #endregion

    public GameObject player;
    public GameObject storedItemButtonPrefab;
    public GameObject equippedItemButtonPrefab;

    public Transform equippedItemParent;
    public Transform storedItemParent;

    

    //Load player's inventories

    //equipped items buttons
    public Button helmetButton;
    public Button chestplateButton;
    public Button greevesButton;
    public Button bootsButton;
    public Button weaponButton;

    //equipped items icons
    public GameObject helmetImage;
    public GameObject chestplateImage;
    public GameObject bootsImage;
    public GameObject weaponImage;

    public TextMeshProUGUI hoveredTextBox;

    public void LoadPlayersEquippedItems()
    {
        /*
        foreach (Item item in player.GetComponent<PlayerManager>().GetPlayersEquippedItems())
        {
            //create button in equipped items tab
            GameObject newButton = Instantiate(equippedItemButtonPrefab) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<EquippedItemButton>().SetText(ButtonTextGenerator(item));
            newButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);

            newButton.transform.SetParent(equippedItemParent, false);
        }*/

        //SET EACH ITEM TYPE IN EQUIPPED ITEMS TO RELEVANT SLOTS
        
        foreach (Item item in GameManager.instance.player.GetComponent<PlayerManager>().GetPlayersEquippedItems())
        {
            if (item.isWeapon)
            {
                //set weapon button
                weaponButton.GetComponent<EquippedItemHoverText>().SetItem(item);
                weaponButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);
                weaponImage.SetActive(true);
                //Debug.Log("enabled weapon");
            }

            switch (item.itemType)
            {
                case "Helmet":
                    helmetButton.GetComponent<EquippedItemHoverText>().SetItem(item);
                    helmetButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);
                    helmetImage.SetActive(true);
                    break;
                case "Chestplate":
                    chestplateButton.GetComponent<EquippedItemHoverText>().SetItem(item);
                    chestplateButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);
                    chestplateImage.SetActive(true);
                    break;
                case "Greeves":
                    greevesButton.GetComponent<EquippedItemHoverText>().SetItem(item);
                    greevesButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);
                    break;
                case "Boots":
                    bootsButton.GetComponent<EquippedItemHoverText>().SetItem(item);
                    bootsButton.GetComponent<EquippedItemButton>().SetReferencedItem(item);
                    bootsImage.SetActive(true);
                    break;

            }
        }
    }

    public void LoadPlayersStoredItems()
    {
        foreach (Item item in player.GetComponent<PlayerManager>().GetPlayerInventory())
        {
            //create button in stored items tab
            GameObject newButton = Instantiate(storedItemButtonPrefab) as GameObject;
            newButton.SetActive(true);

            newButton.GetComponent<StoredItemButton>().SetText(ButtonTextGenerator(item));
            newButton.GetComponent<StoredItemButton>().SetReferencedItem(item);

            newButton.transform.SetParent(storedItemParent, false);
        }
    }

    public string ButtonTextGenerator(Item item)
    {
        string levelText = "Lv. " + item.itemLevel;
        if (!item.levelRequirement)
        {
            levelText = "";
        }

        string buttonText = levelText + " " + item.itemQuality + " " + item.itemType + "\n"
                + item.name + "" + ItemAmountText(item) + "\n"
                + "<sprite=0> " + item.damageValue + " <sprite=2> " + item.armorValue + " <sprite=1> " + item.healingValue;

        return buttonText;
    }

    public string ItemAmountText(Item item)
    {
        string amount = "";

        if (item.itemAmount == 0 || item.itemAmount == 1)
        {
            amount = "";
        } else
        {
            amount = " x" + item.itemAmount;
        }

        return amount;
    }

    
}
