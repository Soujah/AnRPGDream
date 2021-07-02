using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//ScriptableObject, ISerializationCallbackReceiver
[System.Serializable]
public class Item
{

    public int ID;
    public string name;
    public string itemType;
    public string itemQuality;
    public int itemAmount;
    public bool equippable;
    public bool consumable;
    public int armorValue;
    public int damageValue;
    public int healingValue;
    public int itemLevel;
    public bool isWeapon;
    public bool isArmor;
    public float minAccuracy;
    public float maxAccuracy;
    public int weaponTypeDamage;
    public bool levelRequirement;

    public Item(int id, string name, string itemType, string itemQuality, int itemAmount, bool equippable,
        bool consumable, int armorValue, int damageValue, int healingValue, int itemLevel, bool weapon, bool armor, float minAccuracy, float maxAccuracy, int weaponTypeDamage, bool levelRequirement)
    {
        this.ID = id;
        this.name = name;
        this.itemType = itemType;
        this.itemQuality = itemQuality;
        this.itemAmount = itemAmount;
        this.equippable = equippable;
        this.consumable = consumable;
        this.armorValue = armorValue;
        this.damageValue = damageValue;
        this.healingValue = healingValue;
        this.itemLevel = itemLevel;
        this.isWeapon = weapon;
        this.isArmor = armor;
        this.minAccuracy = minAccuracy;
        this.maxAccuracy = maxAccuracy;
        this.weaponTypeDamage = weaponTypeDamage;
        this.levelRequirement = levelRequirement;
    }


    #region Scriptable Object Code
    /*

    [SerializeField] private string itemType = "";
    private string runTimeType;
    [SerializeField] private string itemName = "";
    private string runTimeName;
    [SerializeField] private string itemQuality = "";
    private string runTimeQuality;
    [SerializeField] private int itemAmount = 0;
    private int runTimeItemAmount;
    [SerializeField] private bool equippable = false;
    private bool runTimeEquippable;
    [SerializeField] private bool consumable = false;
    private bool runTimeConsumable;
    [SerializeField] private int armorValue = 0;
    private int runTimeArmorValue;
    [SerializeField] private int damageValue = 0;
    private int runTimeDamageValue;
    [SerializeField] private int healingValue = 0;
    private int runTimeHealingValue;
    [SerializeField] private int itemLevel = 0;
    private int runTimeItemLevel;

    
    public void OnAfterDeserialize()
    {
        runTimeType = itemType;
        runTimeName = itemName;
        runTimeQuality = itemQuality;
        runTimeItemAmount = itemAmount;
        runTimeEquippable = equippable;
        runTimeConsumable = consumable;
        runTimeDamageValue = damageValue;
        runTimeArmorValue = armorValue;
        runTimeHealingValue = healingValue;
        runTimeItemLevel = itemLevel;
        
    }

    public void OnBeforeSerialize()
    {
        
    }

    public string GetItemType()
    {
        return runTimeType;
    }

    public void SetItemType(string newType)
    {
        runTimeType = newType;
    }

    public string GetItemName()
    {
        return runTimeName;
    }

    public void SetItemName(string newName)
    {
        runTimeName = newName;
    }

    public string GetItemQuality()
    {
        return runTimeQuality;
    }

    public void SetItemQuality(string newQuality)
    {
        runTimeQuality = newQuality;
    }

    public int GetItemAmount()
    {
        return runTimeItemAmount;
    }

    public void SetItemAmount(int newAmount)
    {
        runTimeItemAmount = newAmount;
    }

    public void AppendItemAmount(int amount)
    {
        runTimeItemAmount += amount;
    }

    public bool GetIsEquippable()
    {
        return runTimeEquippable;
    }

    public void SetEquippable(bool isEquippable)
    {
        runTimeEquippable = isEquippable;
    }

    public bool GetIsConsumable()
    {
        return runTimeConsumable;
    }

    public void SetConsumable(bool isConsumable)
    {
       runTimeConsumable = isConsumable;
    }

    public int GetArmorValue()
    {
        return runTimeArmorValue;
    }

    public void SetArmorValue(int newArmorValue)
    {
        runTimeArmorValue = newArmorValue;
    }

    public int GetDamageValue()
    {
        return runTimeDamageValue;
    }

    public void SetDamageValue(int newDamageValue)
    {
        runTimeDamageValue = newDamageValue;
    }

    public int GetHealingValue()
    {
        return runTimeHealingValue;
    }

    public void SetHealingValue(int newHealingValue)
    {
        runTimeHealingValue = newHealingValue;
    }

    public int GetItemLevel()
    {
        return runTimeItemLevel;
    }

    public void SetItemLevel(int newItemLevel)
    {
        runTimeItemLevel = newItemLevel;
    }
    */
    #endregion
}
