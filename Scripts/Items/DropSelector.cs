using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSelector : MonoBehaviour
{

    #region Singleton

    public static DropSelector instance;
    public GameObject inventoryManager;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Drop Selector Found");
        }
        instance = this;
    }
    #endregion

    public AudioSource rewardSoundSource;

    public void RandomReward(int level, bool betterLoot = false, Enemy enemy = null)
    {
        int randomNumber = Random.Range(1, 100);

        if (betterLoot)
        {
            randomNumber += 20;
        }
        int dropAmount = Random.Range(0,3);
        int itemsDropped = 0;
        //Debug.Log(randomNumber);
        //Debug.Log(randomNumber);
        if (randomNumber > 30)//default 20
        {
            if (enemy != null)
            {
                if (enemy.enemyUniqueItems.Length != 0 && randomNumber > 80)
                {
                    Item randomUnqiueItem = enemy.enemyUniqueItems[Random.Range(0, enemy.enemyUniqueItems.Length)];
                    GameManager.instance.player.GetComponent<PlayerManager>().AddStoredItem(randomUnqiueItem);
                    ItemGenerator.instance.itemLog.GetComponent<ItemLog>().NewItemObtained(randomUnqiueItem);
                } else
                {
                    while (itemsDropped <= dropAmount)
                    {
                        ItemGenerator.instance.NewRandomItem(level);
                        GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerWallet(MoneyReward(level));
                        GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerExperience(ExperienceReward(level, betterLoot));
                        itemsDropped++;
                    }
                    
                }
            } else
            {
                while (itemsDropped <= dropAmount)
                {
                    ItemGenerator.instance.NewRandomItem(level);
                    GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerWallet(MoneyReward(level));
                    GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerExperience(ExperienceReward(level, betterLoot));
                    itemsDropped++;
                }

                    
            }

            inventoryManager.GetComponent<InventoryTabSwitch>().EnableStoredItems();


        }
        else
        {
            GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerWallet(MoneyReward(level));
            GameManager.instance.player.GetComponent<PlayerManager>().AppendPlayerExperience(ExperienceReward(level, betterLoot));
        }

        rewardSoundSource.Play();

    }

    public int ExperienceReward(int level, bool betterLoot = false)
    {
        float experienceAmount = Random.Range(1,25) + 50 + (level * 40) + (Mathf.Pow(level, 3.6f)) * (0.5f + (level * 0.005f));
        experienceAmount = experienceAmount * 0.35f;

        if (betterLoot)
        {
            experienceAmount *= 1.2f;
        }

        Debug.Log("Exp: " + experienceAmount);
        //Debug.Log(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerExperience() + "/" + 50 + (level * 40) + ((Mathf.Pow(level, 3.6f)) * (0.5 + (level * 0.005))));
        return (int)experienceAmount;
    }

    public int MoneyReward(int level)
    {
        int moneyAmount = level * Random.Range(1, 25);
        return moneyAmount;
    }

    public void BossDrop(int level, bool dungeon, Enemy enemy)
    {
        if (!dungeon)
        {
            GameManager.instance.player.GetComponent<PlayerManager>().AppendWorldShards(Random.Range(1, 4)); 
            RandomReward(level, false, enemy);
        } else
        {
            RandomReward(level, true, enemy);
        }
    }
}
