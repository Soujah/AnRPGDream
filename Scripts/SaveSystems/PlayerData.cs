using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
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
}
