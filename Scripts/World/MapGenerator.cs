using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    #region Singleton

    public static MapGenerator instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one Map Generator Found");
        }
        instance = this;
    }
    #endregion


    public List<GameObject> testBlocks = new List<GameObject>();

    public GameObject blockGameObject;
    public GameObject mapInteractableObject;
    public GameObject treasureChestPrefab;

    public float worldSizeX = 10;
    public float worldSizeZ = 10;
    public float gridOffset = 1.1f;
    public float noiseHeight = 3;
    public float perlinDetail = 0;

    private float offsetX;
    private float offsetZ;

    public float assetHeight = 1.5f;

    //public List<Vector3> blockPositions = new List<Vector3>();
    private List<Vector3> objectPositions = new List<Vector3>();

    public List<GameObject> worldMaps = new List<GameObject>();

    public TerrainLayer[] worldLayer;
    public WorldPallet[] worldPallets;
    public string[] worldTypes;

    /*
    public GameObject theWorld;
    public GameObject worldMapPrefab;
    public GameObject worldMapPrefab3D;
    public GameObject hologramContainer;
    

    //interactable spawns in the world
    
    public GameObject mapInteractablePrefab;
    public GameObject[] worldEnemies;

    public int maxTreasureChests;
    public int maxEnemies;
    */


    private void Start()
    {
        //creates a random world at the start of the game
        //StartCoroutine(GenerateNewMap());
    }

    /*
    IEnumerator GenerateNewMap()
    {
        mapGenerationFinished = false;

        GameObject new3DMap = Instantiate(worldMapPrefab3D, hologramContainer.transform.position, Quaternion.identity);
        new3DMap.transform.SetParent(hologramContainer.transform);

        GameObject new2DMap = Instantiate(worldMapPrefab, transform.position, Quaternion.identity);
        new2DMap.transform.SetParent(theWorld.transform);

        offsetX = Random.Range(0f, 99999f);
        offsetZ = Random.Range(0f, 99999f);
        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset, GenerateNoise(x, z, perlinDetail) * noiseHeight,z * gridOffset);
                yield return new WaitForSeconds(0.000000001f);
                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity);
                blockPositions.Add(block.transform.position);
                block.transform.SetParent(new3DMap.transform);

                block.gameObject.layer = 12;

                new3DMap.GetComponent<MapManager3D>().groundTileList.Add(block);

                for (int i = 0; i < worldLayer.Length; i++)
                {
                    if (block.transform.position.y <= worldLayer[i].height)
                    {
                        //block.GetComponent<MeshRenderer>().material = worldLayer[i].color;
                        break;
                    }
                }

                //generate 2D version of world

                GameObject block2D = Instantiate(block, block.transform.position, Quaternion.identity); ;
                block2D.transform.position = new Vector3(block.transform.position.x, 1, block.transform.position.z);

                block2D.transform.SetParent(new2DMap.transform);
                block2D.gameObject.layer = 0;
                new2DMap.GetComponent<MapManager>().groundTileList2D.Add(block);

                
            }
        }

        SpawnObjects(new3DMap);
        //Spawn World Interactables
        //Spawn World Enemies
        //SpawnInteractables(new2DMap);
        gameObject.GetComponent<GameManager>().currentWorld = new2DMap;
        gameObject.GetComponent<GameManager>().worldMaps.Add(new3DMap);
        new2DMap.GetComponent<MapManager>().LoadMapInteractables();
        mapGenerationFinished = true;

    }*/
    /*
    private void WorldConverter2D(Map world3D)
    {
        GameObject new2DMap = Instantiate(worldMapPrefab, transform.position, Quaternion.identity);
        new2DMap.transform.SetParent(theWorld.transform);

        foreach (GameObject block in world3D.groundBlocks)
        {
            GameObject block2D = Instantiate(block, block.transform.position, Quaternion.identity); ;
            block2D.transform.position = new Vector3(block.transform.position.x, 1, block.transform.position.z);

            block2D.transform.SetParent(new2DMap.transform);
            block2D.gameObject.layer = 0;
            new2DMap.GetComponent<MapManager>().groundTileList2D.Add(block);
        }

        
    }*/

    public float GenerateNoise(float x, float z, float detailScale)
    {
        float xCoord = x / worldSizeX * detailScale + offsetX;
        float zCoord = z / worldSizeZ * detailScale + offsetZ;
        
        return Mathf.PerlinNoise(xCoord, zCoord);
    }
    /*
    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count);

        Vector3 newPos = new Vector3(blockPositions[rndIndex].x, blockPositions[rndIndex].y + 1.5f, blockPositions[rndIndex].z);

        if (objectPositions.Contains(newPos))
        {
            ObjectSpawnLocation();
        } else
        {
            objectPositions.Add(newPos);
        }
        return newPos;

    }*/

    /*private void SpawnObjects(GameObject parent)
    {
        for (int i = 0; i < objectLimit; i++)
        {
            GameObject toPlaceObject = Instantiate(spawnObject[Random.Range(0, spawnObject.Length)], ObjectSpawnLocation(), Quaternion.identity);
            toPlaceObject.transform.SetParent(parent.transform);
            parent.GetComponent<MapManager3D>().mapAssets.Add(toPlaceObject);
        }
    }*/

    /*
    public void SpawnInteractables(GameObject map)
    {
        for (int i = 0; i < maxTreasureChests; i++)
        {
            //spawn treasure chests
            //Instantiate(treasureChestPrefab, new Vector3(Random.Range(0,worldSizeX-1), 6, Random.Range(0, worldSizeZ-1)), Quaternion.identity);

            GameObject newTreasureChest = Instantiate(mapInteractablePrefab, new Vector3((int)Random.Range(0, worldSizeX - 1), 6, (int)Random.Range(0, worldSizeZ - 1)), Quaternion.Euler(new Vector3(90,0,0)));
            newTreasureChest.GetComponent<MapInteractable>().ma("TreasureChest"+i, newTreasureChest.transform.position, MapInteractable.InteractableType.TreasureChest);
            map.GetComponent<MapManager>().interactableMapObjects.Add(newTreasureChest);
            newTreasureChest.transform.SetParent(map.transform);
        }

        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject newEnemy = Instantiate(worldEnemies[Random.Range(0, worldEnemies.Length)], new Vector3((int)Random.Range(0, worldSizeX-1), 6, (int)Random.Range(0, worldSizeZ-1)), Quaternion.Euler(new Vector3(90, 0, 0)));
            newEnemy.transform.SetParent(map.transform);

        }
    }*/

    

    public Map GenerateWorld(Map.WorldType worldType)
    {
        objectPositions.Clear();

        List<GameObject> groundBlocks = new List<GameObject>();
        List<Vector3> groundBlocksPos = new List<Vector3>();
        List<Material> groundBlocksMaterial = new List<Material>();
        List<GameObject> assets = new List<GameObject>();
        List<Vector3> assetsPos = new List<Vector3>();
        List<MapInteractable> mapInteractables = new List<MapInteractable>();
        List<Enemy> lv5MapEnemies = new List<Enemy>();
        List<Enemy> lv10MapEnemies = new List<Enemy>();
        List<Enemy> mapBosses = new List<Enemy>();


        offsetX = Random.Range(0f, 99999f);
        offsetZ = Random.Range(0f, 99999f);

        WorldPallet pallet = worldPallets[0];

        for (int i = 0; i < worldPallets.Length; i++)
        {
            if (worldType.ToString() == worldPallets[i].palletName)
            {
                pallet = worldPallets[i];
                break;
            }
        }
        bool dungeon = false;
        if (worldType == Map.WorldType.Dungeon)
        {
            dungeon = true;
        } 

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {

                GameObject block = blockGameObject;
                Vector3 pos = new Vector3(x * gridOffset, GenerateNoise(x, z, perlinDetail) * noiseHeight, z * gridOffset);
                groundBlocksPos.Add(pos);

                for (int i = 0; i < pallet.color.Length; i++)
                {
                    if (pos.y <= worldLayer[i].height)
                    {
                        
                        //block.GetComponent<MeshRenderer>().material = pallet.color[i];
                        groundBlocksMaterial.Add(pallet.color[i]);
                        break;
                    }
                }
                groundBlocks.Add(block);
            }
        }

        //spawn assets
        
        for (int i = 0; i < pallet.assetLimit; i++)
        {
            //Debug.Log(pallet.palletName);
            GameObject newAsset = pallet.assets[Random.Range(0, pallet.assets.Length)];
            int rndIndex = Random.Range(0, groundBlocks.Count);
            /*
            Vector3 newPos = new Vector3(groundBlocks[rndIndex].transform.position.x,
                groundBlocks[rndIndex].transform.position.y + 1.5f,
                groundBlocks[rndIndex].transform.position.z);
            */
            Vector3 newPos = new Vector3(groundBlocksPos[rndIndex].x, groundBlocksPos[rndIndex].y + assetHeight, groundBlocksPos[rndIndex].z);

            if (!objectPositions.Contains(newPos))
            {
                objectPositions.Add(newPos);
                assetsPos.Add(newPos);
                assets.Add(newAsset);
            }
            
            
        }

        //spawn interactables
        
        for (int i = 0; i < pallet.imteractableLimit; i++)
        {


            Vector3 interactablePos = new Vector3((int)Random.Range(0, worldSizeX - 1), 6, (int)Random.Range(0, worldSizeZ - 1));

            var interactableTypeCount = System.Enum.GetValues(typeof(MapInteractable.InteractableType)).Length;
            MapInteractable.InteractableType interactableType = (MapInteractable.InteractableType)Random.Range(0, interactableTypeCount);

            //newInteractable.GetComponent<MapInteractable>().MapInteractableConstruc(interactableType.ToString() + i, interactablePos, interactableType);

            //interactableType.ToString() + i, interactablePos, interactableType

            /*
            GameObject interactableObj = null;
            if (interactableType == MapInteractable.InteractableType.TreasureChest)
            {
                //treasure chest prefab
                interactableObj = treasureChestPrefab;
            }*/

            MapInteractable newInteractable = new MapInteractable(interactableType.ToString() + i, interactablePos, interactableType);
            /*
            newInteractable.interactableName = interactableType.ToString() + i;
            newInteractable.interactablePosition = interactablePos;
            newInteractable.interactableType = interactableType;*/
        
            mapInteractables.Add(newInteractable);
        }



        //set lv 5 enemies;
        
        for (int i = 0; i < pallet.lv5EnemyLimit; i++)
        {
            
            Enemy newEnemy = pallet.lv5Enemies[Random.Range(0, pallet.lv5Enemies.Length)];
            lv5MapEnemies.Add(newEnemy);
            
        }

        //set lv 10 enemies
        for (int i = 0; i < pallet.lv10EnemyLimit; i++)
        {
            
            Enemy newEnemy = pallet.lv10Enemies[Random.Range(0, pallet.lv10Enemies.Length)];
            lv10MapEnemies.Add(newEnemy);

        }

        //set lv 15 boss enemies
        for (int i = 0; i < pallet.enemyBossLimit; i++)
        {
            
            Enemy newEnemy = pallet.bossEnemies[Random.Range(0, pallet.bossEnemies.Length)];
            mapBosses.Add(newEnemy);
        }

        //get pallet particle
        GameObject particleEffect = pallet.particle;

        //generate map name
        int id = GameManager.instance.GetComponent<GameManager>().worldCountID++;

        //int enemyLevelMultiplier = newEnemy.GetComponent<EnemyController>().GetEnemy().IsBoss() ? 10 : Random.Range(1, 5);
        //newEnemy.GetComponent<EnemyController>().GetEnemy().ScaleByLevel(enemyLevelMultiplier + player.GetComponent<PlayerManager>().GetPlayerLevel());

        int mapLevel = GameManager.instance.player.GetComponent<PlayerManager>().GetPlayerLevel();

        Map newMap = new Map("World"+id,groundBlocks,assets,mapInteractables, mapBosses, groundBlocksPos, groundBlocksMaterial, assetsPos, mapLevel,
            worldType, dungeon, particleEffect.gameObject, false, lv5MapEnemies, lv10MapEnemies, pallet.lv5EnemyLimit, pallet.lv10EnemyLimit, pallet.enemyBossLimit);
        GameManager.instance.worldMaps.Add(newMap);
        return newMap;
    }
}

[System.Serializable]
public struct TerrainLayer
{
    public string layerName;
    public float height;
}

[System.Serializable]
public struct WorldPallet
{
    public string palletName;
    public Material[] color;
    public Enemy[] lv5Enemies;
    public Enemy[] lv10Enemies;
    public Enemy[] bossEnemies;
    public GameObject[] assets;
    public GameObject particle;
    public int assetLimit;
    public int imteractableLimit;
    public int lv5EnemyLimit;
    public int lv10EnemyLimit;
    public int enemyBossLimit;

}
