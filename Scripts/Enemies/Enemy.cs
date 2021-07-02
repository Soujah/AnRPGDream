using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu]
public class Enemy : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private string enemyName = "";
    [SerializeField] private int enemyLevel = 0;
    private int runTimeEnemyLevel;
    [SerializeField] private int enemyDamage = 0;
    private int runTimeEnemyDamage;
    [SerializeField] private int enemyEvasion = 0;
    [SerializeField] private int enemyHealth = 0;
    private int runTimeEnemyHealth;
    [SerializeField] private float enemyMoveTime = 0;
    [SerializeField] private int enemyID = 0;
    [SerializeField] private Sprite sprite;
    [SerializeField] private bool boss;
    [SerializeField] private float moveSpeed = 0;
    [SerializeField] private float perfectAttackChance = 0;
    public Item[] enemyUniqueItems;

    public void OnAfterDeserialize()
    {
        //scale enemy stats with level
        runTimeEnemyLevel = enemyLevel;
        runTimeEnemyHealth = enemyHealth;
        runTimeEnemyDamage = enemyDamage;
    }

    public void OnBeforeSerialize()
    {

    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    public int GetEnemyLevel()
    {
        return runTimeEnemyLevel;
    }

    public void SetEnemyLevel(int newLevel)
    {
        runTimeEnemyLevel = newLevel;
    }

    public int GetEnemyDamage()
    {
        return runTimeEnemyDamage;
    }

    public int GetEnemyEvasion()
    {
        return enemyEvasion;
    }
    public float GetEnemyMoveTime()
    {
        return enemyMoveTime;
    }

    public int GetEnemyHealth()
    {
        return runTimeEnemyHealth;
    }

    public void AppendEnemyHealth(int amount)
    {
        runTimeEnemyHealth += amount;
    }

    public void EnemyDeath()
    {
        runTimeEnemyHealth = enemyHealth;
    }

    public void EnemyEscaped()
    {
        runTimeEnemyHealth = enemyHealth;
    }

    public int GetEnemyID()
    {
        return enemyID;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void SetBoss(bool bossStatus)
    {
        boss = bossStatus;
    }

    public bool IsBoss()
    {
        return boss;
    }

    public void ScaleByLevel(int newLevel)
    {
        runTimeEnemyDamage = Mathf.RoundToInt(enemyDamage + (newLevel * 0.75f));
        runTimeEnemyHealth = Mathf.RoundToInt(3 + enemyHealth + newLevel + (newLevel * 0.75f));
        SetEnemyLevel(newLevel);
    }

    public float GetEnemyMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetPerfectAttackChance()
    {
        return perfectAttackChance;
    }

}
