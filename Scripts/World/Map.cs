using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Map
{
    public string mapName;
    public List<GameObject> groundBlocks;
    public List<Vector3> groundBlocksPos;
    public List<Material> groundBlocksMaterial;
    public List<GameObject> assets;
    public List<Vector3> assetsPos;
    public List<MapInteractable> mapInteractables;
    public List<Enemy> lv5MapEnemies;
    public List<Enemy> lv10MapEnemies;
    public List<Enemy> mapBosses;
    public int lv5EnemyLimit;
    public int lv10EnemyLimit;
    public int enemyBossLimit;
    public int lv5ActiveEnemies;
    public int lv10ActiveEnemies;
    public int bossActiveEnemies;
    public int mapLevel;
    public WorldType type;
    public bool bossDefeated = false;
    public bool dungeon;
    public GameObject particle;
    public bool fogRevealed = false;   

    /// <LIST_OF_WORLD_TYPES>
    /// 
    /// Dungeon - Finished
    /// DragonLair
    /// Desert - Finished
    /// IcePlane
    /// RedDesert
    /// Forest - Finished
    /// CherryForest
    /// Planes
    /// SnowyForest
    /// Swamp - Finished
    /// 
    /// </LIST_OF_WORLD_TYPES>>
    
    public enum WorldType
    {
        Dungeon = 0,
        DragonLair = 1,
        Forest = 2,
        Swamp = 3,
        Desert = 4
            

        
    }


    public Map(string mapName, List<GameObject> groundBlocks, List<GameObject> assets, List<MapInteractable> mapInteractables, List<Enemy> mapBosses,
        List<Vector3> groundBlocksPos, List<Material> groundBlocksMaterial, List<Vector3> assetsPos, int level, WorldType type, bool dungeon,
        GameObject particle, bool fogRevealed, List<Enemy> lv5MapEnemies, List<Enemy> lv10MapEnemies, int lv5EnemyLimit, int lv10EnemyLimit,
        int enemyBossLimit)
    {
        this.mapName = mapName;
        this.groundBlocks = groundBlocks;
        this.assets = assets;
        this.mapInteractables = mapInteractables;
        this.mapBosses = mapBosses;
        this.lv5MapEnemies = lv5MapEnemies;
        this.lv10MapEnemies = lv10MapEnemies;
        this.groundBlocksPos = groundBlocksPos;
        this.groundBlocksMaterial = groundBlocksMaterial;
        this.assetsPos = assetsPos;
        this.mapLevel = level;
        this.lv5EnemyLimit = lv5EnemyLimit;
        this.lv10EnemyLimit = lv10EnemyLimit;
        this.enemyBossLimit = enemyBossLimit;
        this.type = type;
        this.dungeon = dungeon;
        this.particle = particle;
        this.fogRevealed = fogRevealed;
    }

}
