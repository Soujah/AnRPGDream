using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Game Manager Found");
        }
        instance = this;
    }
    #endregion

    public GameObject player;
    public int maxPlayerLevel;
    [SerializeField] private List<Vector3> occupiedPositions = new List<Vector3>();

    public List<Map> worldMaps = new List<Map>();
    public Map currentWorld;
    public GameObject currentWorldObject2D;
    public GameObject currentWorldObject3D;
    public GameObject atlasWorldObject3D;

    public bool hologramMode = false;

    //game cameras
    public GameObject Camera3D;
    public GameObject Camera2D;
    
    //world id tracker
    public int worldCountID;

    public bool mapGenerationFinished = false;
    
    //world building
    public GameObject theWorld;
    public GameObject worldMapPrefab;
    public GameObject worldMapPrefab3D;
    public GameObject hologramContainer;
    public GameObject atlasContainer;
    private GameObject container;
    public GameObject mapInteractablePrefab;
    //enemy settings
    public GameObject bossParticles;
    public GameObject enemyPrefab;
    //interactable assets
    public GameObject treasureChestPrefab;
    //public GameObject blockGameObject;

    //starting world
    public GameObject starterWorld2D;
    public GameObject starterWorld3D;
    public bool startingWorld;

    //trader
    public GameObject trader;
    public GameObject dungeonMaster;
    public GameObject gateKeeper;

    //enemy spawning


    //fog of war
    public FogOfWarController fogOfWar;

    //player animations
    public Animator playerAnimator;
    public AnimationClip playerDissolveAnimation;
    public AnimationClip playerIdle;
    public bool playerTeleporting = false;

    //map transition animations
    public Animator worldAnimator;
    public AnimationClip worldTransitionAnimation;
    public AnimationClip worldTransitionIdle;

    //player respawn point
    public Vector3 playerRespawnPoint;

    //treasurechest animations
    public GameObject treasureChestAnim;

    //requirement messages for npcs
    public GameObject DungeonMasterRequirement;
    public GameObject GateKeeperRequirement;

    //tile textures
    public Texture2D[] tileTextures;
    public List<Vector3> GetOccupiedPositions()
    {
        return occupiedPositions;
    }

    public void AppendOccipiedPositions(bool add, Vector3 positon)
    {
        if (add)
        {
            occupiedPositions.Add(positon);
        } else if (!add)
        {
            if (occupiedPositions.Contains(positon))
            {
                occupiedPositions.Remove(positon);
            }
        }
    }

    public void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }*/

        if (currentWorld != null)
        {
            RespawnEnemy();
        }
    }

    public GameObject GetCurrentWorld()
    {
        return currentWorldObject2D;
    }

    public void Start()
    {
        LoadStartWorld();
    }

    public void LoadStartWorld()
    {
        startingWorld = true;
        //fogOfWar2D.ResetFogOfWar();
        //fogOfWar3D.ResetFogOfWar();
        //loads the first world the player explores at the start of the game
        /*
        Map startingWorld = MapGenerator.instance.GenerateWorld(Map.WorldType.Forest);
        currentWorld = startingWorld;
        StartCoroutine(LoadNewWorld(startingWorld));*/
    }


    public void /*IEnumerator*/ LoadNewWorld(Map world, int layer = 12, bool dungeon = false)
    {
        //Debug.Log("world level " + world.mapLevel);
        //set hologram to 3d view of world
        //create the 2d view of the world
        mapGenerationFinished = false;

        if (currentWorldObject3D != null && currentWorldObject2D != null && layer == 12)
        {
            

            Destroy(currentWorldObject3D);
            Destroy(currentWorldObject2D);
            //currentWorld = null;
            
        } else if (layer == 13)
        {
            if (atlasWorldObject3D != null)
            {
                Destroy(atlasWorldObject3D);
            }
        }
        //disable starter world
        if (layer == 12)
        {
            startingWorld = false;
            starterWorld2D.SetActive(false);
            starterWorld3D.SetActive(false);
        }
        

        if (layer == 12)
        {
            container = hologramContainer;
            
        } else if (layer == 13)
        {
            container = atlasContainer;
            //Debug.Log("loading atlas world");
        }

        //create 3d world
        GameObject new3DMap = Instantiate(worldMapPrefab3D, container.transform.position, Quaternion.identity);
        new3DMap.transform.SetParent(container.transform);
        new3DMap.gameObject.name = world.mapName;
        if (layer == 12)
        {
            currentWorldObject3D = new3DMap;
        } else
        {
            atlasWorldObject3D = new3DMap;
        }
        //new3DMap.GetComponent<MapManager3D>().referenceMap = world;

        GameObject new2DMap = null;

        if (layer == 12)
        {
            //create 2d world
            new2DMap = Instantiate(worldMapPrefab, theWorld.transform.position, Quaternion.identity);
            new2DMap.transform.SetParent(theWorld.transform);
            new2DMap.gameObject.name = world.mapName;
            currentWorldObject2D = new2DMap;
            //new2DMap.GetComponent<MapManager>().referenceMap = world;
        }

        

        //spawn ground blocks
        for (int i = 0; i < world.groundBlocks.Count; i++)
        {
            
            //yield return new WaitForSeconds(0);

            GameObject newBlock = Instantiate(world.groundBlocks[i], world.groundBlocksPos[i], Quaternion.identity);
            newBlock.GetComponent<MeshRenderer>().material = world.groundBlocksMaterial[i];
            newBlock.transform.SetParent(new3DMap.transform);
            newBlock.layer = layer;
            newBlock.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", tileTextures[Random.Range(0, tileTextures.Length)]);
            //new3DMap.GetComponent<MapManager3D>().groundTileList.Add(newBlock);

            if (layer == 12)
            {
                //spawn ground blocks for 2d map
                GameObject newBlock2 = Instantiate(world.groundBlocks[i], new Vector3(world.groundBlocksPos[i].x, 1, world.groundBlocksPos[i].z), Quaternion.identity);
                newBlock2.GetComponent<MeshRenderer>().material = world.groundBlocksMaterial[i];
                newBlock2.transform.SetParent(new2DMap.transform);
                newBlock2.GetComponent<MeshRenderer>().material.SetTexture("_Texture2D", tileTextures[Random.Range(0, tileTextures.Length)]);
            }


        }
        //spawn assets
        for (int i = 0; i < world.assets.Count; i++)
        {
            GameObject newAsset = Instantiate(world.assets[i], world.assetsPos[i], Quaternion.identity);
            newAsset.transform.SetParent(new3DMap.transform);
            newAsset.gameObject.layer = layer;
            //new3DMap.GetComponent<MapManager3D>().mapAssets.Add(newAsset);
        }

        //spawn particle effect
        if (world.particle != null)
        {
            GameObject newParticle = Instantiate(world.particle, new3DMap.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));

            newParticle.transform.SetParent(new3DMap.transform);
            newParticle.layer = layer;

        }


        //spawn interactables
        if (layer == 12)
        {
            for (int i = 0; i < world.mapInteractables.Count; i++)
            {
                GameObject newInteractable = Instantiate(mapInteractablePrefab, world.mapInteractables[i].interactablePosition, Quaternion.Euler(new Vector3(90, 0, 0)));
                newInteractable.GetComponent<InteractableHandler>().mapInteractable = world.mapInteractables[i];
                newInteractable.transform.SetParent(new2DMap.transform);

                //spawn 3d interactable asset

                Vector3 pos = new Vector3();
                foreach (Vector3 item in world.groundBlocksPos)
                {
                    float itemPosX = item.x;
                    float itemPosY = item.y;
                    float itemPosZ = item.z;

                    Vector2 itemPos = new Vector2(itemPosX, itemPosZ);
                    if (itemPos == new Vector2(newInteractable.GetComponent<InteractableHandler>().mapInteractable.interactablePosition.x,
                        newInteractable.GetComponent<InteractableHandler>().mapInteractable.interactablePosition.z))
                    {
                        pos = new Vector3(itemPosX, itemPosY + 0.75f, itemPosZ);
                        break;
                    }
                }

                GameObject worldAsset3D = null;
                if (newInteractable.GetComponent<InteractableHandler>().mapInteractable.interactableType == MapInteractable.InteractableType.TreasureChest)
                {
                    worldAsset3D = treasureChestPrefab;
                }

                GameObject newAsset = Instantiate(worldAsset3D, pos, Quaternion.identity);
                newInteractable.GetComponent<InteractableHandler>().mapInteractable.interactableGameObject = newAsset;
                newAsset.transform.SetParent(new3DMap.transform);

                //new2DMap.GetComponent<MapManager>().interactableMapObjects.Add(newInteractable);
            }

            //spawn enemies
            world.lv5ActiveEnemies = 0;
            world.lv10ActiveEnemies = 0;
            world.bossActiveEnemies = 0;
            bool bossSpawned = false;

            /*
            if (dungeon) 
            {
                List<Enemy> enemyList = player.GetComponent<PlayerManager>().GetPlayerDefeatedEnemies();
            } else
            {
                
            }*/

            //spawn lv 5 enemies

            for (int i = 0; i < world.lv5EnemyLimit; i++)
            {

                //SpawnEnemies(i);
                Vector3 enemyPos = new Vector3((int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1),
                        6,
                        (int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1));


                GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(90, 0, 0))); ;

                int enemyIndex = Random.Range(0, world.lv5MapEnemies.Count);
                //Debug.Log("index: " + enemyIndex);
                newEnemy.GetComponent<EnemyController>().SetEnemy(world.lv5MapEnemies[enemyIndex]);
                if (bossSpawned == false)
                {

                    for (int ii = 0; ii < world.lv5MapEnemies.Count; ii++)
                    {
                        if (world.lv5MapEnemies[ii].IsBoss())
                        {
                            //Debug.Log("spawned boss");
                            newEnemy.GetComponent<EnemyController>().SetEnemy(world.lv5MapEnemies[ii]);
                            //Debug.Log("SPAWNING BOSS: boss defeated = " + world.bossDefeated + " boss spawned var = " + bossSpawned);
                            break;
                        }
                    }
                }

                if (newEnemy.GetComponent<EnemyController>().boss && world.bossDefeated || bossSpawned && newEnemy.GetComponent<EnemyController>().boss)
                {
                    Destroy(newEnemy);
                    continue;

                }
                else
                {
                    world.lv5ActiveEnemies++;

                }

                if (newEnemy.GetComponent<EnemyController>().boss)
                {
                    bossSpawned = true;
                    //set particles
                    GameObject newBossParticles = Instantiate(bossParticles, newEnemy.transform.position, Quaternion.identity);
                    newBossParticles.transform.SetParent(newEnemy.transform);
                    newBossParticles.transform.localScale = new Vector3(1, 1, 1);
                    newBossParticles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }

                newEnemy.transform.SetParent(currentWorldObject2D.transform);

                //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().boss ? 10 : Random.Range(1, 3);
                //newEnemy.GetComponent<EnemyController>().enemy.ScaleByLevel(enemyLevelMultiplier + world.mapLevel);
                newEnemy.GetComponent<EnemyController>().enemy.SetEnemyLevel(newEnemy.GetComponent<EnemyController>().enemy.GetEnemyLevel());

                newEnemy.GetComponent<EnemyController>().SetEnemyInfo();




            }

            //spawn lv 10 enemies

            for (int i = 0; i < world.lv10EnemyLimit; i++)
            {

                //SpawnEnemies(i);
                Vector3 enemyPos = new Vector3((int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1),
                        6,
                        (int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1));


                GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(90, 0, 0))); ;

                int enemyIndex = Random.Range(0, world.lv10MapEnemies.Count);
                //Debug.Log("index: " + enemyIndex);
                newEnemy.GetComponent<EnemyController>().SetEnemy(world.lv10MapEnemies[enemyIndex]);
                if (bossSpawned == false)
                {

                    for (int ii = 0; ii < world.lv10MapEnemies.Count; ii++)
                    {
                        if (world.lv10MapEnemies[ii].IsBoss())
                        {
                            //Debug.Log("spawned boss");
                            newEnemy.GetComponent<EnemyController>().SetEnemy(world.lv10MapEnemies[ii]);
                            //Debug.Log("SPAWNING BOSS: boss defeated = " + world.bossDefeated + " boss spawned var = " + bossSpawned);
                            break;
                        }
                    }
                }

                if (newEnemy.GetComponent<EnemyController>().boss && world.bossDefeated || bossSpawned && newEnemy.GetComponent<EnemyController>().boss)
                {
                    Destroy(newEnemy);
                    continue;

                }
                else
                {
                    world.lv10ActiveEnemies++;

                }

                if (newEnemy.GetComponent<EnemyController>().boss)
                {
                    bossSpawned = true;
                    //set particles
                    GameObject newBossParticles = Instantiate(bossParticles, newEnemy.transform.position, Quaternion.identity);
                    newBossParticles.transform.SetParent(newEnemy.transform);
                    newBossParticles.transform.localScale = new Vector3(1, 1, 1);
                    newBossParticles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }

                newEnemy.transform.SetParent(currentWorldObject2D.transform);

                //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().boss ? 10 : Random.Range(1, 3);
                //newEnemy.GetComponent<EnemyController>().enemy.ScaleByLevel(enemyLevelMultiplier + world.mapLevel);
                newEnemy.GetComponent<EnemyController>().enemy.SetEnemyLevel(newEnemy.GetComponent<EnemyController>().enemy.GetEnemyLevel());

                newEnemy.GetComponent<EnemyController>().SetEnemyInfo();




            }

            //spawn boss (lv 15 enemies)

            for (int i = 0; i < world.enemyBossLimit; i++)
            {

                //SpawnEnemies(i);
                Vector3 enemyPos = new Vector3((int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1),
                        6,
                        (int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1));


                GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(90, 0, 0))); ;

                int enemyIndex = Random.Range(0, world.mapBosses.Count);
                //Debug.Log("index: " + enemyIndex);
                newEnemy.GetComponent<EnemyController>().SetEnemy(world.mapBosses[enemyIndex]);
                if (bossSpawned == false)
                {

                    for (int ii = 0; ii < world.mapBosses.Count; ii++)
                    {
                        if (world.mapBosses[ii].IsBoss())
                        {
                            //Debug.Log("spawned boss");
                            newEnemy.GetComponent<EnemyController>().SetEnemy(world.mapBosses[ii]);
                            //Debug.Log("SPAWNING BOSS: boss defeated = " + world.bossDefeated + " boss spawned var = " + bossSpawned);
                            break;
                        }
                    }
                }

                if (newEnemy.GetComponent<EnemyController>().boss && world.bossDefeated || bossSpawned && newEnemy.GetComponent<EnemyController>().boss)
                {
                    Destroy(newEnemy);
                    continue;

                }
                else
                {
                    world.bossActiveEnemies++;

                }

                if (newEnemy.GetComponent<EnemyController>().boss)
                {
                    bossSpawned = true;
                    //set particles
                    GameObject newBossParticles = Instantiate(bossParticles, newEnemy.transform.position, Quaternion.identity);
                    newBossParticles.transform.SetParent(newEnemy.transform);
                    newBossParticles.transform.localScale = new Vector3(1, 1, 1);
                    newBossParticles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }

                newEnemy.transform.SetParent(currentWorldObject2D.transform);

                //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().boss ? 10 : Random.Range(1, 3);
                //newEnemy.GetComponent<EnemyController>().enemy.ScaleByLevel(enemyLevelMultiplier + world.mapLevel);
                newEnemy.GetComponent<EnemyController>().enemy.SetEnemyLevel(newEnemy.GetComponent<EnemyController>().enemy.GetEnemyLevel());

                newEnemy.GetComponent<EnemyController>().SetEnemyInfo();
            }

            if (world.bossDefeated)
            {
                bossSpawned = true;
                //Debug.Log("World's Boss Defeated");
                //Debug.Log(world.maxEnemyCount);
            }

            //List<Enemy> enemyList = dungeon ? player.GetComponent<PlayerManager>().GetPlayerDefeatedEnemies() : world.enemies;


            /*
            for (int i = 0; i < world.maxEnemyCount; i++)
            {
                
                //SpawnEnemies(i);
                Vector3 enemyPos = new Vector3((int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1),
                        6,
                        (int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1));


                GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(90, 0, 0))); ;

                int enemyIndex = Random.Range(0, enemyList.Count);
                //Debug.Log("index: " + enemyIndex);
                newEnemy.GetComponent<EnemyController>().SetEnemy(enemyList[enemyIndex]);
                if (bossSpawned == false)
                {

                    for (int ii = 0; ii < enemyList.Count; ii++)
                    {
                        if (enemyList[ii].IsBoss())
                        {
                            //Debug.Log("spawned boss");
                            newEnemy.GetComponent<EnemyController>().SetEnemy(enemyList[ii]);
                            //Debug.Log("SPAWNING BOSS: boss defeated = " + world.bossDefeated + " boss spawned var = " + bossSpawned);
                            break;
                        }
                    }
                }
                
                if (newEnemy.GetComponent<EnemyController>().boss && world.bossDefeated || bossSpawned && newEnemy.GetComponent<EnemyController>().boss)
                {
                    Destroy(newEnemy);
                    continue;

                } else
                {
                    world.activeEnemies++;
                    
                }

                if (newEnemy.GetComponent<EnemyController>().boss)
                {
                    bossSpawned = true;
                    //set particles
                    GameObject newBossParticles = Instantiate(bossParticles, newEnemy.transform.position, Quaternion.identity);
                    newBossParticles.transform.SetParent(newEnemy.transform);
                    newBossParticles.transform.localScale = new Vector3(1, 1, 1);
                    newBossParticles.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }

                newEnemy.transform.SetParent(currentWorldObject2D.transform);

                //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().boss ? 10 : Random.Range(1, 3);
                //newEnemy.GetComponent<EnemyController>().enemy.ScaleByLevel(enemyLevelMultiplier + world.mapLevel);
                newEnemy.GetComponent<EnemyController>().enemy.SetEnemyLevel(newEnemy.GetComponent<EnemyController>().enemy.GetEnemyLevel());

                newEnemy.GetComponent<EnemyController>().SetEnemyInfo();

                


            }*/

            currentWorld = world;
        }
        //Debug.Log("generated:" + world.mapName + " layer: " + layer);
        
        if (world.fogRevealed == false && startingWorld == false && layer == 12)
        {
            fogOfWar.EnableFog();
        } else if (layer == 12)
        {
            fogOfWar.DisableFog();
        }
        mapGenerationFinished = true;

        
    }
    
    public void GenerateNewDungeon()
    {
        Debug.Log("generating dungeon");
        if (mapGenerationFinished && player.GetComponent<PlayerManager>().GetWorldShards() > 0)
        {
            player.GetComponent<PlayerManager>().AppendWorldShards(-1);
            LoadNewWorld(MapGenerator.instance.GenerateWorld(Map.WorldType.Dungeon), 12, true);
            
        }

    }

    public void CreateNewWorld()
    {
        if (mapGenerationFinished)
        {
            //player.GetComponent<PlayerManager>().AppendWorldShards(-1);
            var worldTypeCount = System.Enum.GetValues(typeof(Map.WorldType)).Length;

            

            Map.WorldType worldType = (Map.WorldType)Random.Range(2, worldTypeCount);
            //StartCoroutine(LoadNewWorld(MapGenerator.instance.GenerateWorld(worldType)));
            LoadNewWorld(MapGenerator.instance.GenerateWorld(worldType));
            Debug.Log("finished world generation");


        }
    }

    public void LoadExistingWorldFromID(int id)
    {

        if (mapGenerationFinished)
        {
            string worldIDName = "World" + id;
            //Debug.Log("Searching for " + worldIDName);
            Map worldToLoad;
            foreach (Map item in worldMaps)
            {
                //Debug.Log("found: " +item.mapName);
                if (item.mapName == worldIDName)
                {
                    worldToLoad = item;
                    //StartCoroutine(LoadNewWorld(worldToLoad));
                    LoadNewWorld(worldToLoad);
                    
                    break;
                }
            }
        }
    }

    public void LoadExistingWorldFromMap(Map map, int layer)
    {
        //StartCoroutine(LoadNewWorld(map, layer));
        //LoadNewWorld(map, layer);

        if (playerTeleporting == false && layer == 12)
        {
            StartCoroutine(LoadExistingWorldFromMapExecute(map, layer));
        } else
        {
            LoadNewWorld(map, layer);
        }


    }

    IEnumerator LoadExistingWorldFromMapExecute(Map map, int layer)
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        worldAnimator.SetTrigger("WorldTransition");
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        LoadNewWorld(map, layer);
        playerAnimator.SetTrigger("PlayerAppear");
        playerTeleporting = false;
    }

    //interact with current world

    public void InteractWithWorldObject(Vector3 pos)
    {
        //Debug.Log("interacting");
        if (NPCDistanceCheck(trader.transform.position, pos) && startingWorld) //pos == trader.transform.position
        {
            //interact with trader
            Debug.Log("Interacting with trader");
            if (playerTeleporting == false)
            {
                StartCoroutine(InteractTrader());
            }
            return;
        }
        if (NPCDistanceCheck(dungeonMaster.transform.position, pos) && startingWorld) //pos == dungeonMaster.transform.position
        {
            Debug.Log("interacting with dungeon master");
            if (playerTeleporting == false && player.GetComponent<PlayerManager>().GetWorldShards() > 0)
            {
                StartCoroutine(InteractDungeonMaster());
            } else if (player.GetComponent<PlayerManager>().GetWorldShards() <= 0)
            {
                DungeonMasterRequirement.SetActive(true);
            }
        } 
        if (NPCDistanceCheck(gateKeeper.transform.position, pos) && startingWorld) //pos == gateKeeper.transform.position
        {
            //load dragonlair world type
            if (playerTeleporting == false && player.GetComponent<PlayerManager>().GetPlayerLevel() >= maxPlayerLevel)
            {
                Map newMap = MapGenerator.instance.GenerateWorld(Map.WorldType.DragonLair);
                StartCoroutine(InteractGatekeeper(newMap));
            } else if (player.GetComponent<PlayerManager>().GetPlayerLevel() < maxPlayerLevel)
            {
                GateKeeperRequirement.SetActive(true);
            }
        } 
        if (currentWorld == null)
        {
            return;
        }
        if (currentWorld.mapInteractables != null)
        {
            for (int i = 0; i < currentWorld.mapInteractables.Count; i++)
            {
                if (pos == currentWorld.mapInteractables[i].interactablePosition)
                {
                    //TREASURE CHEST DETECTION
                    if (currentWorld.mapInteractables[i].interactableType == MapInteractable.InteractableType.TreasureChest)
                    {
                        OpenTreasureChest(currentWorld.mapInteractables[i]);
                    }
                }
            }
        }
        
    }

    IEnumerator InteractDungeonMaster()
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        worldAnimator.SetTrigger("WorldTransition");
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        GenerateNewDungeon();
        playerAnimator.SetTrigger("PlayerAppear");
        playerTeleporting = false;

    }

    IEnumerator InteractTrader()
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        worldAnimator.SetTrigger("WorldTransition");
        player.GetComponent<PlayerController>().SetCombatStatus(true);
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        CreateNewWorld();
        playerAnimator.SetTrigger("PlayerAppear");
        playerTeleporting = false;
        yield return new WaitForSeconds(3f);
        player.GetComponent<PlayerController>().SetCombatStatus(false);

    }

    IEnumerator InteractGatekeeper(Map mapToLoad)
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        worldAnimator.SetTrigger("WorldTransition");
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        //LoadNewWorld(MapGenerator.instance.GenerateWorld(Map.WorldType.DragonLair), 12, false);
        LoadNewWorld(mapToLoad, 12, false);
        playerAnimator.SetTrigger("PlayerAppear");
        playerTeleporting = false;

    }

    //INTERACTABLE OBJECT - TREASURE CHEST
    public void OpenTreasureChest(MapInteractable treasureChest)
    {
        //open a treasurechest
        DropSelector.instance.RandomReward(GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel());
        currentWorld.mapInteractables.Remove(treasureChest);

        GameObject treasureChestObject = treasureChest.interactableGameObject;
        treasureChest.interactableGameObject = null;

        //treasurechest animation
        treasureChestAnim.GetComponent<Animator>().SetTrigger("OpenChest");


        Destroy(treasureChestObject);
        Destroy(treasureChest.thisGameObject);

    }

    public void ReturnHome()
    {
        if (starterWorld2D.activeSelf == false && starterWorld3D.activeSelf == false && mapGenerationFinished)
        {
            if (playerTeleporting == false)
            {
                StartCoroutine(ReturnHomeExecute());
            }

        }
    }

    IEnumerator ReturnHomeExecute()
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        worldAnimator.SetTrigger("WorldTransition");
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        Destroy(currentWorldObject3D);
        Destroy(currentWorldObject2D);
        currentWorld = null;

        startingWorld = true;

        currentWorldObject2D = starterWorld2D;
        starterWorld2D.SetActive(true);
        starterWorld3D.SetActive(true);

        fogOfWar.DisableFog();

        mapGenerationFinished = true;
        //make player appear
        playerAnimator.SetTrigger("PlayerAppear");
        player.GetComponent<PlayerController>().playerMovePoint.position = playerRespawnPoint;
        player.GetComponent<PlayerController>().currentPosition = playerRespawnPoint;
        player.transform.position = playerRespawnPoint;
        playerTeleporting = false;
    }

    public void RespawnEnemy()
    {
        //Debug.Log(currentWorld.activeEnemies + "respawning enemies " + currentWorld.maxEnemyCount);

        //respawn enemies on the current map if the current enemy count is lower than cap
        /*
        if (currentWorld.activeEnemies < currentWorld.maxEnemyCount && mapGenerationFinished)
        {
            //spawn enemy //TODO DONT RESPAWN BOSSES
            SpawnEnemies(1);
        }*/

        //respawn lv 5 enemies

        if (currentWorld.lv5ActiveEnemies < currentWorld.lv5EnemyLimit && mapGenerationFinished)
        {
            //Debug.Log("spawning LV5 enemy");
            SpawnEnemies(1, currentWorld.lv5MapEnemies, true);
        }
        //respawn lv 10 enemies
        else if (currentWorld.lv10ActiveEnemies < currentWorld.lv10EnemyLimit && mapGenerationFinished)
        {
            //Debug.Log("spawning LV10 enemy");
            SpawnEnemies(1, currentWorld.lv10MapEnemies, false);
        }


    }

    public void SpawnEnemies(int amount, List<Enemy> spawnableEnemyList, bool lv5)
    {

        for (int i = 0; i < amount; i++)
        {
            Vector3 enemyPos = new Vector3((int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeX - 1),
                    6,
                    (int)Random.Range(0, GameManager.instance.GetComponent<MapGenerator>().worldSizeZ - 1));

            GameObject newEnemy = Instantiate(enemyPrefab, enemyPos, Quaternion.Euler(new Vector3(90, 0, 0)));

            List<Enemy> enemyList = /*currentWorld.dungeon ? player.GetComponent<PlayerManager>().GetPlayerDefeatedEnemies() :*/ spawnableEnemyList;
            int enemyIndex = Random.Range(0, enemyList.Count);

            newEnemy.GetComponent<EnemyController>().SetEnemy(enemyList[enemyIndex]);
            //Debug.Log("Respawning Enemy : " + newEnemy.GetComponent<EnemyController>().GetEnemy().GetEnemyName() + " Index: " + enemyIndex  + " Dungeon? " + currentWorld.dungeon);
            //Debug.Log("respawning index: " + enemyIndex + " name: " +  newEnemy.GetComponent<EnemyController>().GetEnemy().GetEnemyName());
            if (newEnemy.GetComponent<EnemyController>().boss)
            {
                Destroy(newEnemy);
                continue;
            }
            newEnemy.transform.SetParent(currentWorldObject2D.transform);

            //use enemy's preset level and stats

            //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().boss ? 10 : Random.Range(1, 5);
            //newEnemy.GetComponent<EnemyController>().enemy.ScaleByLevel(enemyLevelMultiplier + currentWorld.mapLevel);

            newEnemy.GetComponent<EnemyController>().enemy.SetEnemyLevel(newEnemy.GetComponent<EnemyController>().enemy.GetEnemyLevel());

            if (lv5)
            {
                currentWorld.lv5ActiveEnemies++;
            } else
            {
                currentWorld.lv10ActiveEnemies++;
            }

            //currentWorld.activeEnemies++;
            newEnemy.GetComponent<EnemyController>().SetEnemyInfo();
        }


    }

    public void GenerateWorldOfType(int type)
    {
        if (!mapGenerationFinished)
        {
            return;
        }
        Map.WorldType mapType = Map.WorldType.Forest;
        switch ((Map.WorldType)type) 
        {
            case (Map.WorldType.Forest):
                mapType = Map.WorldType.Forest;
                break;
            
        }
        Map typeWorld = MapGenerator.instance.GenerateWorld(mapType);
        //StartCoroutine(LoadNewWorld(typeWorld));
        LoadNewWorld(typeWorld);
    }

    public void Death()
    {
        player.GetComponent<PlayerManager>().AppendWorldShards(-Random.Range(0, player.GetComponent<PlayerManager>().GetWorldShards()));
        //StartCoroutine(DeathSequence());
        if (playerTeleporting == false)
        {
            StartCoroutine(ReturnHomeExecute());
            

        }
    }
    /*
    IEnumerator DeathSequence()
    {
        playerTeleporting = true;
        playerAnimator.SetTrigger("PlayerDissolve");
        yield return new WaitForSeconds(playerDissolveAnimation.length);
        StartCoroutine(ReturnHomeExecute());
        playerAnimator.SetTrigger("PlayerAppear");
        playerTeleporting = false;

    }*/


    public void CaptureWorld()
    {
        var worldTypeCount = System.Enum.GetValues(typeof(Map.WorldType)).Length;
        Map.WorldType worldType = (Map.WorldType)Random.Range(0, worldTypeCount);
        player.GetComponent<PlayerManager>().AppendPlayerMaps(true, GameManager.instance.GetComponent<MapGenerator>().GenerateWorld(worldType));
    }

    /*
    public bool PlayerTeleporting()
    {
        AnimatorClipInfo[] clipInfo = playerAnimator.GetCurrentAnimatorClipInfo(0);
        string clipName = clipInfo[0].clip.name;
        bool playerTP = clipName == playerIdle.name ? false : true;
        Debug.Log("player teleporting: " + playerTP);
        return playerTP;
    }*/

    public bool NPCDistanceCheck(Vector3 npcPos, Vector3 playerPos)
    {

        if (Vector3.Distance(npcPos, playerPos) <= 1)
        {
            return true;
        }

        return false;
    }
}
