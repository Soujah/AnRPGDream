using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
    //Player Stat Variables
    public int playerHealth;
    public int playerMaxHealth;
    public List<Item> playerInventory = new List<Item>();
    public List<Item> playerEquippedItems = new List<Item>();
    public int playerWallet;
    public int playerLevel;
    public int playerDamage = 0;
    public int playerArmor = 0;
    public float playerExperience = 0;
    public bool equippedWeapon;
    public int worldShards;
    public Item equippedWeaponItem;
    public List<Map> ownedMaps = new List<Map>();
    public List<Enemy> defeatedEnemies = new List<Enemy>();
    public List<Enemy> defaultEnemyUnlocks = new List<Enemy>();
    public int basePlayerDamage;

    private void Start()
    {
        foreach (Enemy item in defaultEnemyUnlocks)
        {
            AppendDefeatedEnemies(true, item);
        }
        playerDamage = basePlayerDamage;
    }

    public float GetPlayerExperience()
    {
        return playerExperience;
    }

    public void AppendPlayerExperience(int amount)
    {
        playerExperience += amount;
        if (playerExperience >= 50 + (playerLevel * 40) + ((Mathf.Pow(playerLevel, 3.6f)) * (0.5 + (playerLevel * 0.005))) && playerLevel < GameManager.instance.maxPlayerLevel)
        {
            //float leftOverXP = playerExperience - (playerLevel * 150) ;
            playerExperience = 0/*leftOverXP*/;
            playerMaxHealth += 2;
            //basePlayerDamage += 1;
            if (!equippedWeapon)
            {
                playerDamage = basePlayerDamage;
            }
            playerHealth = playerMaxHealth;
            playerLevel++;
        }
    }

    //Player Health
    public int GetPlayerHealth()
    {
        return playerHealth;
    }

    public void AppendPlayerHealth(int amount)
    {
        playerHealth += amount;
    }

    public int GetPlayerMaxHealth()
    {
        return playerMaxHealth;
    }

    public void AppendPlayerMaxHealth(int amount)
    {
        playerMaxHealth += amount;
    }

    //Player Inventory
    public List<Item> GetPlayerInventory()
    {
        return playerInventory;
    }

    public void AddStoredItem(Item item)
    {
        bool addItem = false;

        foreach (Item i in playerInventory)
        {
            if (i.name == item.name && i.consumable)
            {
                //Debug.Log("already contains: " + item.name);
                i.itemAmount += item.itemAmount;
                addItem = false;
                break;
            } else
            {
                //Debug.Log("adding unique: " + item.name);
                addItem = true;
            }
        }

        if (playerInventory.Count == 0 || addItem)
        {
            playerInventory.Add(item);

        }

        InventoryTabSwitch.instance.UpdateInventory();

    }

    public void RemoveStoredItem(Item item)
    {
        playerInventory.Remove(item);
    }

    public void EquipItem(Item item)
    {
        if (item.isWeapon)
        {
            equippedWeaponItem = item;
            playerDamage = basePlayerDamage + item.damageValue;
            equippedWeapon = true;
        }
        else if (item.isArmor)
        {
            playerArmor += item.armorValue;
        }

        playerEquippedItems.Add(item);

    }

    public void DeEquipItem(Item item)
    {
        //check item is equipped
        
        playerArmor -= item.armorValue;
        playerEquippedItems.Remove(item);
        playerInventory.Add(item);
        if (item.isWeapon == true)
        {
            playerDamage = basePlayerDamage;
            equippedWeaponItem = null;
            equippedWeapon = false;
        }
        
    }

    public List<Item> GetPlayersEquippedItems()
    {
        return playerEquippedItems;
    }

    public void ConsumeItem(int healthAmount)
    {
        //consume item
        AppendPlayerHealth(healthAmount);
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
    }

    public int GetPlayerLevel()
    {
        return playerLevel;
    }

    public void SetPlayerLevel(int level)
    {
        playerLevel += level;
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    public int GetPlayerArmor()
    {
        return playerArmor;
    }

    public int GetPlayerWallet()
    {
        return playerWallet;
    }

    public void AppendPlayerWallet(int amount)
    {
        playerWallet += amount;
    }

    public bool hasWeapon()
    {
        return equippedWeapon;
    }

    public int GetWorldShards()
    {
        return worldShards;
    }

    public void AppendWorldShards(int amount)
    {
        worldShards += amount;
    }

    public void Death()
    {
        //lose exp when you die and set player's health to max health
        playerHealth = playerMaxHealth;

        float lostXPAmount = 0.75f * playerExperience;
        playerExperience -= lostXPAmount;

    }

    public Item GetWeapon()
    {
        return equippedWeapon ? equippedWeaponItem : null;      
    }

    public List<Map> GetPlayerMaps()
    {
        return ownedMaps;
    }

    public void AppendPlayerMaps(bool add, Map map)
    {
        if (add)
        {
            ownedMaps.Add(map);
            //Debug.Log("added to inv: " + map.mapName);
        } else
        {
            ownedMaps.Remove(map);
        }
    }

    public void DebugMapInv()
    {
        foreach (var item in ownedMaps)
        {
            Debug.Log("Map: " + item.mapName);
        }
    }

    public List<Enemy> GetPlayerDefeatedEnemies()
    {
        return defeatedEnemies;
    }

    public void AppendDefeatedEnemies(bool add, Enemy enemy)
    {
        if (add)
        {
            defeatedEnemies.Add(enemy);
        } else
        {
            defeatedEnemies.Remove(enemy);
        }
    }
}
