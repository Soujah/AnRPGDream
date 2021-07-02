using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTabSwitch : MonoBehaviour
{

    #region Singleton

    public static InventoryTabSwitch instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Inventory Tab Switch Found");
        }
        instance = this;
    }
    #endregion

    public InventoryManager inventoryManager;
    public GameObject equippedItemsTab;
    public GameObject storedItemsTab;
    //public GameObject equippedItemsTabButton;
    //public GameObject storedItemsTabButton;

    public GameObject equippedItemContentHolder;
    public GameObject storedItemContentHolder;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI worldShardText;
    public TextMeshProUGUI playerLevelText;


    private void Start()
    {
        //inventoryManager = gameObject.GetComponent<InventoryManager>();
        UpdateInventory();
    }

    public void EnableStoredItems()
    {
        //enable stored items ui
        storedItemsTab.SetActive(true);
        //disable equipped items ui and remove buttons
        //equippedItemsTab.SetActive(false);
        RemoveButtons();
        //load items from player's inventory into buttons
        inventoryManager.LoadPlayersStoredItems();

        //disable stored items tab button
        //storedItemsTabButton.GetComponent<Button>().enabled = false;
        //enable equipped items tab button
        //equippedItemsTabButton.GetComponent<Button>().enabled = true;
    }

    public void EnableEquippedItems()
    {
        //enable equipped items ui
        equippedItemsTab.SetActive(true);
        //disable stored items ui and remove buttons
        //storedItemsTab.SetActive(false);
        RemoveButtons();
        //load items from player's inventory into buttons
        inventoryManager.LoadPlayersEquippedItems();

        //disable equipped items tab button
        //equippedItemsTabButton.GetComponent<Button>().enabled = false;
        //enable stored items tab button
        //storedItemsTabButton.GetComponent<Button>().enabled = true;


    }

    public void UpdateInventory()
    {
        //Debug.Log("updating inv");
        EnableEquippedItems();
        EnableStoredItems();

        //update stats ui
        healthText.text = GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerHealth().ToString() + "/"
            + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerMaxHealth().ToString();
        damageText.text = GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerDamage().ToString();
        armorText.text = GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerArmor().ToString();
        worldShardText.text = GameManager.instance.player.GetComponent<PlayerManager>().GetWorldShards().ToString();
        playerLevelText.text = "Lv. " + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel().ToString();
    }

    public void RemoveButtons()
    {
        foreach (Transform child in storedItemContentHolder.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in equippedItemContentHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }

}
