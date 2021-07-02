using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemGenerator : MonoBehaviour
{

    #region Singleton

    public static ItemGenerator instance;

    


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Item Generator Found");
        }
        instance = this;
    }
    #endregion

    public Text prefixHolder;
    public Text suffixHolder;
    public TextGenerator textGenerator;

    public WeaponTypesStruct[] weaponTypes;
    public ConsumableItemTypesStruct[] consumableTypes;
    public string[] armorTypes;
    //public string[] consumableItemTypes;
    public string[] itemQualities;

    public int itemIDTracker = 0;

    //public List<string> itemPrefixes = new List<string>();
    //public List<string> itemSuffixes = new List<string>();
    public string[] itemPrefixes;
    public string[] itemSuffixes;

    //item log
    public GameObject itemLog;

    //consumables


    private void Start()
    {
        itemPrefixes = prefixHolder.text.Split('\n');
        itemSuffixes = suffixHolder.text.Split('\n');
    }

    //create a new equippable item
    public void NewRandomItem(int baseLevel)
    {
        //Debug.Log(baseLevel);
        //Debug.Log("player " + GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel());

        int playerLevel = GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel();

        if (baseLevel > 3 + playerLevel)
        {
            baseLevel = playerLevel + Random.Range(0, 2);
        } else if (baseLevel < playerLevel - 2 && baseLevel > playerLevel - 11)
        {
            baseLevel = playerLevel - Random.Range(0, 1);
        }
        //Debug.Log("new " + baseLevel);
        int randomNumber = Random.Range(0, 3);

        //Item generatedItem = new Item(0,"","","",0,false,false,0,0,0,0);


        if (randomNumber == 0)
        {
            Item newItem = NewWeapon(baseLevel);
            GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(newItem);
            itemLog.GetComponent<ItemLog>().NewItemObtained(newItem);
            //GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(NewWeapon(baseLevel));
        }
        else if (randomNumber == 1)
        {
            Item newItem = NewArmor(baseLevel);
            GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(newItem);
            itemLog.GetComponent<ItemLog>().NewItemObtained(newItem);
            //GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(NewArmor(baseLevel));
        }
        else if (randomNumber == 2)
        {
            Item newItem = NewConsumable();
            GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(newItem);
            itemLog.GetComponent<ItemLog>().NewItemObtained(newItem);
            //GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(NewConsumable());
        }

        //InventoryTabSwitch.instance.UpdateInventory();
        //Debug.Log("Generated: " + generatedItem.GetItemName());
    }

    //generate a random name
    public string RandomItemNameGenerator()
    {
        string randomName = itemPrefixes[Random.Range(0,itemPrefixes.Length)] + " " + itemSuffixes[Random.Range(0, itemSuffixes.Length)];
        return randomName;
    }

    public Item NewWeapon(int level)
    {
        itemIDTracker++;
        float damage = (level * 1.25f);
        string quality = "";
        int qualityChoice = Random.Range(1,100);

        if (qualityChoice > 95)
        {
            quality = itemQualities[0];
            damage += 5;
        } else if (qualityChoice > 85)
        {
            quality = itemQualities[1];
            damage += 3;
        }
        else if (qualityChoice > 70)
        {
            quality = itemQualities[2];
            damage += 2;
        }
        else if (qualityChoice > 20)
        {
            quality = itemQualities[3];
            damage -= 1;
        }
        else if (qualityChoice >= 0)
        {
            quality = itemQualities[4];
            damage -= 2;
        }

        WeaponTypesStruct weaponType = weaponTypes[Random.Range(0, weaponTypes.Length)];
        if (damage <= 0)
        {
            damage = 1;
        }

        level += Random.Range(0, 3);
        if (level > GameManager.instance.maxPlayerLevel)
        {
            level = GameManager.instance.maxPlayerLevel;
        }
        Item newItem = new Item(itemIDTracker,RandomItemNameGenerator(),
            weaponType.weaponName,
            quality, 1, true, false, 0, (int)damage, 0,
            level, true, false, weaponType.weaponMinAccuracy, weaponType.weaponMaxAccuracy, weaponType.weaponTypeDamage, true);

        return newItem;

    }

    public Item NewArmor(int level)
    {
        itemIDTracker++;
        float armor = (level * 1.6f) + 2;
        string quality = "";
        int qualityChoice = Random.Range(1, 100);

        if (qualityChoice > 95)
        {
            quality = itemQualities[0];
            armor += 7;
        }
        else if (qualityChoice > 85)
        {
            quality = itemQualities[1];
            armor += 5;
        }
        else if (qualityChoice > 70)
        {
            quality = itemQualities[2];
            armor += 2;
        }
        else if (qualityChoice > 20)
        {
            quality = itemQualities[3];
            armor -= 1;
        }
        else if (qualityChoice >= 0)
        {
            quality = itemQualities[4];
            armor -= 2;
        }

        if (armor <= 0)
        {
            armor = 1;
        }

        level += Random.Range(0, 3);
        if (level > GameManager.instance.maxPlayerLevel)
        {
            level = GameManager.instance.maxPlayerLevel;
        }

        Item newItem = new Item(itemIDTracker, RandomItemNameGenerator(), armorTypes[Random.Range(0, armorTypes.Length)], quality, 1,
            true, false, (int)armor, 0, 0, level, false, true, 0, 0, 0, true);

        return newItem;
    }

    public Item NewConsumable()
    {
        ConsumableItemTypesStruct newDrop = consumableTypes[0];

        itemIDTracker++;

        int totalWeight = 0;
        int currentWeight = 0;

        for (int i = 0; i < consumableTypes.Length; i++)
        {
            totalWeight += consumableTypes[i].weight;
        }

        float targetWeight = Random.Range(0.0f, 1.0f) * totalWeight;

        for (int i = 0; i < consumableTypes.Length; i++)
        {
            currentWeight += consumableTypes[i].weight;
            if (currentWeight >= targetWeight)
            {
                newDrop = consumableTypes[i];
                break;
            }
        }

        int level = newDrop.levelRequirement ? GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel() : 0;

        Item newItem = new Item(itemIDTracker, newDrop.itemName, "Consumable", newDrop.dropQuality, Random.Range(1,newDrop.maxDropAmount), false, true,
            0, 0, newDrop.healingValue, level , false, false, 0, 0, 0, newDrop.levelRequirement);
        return newItem;
    }

    public void GiveConsumable(int level)
    {
        Item newConsumable = NewConsumable();
        GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(newConsumable);
        //Debug.Log("Adding to inv: " + newConsumable.name);
        //InventoryTabSwitch.instance.UpdateInventory();
    }

}
[System.Serializable]
public struct ConsumableItemTypesStruct
{
    public string itemName;
    public string dropQuality;
    public int maxDropAmount;
    public int healingValue;
    public int weight;
    public bool levelRequirement;

}

[System.Serializable]
public struct WeaponTypesStruct
{
    public string weaponName;
    [Range(0.0f, 1f)]
    public float weaponMinAccuracy;
    [Range(0.0f, 1f)]
    public float weaponMaxAccuracy;
    [Range(0, 10)]
    public int weaponTypeDamage;
}
